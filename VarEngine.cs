using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Parameters;

using Rhino.Geometry;

namespace Lemmings {
    public class VarEngine {
        public double[] values;
        public Interval[] ivals;
        public int[] steps;
        public String[] names;

        public static double DefaultValue = 0.5;
        public static Interval DefaultIval = new Interval();
        public static int DefaultSteps = 3;

        public int VarCount {
            get { return this.values.Length; }
        }

        private List<double[]> Perms;
        public List<double[]> Permutations {
            get {
                if (this.Perms.Count == 0) this.UpdatePermutations();
                return this.Perms;
            }
        }

        public VarEngine() {
            this.values = new double[] { 0.5 };
            this.ivals = new Interval[] { new Interval(0, 1) };
            this.steps = new int[] { VarEngine.DefaultSteps };
            this.names = new String[] { "unnamed" };
            this.ExpirePermutations();
        }


        public void Init(int count) {
            this.values = new double[count];
            this.ivals = new Interval[count];
            this.names = new String[count];

            if (this.steps.Length != count) { 
                this.steps = new int[count];
                for (int i=0; i < count; i++) steps[i] = VarEngine.DefaultSteps;
            }
            this.ExpirePermutations();
        }

        public void SetVariableAt(int idx, double dbl, Interval ival, String name) {
            this.values[idx] = dbl;
            this.ivals[idx] = ival;
            this.names[idx] = name;
            this.ExpirePermutations();
        }

        public void SetStepsAt(int idx, int cnt) {
            this.steps[idx] = cnt;
            this.ExpirePermutations();
        }
        public int GetStepsAt(int idx) {
            return this.steps[idx];
        }

        public void SetValueAt(int idx, double dbl) {
            this.values[idx] = dbl;
        }

        private void UpdatePermutations() {
            List<double[]> values = new List<double[]>();

            for (int i = 0; i < this.VarCount; i++) {
                double[] dbls = new double[this.steps[i]+1];
                for (int j = 0; j <= this.steps[i]; j++) dbls[j] = this.ivals[i].ParameterAt((double)j / (double)this.steps[i]);
                values.Add(dbls);
            }
            this.Perms = Recurse(values);
        }

        public void ExpirePermutations() {
            this.Perms = new List<double[]>();
        }

        private static List<double[]> Recurse(List<double[]> given) {

            if (given.Count > 1) {
                List<double[]> ret = new List<double[]>();
                List<double[]> chopped = new List<double[]>(given);
                chopped.RemoveAt(0);

                foreach (double str in given[0]) {
                    List<double[]> frombelow = Recurse(chopped);
                    foreach (double[] strb in frombelow) {
                        List<double> row = new List<double>();
                        row.Add(str);
                        row.AddRange(strb);
                        ret.Add(row.ToArray());
                    }
                }
                return ret;
            } else {
                List<double[]> ret = new List<double[]>();
                foreach (double str in given[0]) { ret.Add(new double[] { str }); }
                return ret;
            }
        }

    }
}
