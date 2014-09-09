using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Rhino.Geometry;

namespace Lemmings {
    public partial class VarControl : UserControl {
        public int VarIndex;
        public int count;
        public VarControl(int idx, String name, Interval ival, int cnt) {
            this.VarIndex = idx;
            this.count = cnt;
            InitializeComponent();
            this.VarNameLabel.Text = name;
            this.VarIvalLabel.Text = "[" + ival.Min + "->" + ival.Max + "]";
            this.VarCountNumeric.Value = this.count;
        }

        public event EventHandler CountChangedEvent;
        private void VarCountNumeric_ValueChanged(object sender, EventArgs e) {
            this.count = (int) this.VarCountNumeric.Value;
            if (this.CountChangedEvent != null) this.CountChangedEvent(this, e); 
        }
    }
}
