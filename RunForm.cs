using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

using Rhino;
using Rhino.Geometry;
using Rhino.DocObjects;
using Rhino.Collections;

using GH_IO;
using GH_IO.Serialization;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

namespace Lemmings {
    public partial class RunForm : Form {
        public GH_Document GHDoc;
        public LemmingsComponent LemmingComponentParent;
        //public Rhino.RhinoApp rh_app;

        private List<MeshListener> MeshComponents;
        private List<DataListener> JSONComponents;

        private bool Initializing;

        public RunForm(GH_Document doc, LemmingsComponent prnt) {
            InitializeComponent();
            this.GHDoc = doc;
            this.LemmingComponentParent = prnt;

            this.Initializing = true;
            this.checkBox3dm.Checked = this.LemmingComponentParent.Output3DM;
            this.checkBoxWRL.Checked = this.LemmingComponentParent.OutputWRL;
            this.checkBoxOBJ.Checked = this.LemmingComponentParent.OutputOBJ;
            this.checkBoxPNG.Checked = this.LemmingComponentParent.OutputPNG;
            this.Initializing = false;

            //this.rh_app = app;
            //int n = gh_doc.ObjectCount;

            
            //LemmingsIntParameter test = (LemmingsIntParameter) doc.FindComponent(new LemmingsIntParameter().ComponentGuid);
            //int ee = (int) test.VolatileData.get_Branch(0)[0];

            // finds LemmingsMeshComponents and LemmingsJSONComponents in current document
            this.MeshComponents = new List<MeshListener>();
            this.JSONComponents = new List<DataListener>();
            Guid mshid = new MeshListener().ComponentGuid;
            Guid jsonid = new DataListener().ComponentGuid;
            foreach (IGH_DocumentObject docobj in doc.Objects){
                if (docobj.ComponentGuid == mshid) MeshComponents.Add((MeshListener)docobj);
                if (docobj.ComponentGuid == jsonid) JSONComponents.Add((DataListener)docobj);
            }

            for (int i = 0; i < this.LemmingComponentParent.veng.VarCount; i++) { 
                VarControl varControlA;
                varControlA = new Lemmings.VarControl(i,this.LemmingComponentParent.veng.names[i],this.LemmingComponentParent.veng.ivals[i], this.LemmingComponentParent.veng.GetStepsAt(i));
                this.flowLayoutPanel1.Controls.Add(varControlA);
                varControlA.CountChangedEvent += new EventHandler(VarControl_CountChanged);
            }

            this.PermutationCountLabel.Text = this.LemmingComponentParent.veng.Permutations.Count.ToString() + " Permutations";



            //JSONComponents[0].toJSON();
            // setup dicionary, one key for each int_param found
            // see this: http://james.newtonking.com/json
        }

        protected void VarControl_CountChanged(object sender, EventArgs e) {
            VarControl varControl = (VarControl) sender;
            this.LemmingComponentParent.veng.SetStepsAt(varControl.VarIndex,varControl.count);
            this.PermutationCountLabel.Text = this.LemmingComponentParent.veng.Permutations.Count.ToString() + " Permutations";
        }


        private void RunButton_Click(object sender, EventArgs e) {
            Rhino.RhinoApp.WriteLine("isZombie = " + this.LemmingComponentParent.is_zombie.ToString());
            //String JSONPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\erase.txt";

            GH_PreviewMode prevMode = this.GHDoc.PreviewMode;
            this.GHDoc.PreviewMode = GH_PreviewMode.Disabled;
            Rhino.RhinoDoc.ActiveDoc.Views.RedrawEnabled = false;

            if (folderBrowserDialog1.ShowDialog() != DialogResult.OK) return;
            String SelectedPath = folderBrowserDialog1.SelectedPath;
            String TempPath = System.IO.Path.Combine(SelectedPath, "LemmingsTemp");
            System.IO.Directory.CreateDirectory(TempPath);
            String JSONPath = System.IO.Path.Combine(TempPath, "summary.json");
            System.IO.File.WriteAllLines(JSONPath, new String[] {"{","}"});

            Dictionary<String, String> DataPaths = new Dictionary<string, string>();
            if (this.LemmingComponentParent.Output3DM) {
                String RHPath = System.IO.Path.Combine(TempPath, "3dm");
                System.IO.Directory.CreateDirectory(RHPath);
                DataPaths.Add("3dm", RHPath);
            }
            if (this.LemmingComponentParent.OutputOBJ) { 
                String OBJPath = System.IO.Path.Combine(TempPath, "obj");
                System.IO.Directory.CreateDirectory(OBJPath);
                DataPaths.Add("obj",OBJPath);
            }
            if (this.LemmingComponentParent.OutputWRL) {
                String WRLPath = System.IO.Path.Combine(TempPath, "wrl");
                System.IO.Directory.CreateDirectory(WRLPath);
                DataPaths.Add("wrl", WRLPath);
            }
            if (this.LemmingComponentParent.OutputPNG) {
                String PNGPath = System.IO.Path.Combine(TempPath, "png");
                System.IO.Directory.CreateDirectory(PNGPath);
                DataPaths.Add("png", PNGPath);
            }
            
            int dig = this.LemmingComponentParent.veng.Permutations.Count.ToString().Length;

            int n = 0;
            foreach (double[] vars in this.LemmingComponentParent.veng.Permutations) {
                

                List<String> lines = new List<string>(System.IO.File.ReadAllLines(JSONPath));
                lines.RemoveAt(lines.Count - 1);
                //lines = lines.Where((val, idx) => idx != lines.Count() - 1).ToArray(); // remove last line of existing text file

                // UPDATE GH SOLUTION
                double CalculationTime = UpdateGH(vars);

                // SAVE MESH FILES
                foreach (MeshListener meshcomponent in this.MeshComponents) {
                    meshcomponent.DoSave(DataPaths, n.ToString(new String('0', dig)), new Size(800, 800));
                }

                // WRITE JSON
                if (n>0) lines[lines.Count-1] = lines[lines.Count-1] + ","; // add a comma to the end of the last entry
                System.IO.File.WriteAllLines(JSONPath, lines);
                List<String> dVars = JSONStrings();
                List<String> iVars = vars.Select(dbl => Math.Round(dbl,4).ToString()).ToList();
                for (int i = 0; i < vars.Length; i++) iVars[i] = @""""+this.LemmingComponentParent.veng.names[i] + @""":"+ iVars[i];

                using (StreamWriter sw = File.AppendText(JSONPath)) {
                    sw.Write(@"""p" + n.ToString() + @""":{");
                    sw.Write(@"""CalcTime"":" + CalculationTime.ToString() + @"");
                    sw.Write(@",""IVars"":{" + String.Join(",", iVars) + "}");
                    if (dVars.Count > 0) sw.Write(@",""DVars"":{"+String.Join(",", dVars)+"}");
                    sw.Write("}\n}");
                }
                
                this.PermutationCountLabel.Text = n.ToString() + " / " + this.LemmingComponentParent.veng.Permutations.Count.ToString() + " Permutations (last took "+CalculationTime.ToString()+" s)";
                n += 1;
                this.Refresh();
            }


            string zipname = "Lemmings_"+DateTime.Now.ToString("yyMMddHHmmss")+".zip";
            ZipFile.CreateFromDirectory(TempPath, System.IO.Path.Combine(SelectedPath, zipname));
            Directory.Delete(TempPath, true);

            this.PermutationCountLabel.Text = n + " Permutations Completed";
            this.GHDoc.PreviewMode = prevMode;
            this.GHDoc.ExpirePreview(true);
            Rhino.RhinoDoc.ActiveDoc.Views.RedrawEnabled = true;
            /*
            //this.parent.Params.Output[0].AddVolatileData()

             * // TODO: iterate through values of all variables
            int steps = 6;
            Interval ival = this.parent.ivals[0];
            for (int n = 0; n <= steps; n++) {
                this.parent.vars[0] = ival.ParameterAt((double)n/(double)steps);
                this.parent.ExpireSolution(true);
            }

            
            Grasshopper.Kernel.Special.GH_NumberSlider slider = (Grasshopper.Kernel.Special.GH_NumberSlider)this.gh_doc.Objects[0];
            slider.Slider.Value = (decimal)0.25;

            int d = 2;

            for (int n = 0; n < 4; n++) {
                slider.Slider.Value = (decimal)n;
                //slider.ExpireSolution(true);
            }
             */
        }

        private double UpdateGH(double[] vars) {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < vars.Length; i++) this.LemmingComponentParent.veng.SetValueAt(i, vars[i]);
            this.LemmingComponentParent.ExpireSolution(true);
            watch.Stop();
            double CalculationTime = Math.Round((double)(watch.Elapsed.Milliseconds / 1000.0), 2);
            return CalculationTime;
        }

        private List<string> JSONStrings() {
            List<String> strings = new List<string>();
            int v = 0;
            foreach (DataListener c in JSONComponents) {
                if (c.JSONIsPopulated) {
                    //if (c.namexxx == c.DefaultName) strings.Add(@"""dv" + v + @""":{" + c.JSONString + "}");
                    //else strings.Add(c.JSONString);
                    strings.Add(c.JSONString);
                }
                v++;
            }
            return strings;
        }

        private void button2_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void FormatOptionsChanged(object sender, EventArgs e) {
            if (!this.Initializing) { 
            this.LemmingComponentParent.Output3DM = this.checkBox3dm.Checked;
            this.LemmingComponentParent.OutputWRL = this.checkBoxWRL.Checked;
            this.LemmingComponentParent.OutputOBJ = this.checkBoxOBJ.Checked;
            this.LemmingComponentParent.OutputPNG = this.checkBoxPNG.Checked;
            }
        }



    }


}
