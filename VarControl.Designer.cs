namespace Lemmings {
    partial class VarControl {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.VarNameLabel = new System.Windows.Forms.Label();
            this.VarCountNumeric = new System.Windows.Forms.NumericUpDown();
            this.VarIvalLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VarCountNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.69388F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65.30612F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.tableLayoutPanel1.Controls.Add(this.VarNameLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.VarCountNumeric, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.VarIvalLabel, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(184, 25);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // VarNameLabel
            // 
            this.VarNameLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.VarNameLabel.AutoSize = true;
            this.VarNameLabel.Location = new System.Drawing.Point(3, 6);
            this.VarNameLabel.Name = "VarNameLabel";
            this.VarNameLabel.Size = new System.Drawing.Size(21, 13);
            this.VarNameLabel.TabIndex = 1;
            this.VarNameLabel.Text = "XX";
            // 
            // VarCountNumeric
            // 
            this.VarCountNumeric.Location = new System.Drawing.Point(134, 3);
            this.VarCountNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.VarCountNumeric.Name = "VarCountNumeric";
            this.VarCountNumeric.Size = new System.Drawing.Size(46, 20);
            this.VarCountNumeric.TabIndex = 3;
            this.VarCountNumeric.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.VarCountNumeric.ValueChanged += new System.EventHandler(this.VarCountNumeric_ValueChanged);
            // 
            // VarIvalLabel
            // 
            this.VarIvalLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.VarIvalLabel.AutoSize = true;
            this.VarIvalLabel.Location = new System.Drawing.Point(48, 6);
            this.VarIvalLabel.Name = "VarIvalLabel";
            this.VarIvalLabel.Size = new System.Drawing.Size(64, 13);
            this.VarIvalLabel.TabIndex = 2;
            this.VarIvalLabel.Text = "[0.0 to 10.0]";
            // 
            // VarControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "VarControl";
            this.Size = new System.Drawing.Size(190, 30);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VarCountNumeric)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label VarNameLabel;
        private System.Windows.Forms.NumericUpDown VarCountNumeric;
        private System.Windows.Forms.Label VarIvalLabel;
    }
}
