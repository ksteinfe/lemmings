using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Parameters;

using Rhino.Geometry;


namespace Lemmings
{

    public class LemmingsComponent : GH_Component, IGH_VariableParameterComponent
    {
        public bool is_zombie;
        public double test_var;
        public VarEngine veng;

        public int VariableCount {
            get { return Params.Output.Count; }
        }

        public LemmingsComponent()
            : base("Lemmings", "Lemmings",
                "Description",
                "Params", "Lemmings")
        {
            this.is_zombie = false;
            this.test_var = -1.0;
            this.veng = new VarEngine();
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Variable A", "tA", "Variable A", GH_ParamAccess.item,0.5);
            pManager.AddIntervalParameter("Domain A", "dA", "Domain of Variable A", GH_ParamAccess.item,new Interval(0.0,1.0));
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Variable A", "A", "Variable A", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (!this.is_zombie) {
                // Ensure that no lists or trees are connected
                for (int i = 0; i < Params.Input.Count; i++) {
                    int cnt = Params.Input[i].VolatileData.DataCount;
                    if (cnt > 1) {
                        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Please don't connect lists or trees here! Variable at index " + i + " contains " + cnt.ToString() + " items");
                        return;
                    }
                }

                // Update local vars and ivals from input parameters
                this.veng.Init(this.VariableCount);

                for (int i = 0; i < this.VariableCount; i++) {
                    double t = 0.0;
                    Interval ival = new Interval();
                    if (!DA.GetData(i * 2, ref t)) { return; }
                    if (!DA.GetData(i * 2 + 1, ref ival)) { return; }
                    this.veng.SetVariableAt(i, ival.ParameterAt(t), ival, this.Params.Input[i*2].NickName);
                }
            }
                        
            for (int i = 0; i < Params.Output.Count; i++) DA.SetData(i, this.veng.values[i]);

            //if (this.is_zombie) DA.SetData(1, test_var);
            //else DA.SetData(1, var_aa);
        }

        public override bool AppendMenuItems(ToolStripDropDown menu) {
            // Place a call to the base class to ensure the default parameter menu is still there and operational.
            //base.AppendAdditionalComponentMenuItems(menu);

            // Now insert your own custom menu items.
            //Menu_AppendItem(menu, "1 Parameter", Menu_ChangeParamCount, true, true);
            //Menu_AppendItem(menu, "2 Parameters", Menu_ChangeParamCount, true, false);
            //Menu_AppendItem(menu, "3 Parameters", Menu_ChangeParamCount, true, false);
            //Menu_AppendSeparator(menu);
            Menu_AppendItem(menu, "Run Lemmings!", Menu_RunLemmingsClicked);
            return true;
        }

        private void Menu_RunLemmingsClicked(Object sender, EventArgs e) {
            this.is_zombie = true;
            GH_Document gh_doc = Grasshopper.Instances.ActiveCanvas.Document;

            RunForm frm = new RunForm(gh_doc, this);
            frm.ShowDialog();
            frm.Dispose();
            this.is_zombie = false;
            this.ExpireSolution(true);
        }
        /*
        private void Menu_ChangeParamCount_1(Object sender, EventArgs e) { this.ChangeParamCount(1); }
        private void Menu_ChangeParamCount_2(Object sender, EventArgs e) { this.ChangeParamCount(2); }
        private void Menu_ChangeParamCount_3(Object sender, EventArgs e) { this.ChangeParamCount(3); }

        private void ChangeParamCount(int count) {
            this.paramCount = count;
        }
        */

        #region Methods of IGH_VariableParameterComponent interface


        bool IGH_VariableParameterComponent.CanInsertParameter(GH_ParameterSide side, int index) {
            if (side == GH_ParameterSide.Input && Params.Input.Count < 10 && index == Params.Input.Count) return true;
            return false;
        }

        bool IGH_VariableParameterComponent.CanRemoveParameter(GH_ParameterSide side, int index) {
            if (side == GH_ParameterSide.Input && Params.Input.Count > 2 && index%2==0) return true;
            return false;
        }
        IGH_Param IGH_VariableParameterComponent.CreateParameter(GH_ParameterSide side, int index) {

            String ltr = GH_ComponentParamServer.InventUniqueNickname("ABCDEFGHIJKLMNOPQRSTUVWXYZ", Params.Output);

            Param_Number param = new Param_Number {
                NickName = "t" + ltr,
                Name = "Variable" + ltr,
                Description = Name,
                Access = GH_ParamAccess.item
            };
            param.SetPersistentData(0.5);

            Param_Interval dparam = new Param_Interval {
                NickName = "d" + ltr,
                Name = "Domain" + ltr,
                Description = "Domain of Variable "+ltr,
                Access = GH_ParamAccess.item
            };
            dparam.SetPersistentData(new GH_Interval(new Interval(0.0,1.0)));
            Params.RegisterInputParam(dparam, Params.Input.Count);

            Param_Number oparam = new Param_Number {
                NickName = ltr,
                Name = "Variable" + ltr,
                Description = Name,
                Access = GH_ParamAccess.item
            };
            Params.RegisterOutputParam(oparam, Params.Input.Count);

            return param;
        }

        bool IGH_VariableParameterComponent.DestroyParameter(GH_ParameterSide side, int index) {
            Params.UnregisterInputParameter(Params.Input[index + 1]);
            Params.UnregisterOutputParameter(Params.Output[index/2]);
            return true;
        }

        void IGH_VariableParameterComponent.VariableParameterMaintenance() {
            ExpireSolution(true);
            //this.zdata = new double[this.VariableCount];
            //for (int i = 0; i < Params.Output.Count; i++) this.zdata[i] = IntervalAtInputIndex(i).Mid;
        }


        #endregion

        protected override System.Drawing.Bitmap Icon {
            get {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return null;
            }
        }

        public override Guid ComponentGuid { get { return new Guid("{0b0e7702-1dec-4ce9-be54-bb4813b022a3}"); } }


        

    }
}


