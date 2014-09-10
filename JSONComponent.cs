using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;

using Rhino.Geometry;

namespace Lemmings {
    public class JSONComponent : GH_Component {
        public JSONComponent()
            : base("JSON Writer", "JSON",
                "Description",
                "Params", "Lemmings") {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager) {
            pManager.AddGenericParameter("Data", "D", "Any kind of data - we'll do what we can", GH_ParamAccess.tree);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager) {
            pManager.AddTextParameter("JSON String", "J", "Resulting JSON string", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA) {
            //Object obj = new Object();
            //if (!DA.GetData(0, ref obj)) return;
            Grasshopper.Kernel.Data.GH_Structure<Grasshopper.Kernel.Types.IGH_Goo> tree = new Grasshopper.Kernel.Data.GH_Structure<Grasshopper.Kernel.Types.IGH_Goo>();
            if (!DA.GetDataTree(0, out tree)) { return; }

            String JSONOpen = @"{""" + "SOMENAME" + @""":{";
            String JSONClose = "}}";

            // FOR SINGLETONS AND LISTS
            if (tree.PathCount == 1) {
                List<IGH_Goo> failures = new List<IGH_Goo>();
                List<String> ObjStrings = JSONConverter.ObjectsToJSON(tree.get_Branch(tree.Paths[0]), ref failures);
                if (failures.Count > 0) AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Some items failed to be converted to JSON.");
                if (ObjStrings.Count == 0) {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Nothing was successfully converted to JSON.");
                    // todo: set local flag showing failure
                    return;
                }
                
                String JSONString = JSONOpen + String.Join(",", ObjStrings.ToArray()) + JSONClose;
                DA.SetData(0, JSONString);
                // todo: store JSON locally
                return;
            } else {
                List<IGH_Goo> failures = new List<IGH_Goo>();
                List<String> BranchStrings = new List<string>();

                JSONOpen += @"""BranchCount"":" + tree.Branches.Count + @",""DataCount"":" + tree.DataCount + @", ""Paths"":{";
                JSONClose += "}";
                foreach (Grasshopper.Kernel.Data.GH_Path path in tree.Paths) {
                    String BRANCHOpen = @"""" + path.ToString() + @""":{";
                    String BRANCHClose = "}";

                    List<String> ObjStrings = JSONConverter.ObjectsToJSON(tree.get_Branch(path), ref failures);
                    if (ObjStrings.Count >= 0) {
                        BranchStrings.Add(BRANCHOpen + String.Join(",", ObjStrings.ToArray()) + BRANCHClose);
                    } else {
                        AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "An entire branch failed to be converted to JSON.");
                    }
                }

                if (failures.Count > 0) AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Some items failed to be converted to JSON.");


                String JSONString = JSONOpen + String.Join(",", BranchStrings.ToArray()) + JSONClose;
                DA.SetData(0, JSONString);
                // todo: store JSON locally
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

        public static List<string> ObjectsToJSON(System.Collections.IList objects, ref List<IGH_Goo> failures) {
            List<String> ObjStrings = new List<String>();
            foreach (IGH_Goo obj in objects) {
                String str = "";
                if (JSONConverter.ObjectToJSON(obj, ref str)) ObjStrings.Add(str);
                else failures.Add(obj);
            }
            return ObjStrings;
        }


        public static bool ObjectToJSON(IGH_Goo obj, ref String str) {
            if (obj == null) return false;
            if (obj.GetType() == typeof(GH_Boolean)){
                str = @"""GH_Boolean"":{";
                str += @"""Value"":""" + ((GH_Boolean)obj).Value.ToString() + @"""";
                str += "}";
                return true;
            }
            if (obj.GetType() == typeof(GH_Number)){
                str = @"""GH_Number"":{";
                str += @"""Value"":" + ((GH_Number)obj).Value.ToString();
                str += "}";
                return true;
            }
            if (obj.GetType() == typeof(GH_Integer)) {
                str = @"""GH_Integer"":{";
                str += @"""Value"":" + ((GH_Integer)obj).Value.ToString();
                str += "}";
                return true;
            }
            if (obj.GetType() == typeof(GH_String)) {
                str = @"""GH_String"":{";
                str += @"""Value"":""" + ((GH_String)obj).Value.ToString() + @"""";
                str += "}";
                return true;
            }
            if (obj.GetType() == typeof(GH_Colour)) {
                str = @"""GH_Colour"":{";
                GH_Colour clr = (GH_Colour)obj;
                str += @"""A"":" + clr.Value.A.ToString() + ",";
                str += @"""R"":" + clr.Value.R.ToString() + ",";
                str += @"""G"":" + clr.Value.G.ToString() + ",";
                str += @"""B"":" + clr.Value.B.ToString();
                str += "}";
                return true;
            }
            if (obj.GetType() == typeof(GH_Interval)) {
                str = @"""GH_Interval"":{";
                GH_Interval ival = (GH_Interval)obj;
                str += @"""T0"":" + ival.Value.T0.ToString() + ",";
                str += @"""T1"":" + ival.Value.T1.ToString();
                str += "}";
                return true;
            }
            if (obj.GetType() == typeof(GH_Interval2D)) {
                str = @"""GH_Interval2D"":{";
                GH_Interval2D ival = (GH_Interval2D)obj;
                str += @"""U0"":" + ival.Value.U0.ToString() + ",";
                str += @"""U1"":" + ival.Value.U1.ToString() + ",";
                str += @"""V0"":" + ival.Value.V0.ToString() + ",";
                str += @"""V1"":" + ival.Value.V1.ToString();
                str += "}";
                return true;
            }
            if (obj.GetType() == typeof(GH_Guid)) {
                str = @"""GH_Guid"":{";
                str += @"""Value"":""" + ((GH_Guid)obj).Value.ToString() + @"""";
                str += "}";
                return true;
            }
            if (obj.GetType() == typeof(GH_Time)) {
                str = @"""GH_Time"":{";
                str += @"""Value"":""" + ((GH_Time)obj).Value.ToString() + @"""";
                str += "}";
                return true;
            }
            return false;
        }

    }
}