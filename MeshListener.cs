using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Lemmings {
    public class MeshListener : NameableComponent {

        List<Mesh> meshes;

        public MeshListener()
            : base("Lemmings Mesh Listener", "MeshListen",
                "Saves a given collection of meshes to a Lemmings ZIP file") {
                    this.meshes = new List<Mesh>();
                    this.namePrefix = "mesh_";
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager) {
            pManager.AddMeshParameter("Mesh", "M", "The collection of meshes to save", GH_ParamAccess.tree);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager) {
        }

        protected override void SolveInstance(IGH_DataAccess DA) {
            Grasshopper.Kernel.Data.GH_Structure<Grasshopper.Kernel.Types.GH_Mesh> meshtree = new Grasshopper.Kernel.Data.GH_Structure<Grasshopper.Kernel.Types.GH_Mesh>();
            if (!DA.GetDataTree(0, out meshtree)) return; 
            this.meshes = new List<Mesh>();
            foreach (GH_Mesh ghmsh in meshtree.FlattenData()) this.meshes.Add(ghmsh.Value);
        }

        public bool DoSave(Dictionary<String, String> DataPaths, String iter, System.Drawing.Size? ViewCaptureSize = null) {
            
            List<Guid> ids = new List<Guid>();
            foreach (Mesh msh in this.meshes) ids.Add(Rhino.RhinoDoc.ActiveDoc.Objects.AddMesh(msh));
            if (DataPaths.ContainsKey("png")) {
                string Path = System.IO.Path.Combine(DataPaths["png"], this.ComponentName + " " + iter + ".png");
                if (ViewCaptureSize.HasValue) CaptureViewToFile(Path, Rhino.RhinoDoc.ActiveDoc.Views.ActiveView, ViewCaptureSize.Value);
            }

            Rhino.RhinoDoc.ActiveDoc.Objects.Select(ids);
            if (DataPaths.ContainsKey("obj")) {
                string Path = System.IO.Path.Combine(DataPaths["obj"], this.ComponentName + " " + iter + ".obj");
                Rhino.RhinoApp.RunScript(@"-Export """ + Path + @""" _Enter _Enter", false);
            }
            if (DataPaths.ContainsKey("3dm")) {
                string Path = System.IO.Path.Combine(DataPaths["3dm"], this.ComponentName + " " + iter + ".3dm");
                Rhino.RhinoApp.RunScript(@"-Export """ + Path + @""" _Enter _Enter", false);
            }
            if (DataPaths.ContainsKey("wrl")) {
                string Path = System.IO.Path.Combine(DataPaths["wrl"], this.ComponentName + " " + iter + ".wrl");
                Rhino.RhinoApp.RunScript(@"-Export """ + Path + @""" _Enter _Enter", false);
            }
            Rhino.RhinoApp.RunScript("Delete", false);
            return true;
        }

        private void CaptureViewToFile(String filepath, Rhino.Display.RhinoView rhview, System.Drawing.Size size) {
            Bitmap bmp = rhview.CaptureToBitmap(size);
            bmp.Save(filepath, System.Drawing.Imaging.ImageFormat.Png);
        }
        
        protected override System.Drawing.Bitmap Icon {
            get { return Lemmings.Properties.Resources.Icons_MeshListener; }
        }

        public override Guid ComponentGuid { get { return new Guid("{18414208-23be-48c5-9259-63f3ea4f62aa}"); } }
    }
}