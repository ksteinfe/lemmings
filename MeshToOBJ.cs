using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Lemmings {
    public class MeshToOBJComponent : GH_Component {

        List<Mesh> meshes;
        bool isActive;
        String filepath;
        
        public MeshToOBJComponent()
            : base("Mesh to OBJ File", "MeshToOBJ",
                "Saves a given collection of meshes to an OBJ File",
                "Params", "Lemmings") {
                    this.isActive = false;
                    this.filepath = "";
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager) {
            pManager.AddTextParameter("Filepath", "F", "A path to the desired file location",GH_ParamAccess.item);
            //pManager.AddBooleanParameter("IsActive", "B", "Component only saves files is this is set to True", GH_ParamAccess.item, false);
            pManager.AddMeshParameter("Mesh", "M", "The collection of meshes to save", GH_ParamAccess.tree);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager) {
        }

        protected override void SolveInstance(IGH_DataAccess DA) {
            if (!DA.GetData(0, ref this.filepath)) { return; }
            //bool isActive = false;
            //if (!DA.GetData(1, ref isActive)) { return; }
            Grasshopper.Kernel.Data.GH_Structure<Grasshopper.Kernel.Types.GH_Mesh> meshtree = new Grasshopper.Kernel.Data.GH_Structure<Grasshopper.Kernel.Types.GH_Mesh>();
            if (!DA.GetDataTree(1, out meshtree)) { return; }

            this.meshes = new List<Mesh>();
            foreach (GH_Mesh ghmsh in meshtree.FlattenData()) this.meshes.Add(ghmsh.Value);

            if (isActive) DoSave(this.filepath);
            else AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, "No files will be written unless you click on 'Save One File' or 'Save Files Constantly' in the dropdown menu.");
            
        }

        public void DoSave(String filepath) {
            List<Guid> ids = new List<Guid>();
            foreach (Mesh msh in this.meshes) ids.Add(Rhino.RhinoDoc.ActiveDoc.Objects.AddMesh(msh));

            Rhino.RhinoDoc.ActiveDoc.Objects.Select(ids);
            //Rhino.RhinoApp.RunScript("SelAll", true);
            Rhino.RhinoApp.RunScript("-Export " + filepath + " _Enter _Enter", false);
            Rhino.RhinoApp.RunScript("Delete", false);
        }


        public override void AppendAdditionalMenuItems(ToolStripDropDown menu) {
            // Place a call to the base class to ensure the default parameter menu is still there and operational.
            base.AppendAdditionalComponentMenuItems(menu);
            Menu_AppendItem(menu, "Save One File", Menu_SaveOneFileClicked);
            Menu_AppendSeparator(menu);
            Menu_AppendItem(menu, "Save Files Constantly", Menu_SaveFilesConstantlyClicked, true, this.isActive);
            //return true;
        }

        private void Menu_SaveFilesConstantlyClicked(Object sender, EventArgs e) {
            this.isActive = !this.isActive;
            this.ExpireSolution(true);
        }

        private void Menu_SaveOneFileClicked(Object sender, EventArgs e) {
            DoSave(this.filepath);
        }

        protected override System.Drawing.Bitmap Icon {
            get {return null; }
        }

        public override Guid ComponentGuid { get { return new Guid("{18414208-23be-48c5-9259-63f3ea4f62aa}"); } }
    }
}