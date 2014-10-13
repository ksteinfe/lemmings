namespace Lemmings {
    partial class RunForm {
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.button1 = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.PermutationCountLabel = new System.Windows.Forms.ToolStripLabel();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBoxPNG = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox3dm = new System.Windows.Forms.CheckBox();
            this.checkBoxWRL = new System.Windows.Forms.CheckBox();
            this.checkBoxOBJ = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.toolStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.FlatAppearance.BorderSize = 2;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(147, 232);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Run";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PermutationCountLabel});
            this.toolStrip1.Location = new System.Drawing.Point(0, 258);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(234, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // PermutationCountLabel
            // 
            this.PermutationCountLabel.Name = "PermutationCountLabel";
            this.PermutationCountLabel.Size = new System.Drawing.Size(116, 22);
            this.PermutationCountLabel.Text = "XXXXX Permutations";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(210, 214);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.flowLayoutPanel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(202, 188);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Permutations";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(200, 188);
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(202, 188);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Output";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBoxPNG);
            this.groupBox2.Location = new System.Drawing.Point(6, 62);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(190, 120);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "IMG Formats";
            // 
            // checkBoxPNG
            // 
            this.checkBoxPNG.AutoSize = true;
            this.checkBoxPNG.Location = new System.Drawing.Point(6, 19);
            this.checkBoxPNG.Name = "checkBoxPNG";
            this.checkBoxPNG.Size = new System.Drawing.Size(47, 17);
            this.checkBoxPNG.TabIndex = 1;
            this.checkBoxPNG.Text = ".png";
            this.checkBoxPNG.UseVisualStyleBackColor = true;
            this.checkBoxPNG.CheckedChanged += new System.EventHandler(this.FormatOptionsChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox3dm);
            this.groupBox1.Controls.Add(this.checkBoxWRL);
            this.groupBox1.Controls.Add(this.checkBoxOBJ);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(190, 50);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "3D formats";
            // 
            // checkBox3dm
            // 
            this.checkBox3dm.AutoSize = true;
            this.checkBox3dm.Location = new System.Drawing.Point(103, 19);
            this.checkBox3dm.Name = "checkBox3dm";
            this.checkBox3dm.Size = new System.Drawing.Size(49, 17);
            this.checkBox3dm.TabIndex = 2;
            this.checkBox3dm.Text = ".3dm";
            this.checkBox3dm.UseVisualStyleBackColor = true;
            this.checkBox3dm.CheckedChanged += new System.EventHandler(this.FormatOptionsChanged);
            // 
            // checkBoxWRL
            // 
            this.checkBoxWRL.AutoSize = true;
            this.checkBoxWRL.Location = new System.Drawing.Point(55, 19);
            this.checkBoxWRL.Name = "checkBoxWRL";
            this.checkBoxWRL.Size = new System.Drawing.Size(42, 17);
            this.checkBoxWRL.TabIndex = 1;
            this.checkBoxWRL.Text = ".wrl";
            this.checkBoxWRL.UseVisualStyleBackColor = true;
            this.checkBoxWRL.CheckedChanged += new System.EventHandler(this.FormatOptionsChanged);
            // 
            // checkBoxOBJ
            // 
            this.checkBoxOBJ.AutoSize = true;
            this.checkBoxOBJ.Location = new System.Drawing.Point(6, 19);
            this.checkBoxOBJ.Name = "checkBoxOBJ";
            this.checkBoxOBJ.Size = new System.Drawing.Size(43, 17);
            this.checkBoxOBJ.TabIndex = 0;
            this.checkBoxOBJ.Text = ".obj";
            this.checkBoxOBJ.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(91, 232);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(50, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "Exit";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // RunForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(234, 283);
            this.ControlBox = false;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(250, 317);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(250, 317);
            this.Name = "RunForm";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Lemmings";
            this.TopMost = true;
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel PermutationCountLabel;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBoxOBJ;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBoxPNG;
        private System.Windows.Forms.CheckBox checkBox3dm;
        private System.Windows.Forms.CheckBox checkBoxWRL;
    }
}