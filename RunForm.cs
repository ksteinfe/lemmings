using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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


            int_params[0].toJSON();
            // setup dicionary, one key for each int_param found
            // see this: http://james.newtonking.com/json
        }



        private void button1_Click(object sender, EventArgs e) {

            Rhino.RhinoApp.WriteLine(this.parent.is_zombie.ToString());
            //this.parent.Params.Output[0].AddVolatileData()


            // TODO: iterate through values of all variables
            int steps = 6;
            Interval ival = this.parent.ivals[0];
            for (int n = 0; n <= steps; n++) {
                this.parent.vars[0] = ival.ParameterAt((double)n/(double)steps);
                this.parent.ExpireSolution(true);
            }

            /*
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
