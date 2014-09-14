using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;

using Rhino.Geometry;

namespace Lemmings {
    public class JSONComponent : GH_Component {

        public String name;
        public String DefaultName = "UNTITLED";
        public bool JSONIsPopulated;
        public String JSONString;


        enum DataStructure { Singleton, List, Tree };

        public JSONComponent()
            : base("JSON Writer", "JSON",
                "Description",
                "Params", "Lemmings") {
                    this.JSONIsPopulated = false;
                    this.JSONString = "";
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager) {
            pManager.AddTextParameter("Key", "K", "The key to assign to the generated JSON array.", GH_ParamAccess.item, this.DefaultName);
            pManager.AddGenericParameter("Values", "V", "Any kind of data (we'll do what we can)", GH_ParamAccess.tree);
            //TODO: add flag to allow/disallow nulls
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager) {
            pManager.AddTextParameter("JSON String", "J", "Resulting JSON string", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA) {
            this.name = this.DefaultName;
            this.JSONIsPopulated = false;
            this.JSONString = "";
            DA.GetData(0, ref name);
            Grasshopper.Kernel.Data.GH_Structure<Grasshopper.Kernel.Types.IGH_Goo> tree = new Grasshopper.Kernel.Data.GH_Structure<Grasshopper.Kernel.Types.IGH_Goo>();
            if (!DA.GetDataTree(1, out tree)) { return; }
            
            
            String JSONOpen = "";
            String JSONClose = "";
            if (name != this.DefaultName) {
                JSONOpen = @"""" + name + @""":{";
                JSONClose = "}";
            }

            DataStructure dim;
            if (tree.PathCount == 1) {
                if (tree.DataCount == 1) dim = DataStructure.Singleton; // a singleton
                else dim = DataStructure.List; // a list
            } else dim = DataStructure.Tree; // a tree
            JSONOpen += @"""DataStructure"":""" + dim.ToString() + @""",";

            String UniformType = "";
            bool IsUniform = JSONConverter.UniformDataType(tree, ref UniformType);
            if (IsUniform) JSONOpen += @"""Type"":""" + UniformType + @""",";

            if (dim == DataStructure.Singleton) {
                // SINGLETONS
                //
                String ObjString = "";
                IGH_Goo obj = tree.get_FirstItem(false);
                if ((obj != null)&&(JSONConverter.ObjectToJSON(obj, ref ObjString,false))){
                    this.JSONString = JSONOpen + @"""Value"":"+ObjString + JSONClose;
                    DA.SetData(0, "{" + this.JSONString + "}");
                    this.JSONIsPopulated = true;
                    return;
                } else {
                    // TODO: return failed item
                    this.JSONIsPopulated = false;
                    return;
                }
            } else if (dim == DataStructure.List) {
                // LISTS
                //

                JSONOpen += @"""Items"":{";
                JSONClose += "}";

                List<IGH_Goo> failures = new List<IGH_Goo>();
                List<String> ObjStrings = JSONConverter.ObjectsToJSON(tree.get_Branch(tree.Paths[0]), ref failures, !IsUniform);
                if (failures.Count > 0) AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Some items failed to be converted to JSON.");
                if (ObjStrings.Count == 0) {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Nothing was successfully converted to JSON.");
                    this.JSONIsPopulated = false;
                    return;
                }

                this.JSONString = JSONOpen + String.Join(",", ObjStrings.ToArray()) + JSONClose;
                DA.SetData(0, "{"+this.JSONString+"}");
                this.JSONIsPopulated = true;
                // TODO: return failed items
                return;
            } else {
                // TREES
                //
                JSONOpen += @"""BranchCount"":" + tree.Branches.Count + @",""DataCount"":" + tree.DataCount + @", ""Paths"":{";
                JSONClose += "}";

                List<IGH_Goo> failures = new List<IGH_Goo>();
                List<String> BranchStrings = new List<string>();

                foreach (Grasshopper.Kernel.Data.GH_Path path in tree.Paths) {
                    String BRANCHOpen = @"""" + path.ToString() + @""":{";
                    String BRANCHClose = "}";

                    List<String> ObjStrings = JSONConverter.ObjectsToJSON(tree.get_Branch(path), ref failures, !IsUniform);
                    if (ObjStrings.Count >= 0) {
                        BranchStrings.Add(BRANCHOpen + String.Join(",", ObjStrings.ToArray()) + BRANCHClose);
                    } else {
                        AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "An entire branch failed to be converted to JSON.");
                    }
                }

                if (failures.Count > 0) AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Some items failed to be converted to JSON.");


                this.JSONString = JSONOpen + String.Join(",", BranchStrings.ToArray()) + JSONClose;
                DA.SetData(0, "{" + this.JSONString + "}");
                this.JSONIsPopulated = true;
                // TODO: return failed items
                return;
            }
            //if (JSONConverter.ObjectToJSON(obj, ref JSONString)) DA.SetData(0, JSONString);
            //DA.SetData(0, tree.PathCount.ToString());
        }

        protected override System.Drawing.Bitmap Icon {
            get {return null;}
        }

        public override Guid ComponentGuid { get { return new Guid("{0f35898d-021f-4093-bc10-d815730eb0cc}"); } }
    }


    public class JSONConverter {

        public static String TypeToString(IGH_Goo obj) {
            String[] split = obj.GetType().ToString().Split('.');
            return split[split.Length - 1];
        }

        public static bool UniformDataType(Grasshopper.Kernel.Data.GH_Structure<Grasshopper.Kernel.Types.IGH_Goo> tree, ref String UniformType) {
            if (tree.get_FirstItem(true) == null) return false;
            Type t = tree.get_FirstItem(true).GetType();
            foreach (IGH_Goo obj in tree.AllData(false)) {
                if (obj == null) return false;
                if (obj.GetType() != t) return false;                
            }
            UniformType = TypeToString(tree.get_FirstItem(true));
            return true;
        }


        public static List<string> ObjectsToJSON(System.Collections.IList objects, ref List<IGH_Goo> failures, bool IncludeType = true, bool AllowNulls = true) {
            List<String> ObjStrings = new List<String>();
            int n = 0;
            foreach (IGH_Goo obj in objects) {
                String str = "";
                if (JSONConverter.ObjectToJSON(obj, ref str, IncludeType, AllowNulls)) ObjStrings.Add(@"""" + n.ToString() + @""":" + str);
                else failures.Add(obj);
                n++;
            }
            return ObjStrings;
        }


        public static bool ObjectToJSON(IGH_Goo obj, ref String str, bool IncludeType = true, bool AllowNulls = true) {
            if (obj == null) {
                if (!AllowNulls) return false;
                str = @"{""Type"":""null""}";
                return true;
            }
            if (obj.GetType() == typeof(GH_Boolean)){
                String val = @"""" + ((GH_Boolean)obj).Value.ToString() + @"""";
                if (IncludeType) str = @"{""Type"":"""+TypeToString(obj)+@""",""Value"":" + val + @"}";
                else str = val;
                return true;
            }
            if (obj.GetType() == typeof(GH_Number)){
                String val = ((GH_Number)obj).Value.ToString();
                if (IncludeType) str = @"{""Type"":""" + TypeToString(obj) + @""",""Value"":" + val + @"}";
                else str = val;
                return true;
            }
            if (obj.GetType() == typeof(GH_Integer)) {
                String val = ((GH_Integer)obj).Value.ToString();
                if (IncludeType) str = @"{""Type"":""" + TypeToString(obj) + @""",""Value"":" + val + @"}";
                else str = val;
                return true;
            }
            if (obj.GetType() == typeof(GH_String)) {
                //TODO: escape bad string characters according to http://stackoverflow.com/questions/3020094/how-should-i-escape-strings-in-json
                String val = @"""" + ((GH_String)obj).Value.ToString().Replace(@"""",@"\""") + @"""";
                if (IncludeType) str = @"{""Type"":""" + TypeToString(obj) + @""",""Value"":" + val + @"}";
                else str = val;
                return true;
            }
            if (obj.GetType() == typeof(GH_Guid)) {
                String val = @"""" + ((GH_Guid)obj).Value.ToString() + @"""";
                if (IncludeType) str = @"{""Type"":""" + TypeToString(obj) + @""",""Value"":" + val + @"}";
                else str = val;
                return true;
            }
            if (obj.GetType() == typeof(GH_Time)) {
                String val = @"""" + ((GH_Time)obj).Value.ToString() + @"""";
                if (IncludeType) str = @"{""Type"":""" + TypeToString(obj) + @""",""Value"":" + val + @"}";
                else str = val;
                return true;
            }
            if (obj.GetType() == typeof(GH_Colour)) {
                if (IncludeType) str = @"{""Type"":""" + TypeToString(obj) + @""",";
                else str = @"{";
                GH_Colour clr = (GH_Colour)obj;
                str += @"""A"":" + clr.Value.A.ToString() + ",";
                str += @"""R"":" + clr.Value.R.ToString() + ",";
                str += @"""G"":" + clr.Value.G.ToString() + ",";
                str += @"""B"":" + clr.Value.B.ToString();
                str += "}";
                return true;
            }
            if (obj.GetType() == typeof(GH_Interval)) {
                if (IncludeType) str = @"{""Type"":""" + TypeToString(obj) + @""",";
                else str = @"{";
                GH_Interval ival = (GH_Interval)obj;
                str += @"""T0"":" + ival.Value.T0.ToString() + ",";
                str += @"""T1"":" + ival.Value.T1.ToString();
                str += "}";
                return true;
            }
            if (obj.GetType() == typeof(GH_Interval2D)) {
                if (IncludeType) str = @"{""Type"":""" + TypeToString(obj) + @""",";
                else str = @"{";
                GH_Interval2D ival = (GH_Interval2D)obj;
                str += @"""U0"":" + ival.Value.U0.ToString() + ",";
                str += @"""U1"":" + ival.Value.U1.ToString() + ",";
                str += @"""V0"":" + ival.Value.V0.ToString() + ",";
                str += @"""V1"":" + ival.Value.V1.ToString();
                str += "}";
                return true;
            }
            return false;
        }

    }
}