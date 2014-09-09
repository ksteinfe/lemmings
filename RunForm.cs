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
        public GH_Document gh_doc;
        public LemmingsComponent parent;
        //public Rhino.RhinoApp rh_app;

        private List<LemmingsIntParameter> int_params;

        public RunForm(GH_Document doc, LemmingsComponent prnt) {
            InitializeComponent();
            this.gh_doc = doc;
            this.parent = prnt;
            //this.rh_app = app;
            //int n = gh_doc.ObjectCount;

            
            //LemmingsIntParameter test = (LemmingsIntParameter) doc.FindComponent(new LemmingsIntParameter().ComponentGuid);
            //int ee = (int) test.VolatileData.get_Branch(0)[0];

            // finds LemmingsIntParameters
            this.int_params = new List<LemmingsIntParameter>();
            foreach (IGH_DocumentObject docobj in doc.Objects){
                //Rhino.RhinoApp.WriteLine(docobj.ComponentGuid.ToString());
                if (docobj.ComponentGuid == new LemmingsIntParameter().ComponentGuid) int_params.Add((LemmingsIntParameter)docobj);
            }

            for (int i = 0; i < this.parent.veng.VarCount; i++) { 
                VarControl varControlA;
                varControlA = new Lemmings.VarControl(i,this.parent.veng.names[i],this.parent.veng.ivals[i], this.parent.veng.GetStepsAt(i));
                this.flowLayoutPanel1.Controls.Add(varControlA);
                varControlA.CountChangedEvent += new EventHandler(VarControl_CountChanged);
            }

            this.PermutationCountLabel.Text = this.parent.veng.Permutations.Count.ToString() + " Permutations";



            //int_params[0].toJSON();
            // setup dicionary, one key for each int_param found
            // see this: http://james.newtonking.com/json
        }

        protected void VarControl_CountChanged(object sender, EventArgs e) {
            VarControl varControl = (VarControl) sender;
            this.parent.veng.SetStepsAt(varControl.VarIndex,varControl.count);
            this.PermutationCountLabel.Text = this.parent.veng.Permutations.Count.ToString() + " Permutations";
        }


        private void button1_Click(object sender, EventArgs e) {
            Rhino.RhinoApp.WriteLine("isZombie = "+this.parent.is_zombie.ToString());
            String FilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\erase.txt";
            int n = 0;
            foreach (double[] vars in this.parent.veng.Permutations) {
                Stopwatch watch = new Stopwatch();
                watch.Start();

                for (int i = 0; i < vars.Length; i++) this.parent.veng.SetValueAt(i,vars[i]);
                this.parent.ExpireSolution(true);


                // Create a string array that consists of three lines. 
                string[] strings = { "First sr", "Second sr", "Third sr" };
                // WriteAllLines creates a file, writes a collection of strings to the file, 
                // and then closes the file.
                using (StreamWriter sw = File.AppendText(FilePath)) { sw.WriteLine(n.ToString() + " " + String.Join(" - ", strings)); }

                watch.Stop();
                this.PermutationCountLabel.Text = n.ToString() + " / " + this.parent.veng.Permutations.Count.ToString() + " Permutations (last took "+Math.Round((double)(watch.Elapsed.Milliseconds/1000.0),2).ToString()+" s)";
                n += 1;
                this.Refresh();
            }
            this.PermutationCountLabel.Text = n + " Permutations Completed";

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

    }


}
