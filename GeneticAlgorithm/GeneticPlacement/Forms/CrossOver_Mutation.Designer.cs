namespace GeneticPlacement
{
    partial class CrossOver_Mutation
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CrossOver_Mutation));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.OpenNewXMLtoolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.settingstoolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ExecutetoolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.XMLopenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
           
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenNewXMLtoolStripButton,
            this.toolStripSeparator1,
            this.settingstoolStripButton,
            this.toolStripSeparator2,
            this.ExecutetoolStripButton,
            this.toolStripSeparator3});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.toolStrip1.Size = new System.Drawing.Size(699, 80);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // OpenNewXMLtoolStripButton
            // 
            this.OpenNewXMLtoolStripButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.OpenNewXMLtoolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("OpenNewXMLtoolStripButton.Image")));
            this.OpenNewXMLtoolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.OpenNewXMLtoolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.OpenNewXMLtoolStripButton.Name = "OpenNewXMLtoolStripButton";
            this.OpenNewXMLtoolStripButton.Size = new System.Drawing.Size(64, 77);
            this.OpenNewXMLtoolStripButton.Text = "Choose XML";
            this.OpenNewXMLtoolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.OpenNewXMLtoolStripButton.Click += new System.EventHandler(this.OpenNewXMLtoolStripButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 80);
            // 
            // settingstoolStripButton
            // 
            this.settingstoolStripButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.settingstoolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("settingstoolStripButton.Image")));
            this.settingstoolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.settingstoolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.settingstoolStripButton.Name = "settingstoolStripButton";
            this.settingstoolStripButton.Size = new System.Drawing.Size(64, 77);
            this.settingstoolStripButton.Text = "Settings";
            this.settingstoolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.settingstoolStripButton.Click += new System.EventHandler(this.settingstoolStripButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 80);
            // 
            // ExecutetoolStripButton
            // 
            this.ExecutetoolStripButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.ExecutetoolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("ExecutetoolStripButton.Image")));
            this.ExecutetoolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ExecutetoolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ExecutetoolStripButton.Name = "ExecutetoolStripButton";
            this.ExecutetoolStripButton.Size = new System.Drawing.Size(64, 77);
            this.ExecutetoolStripButton.Text = "Run";
            this.ExecutetoolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.ExecutetoolStripButton.Click += new System.EventHandler(this.ExecutetoolStripButton_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 80);
            // 
            // XMLopenFileDialog
            // 
            this.XMLopenFileDialog.FileName = "openFileDialog1";
            // 
            // CrossOver_Mutation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(699, 516);
            this.Controls.Add(this.toolStrip1);
            this.Name = "CrossOver_Mutation";
            this.Text = "Comparison between Mutation and Crossover";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CrossOver_Mutation_FormClosing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton OpenNewXMLtoolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton settingstoolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton ExecutetoolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.OpenFileDialog XMLopenFileDialog;
    }
}