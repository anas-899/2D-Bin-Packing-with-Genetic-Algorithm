namespace GeneticPlacement
{
    partial class CrossoverMuatationSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CrossoverMuatationSettings));
            this.Canclebutton = new System.Windows.Forms.Button();
            this.Savebutton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.UpdateRatetextBox = new System.Windows.Forms.TextBox();
            this.PopulationNumbertextBox = new System.Windows.Forms.TextBox();
            this.NumberGenerationtextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Canclebutton
            // 
            this.Canclebutton.Image = ((System.Drawing.Image)(resources.GetObject("Canclebutton.Image")));
            this.Canclebutton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Canclebutton.Location = new System.Drawing.Point(302, 161);
            this.Canclebutton.Name = "Canclebutton";
            this.Canclebutton.Size = new System.Drawing.Size(100, 32);
            this.Canclebutton.TabIndex = 2;
            this.Canclebutton.Text = "Close";
            this.Canclebutton.UseVisualStyleBackColor = true;
            this.Canclebutton.Click += new System.EventHandler(this.Canclebutton_Click);
            // 
            // Savebutton
            // 
            this.Savebutton.Image = ((System.Drawing.Image)(resources.GetObject("Savebutton.Image")));
            this.Savebutton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Savebutton.Location = new System.Drawing.Point(144, 161);
            this.Savebutton.Name = "Savebutton";
            this.Savebutton.Size = new System.Drawing.Size(100, 32);
            this.Savebutton.TabIndex = 1;
            this.Savebutton.Text = "Save";
            this.Savebutton.UseVisualStyleBackColor = true;
            this.Savebutton.Click += new System.EventHandler(this.Savebutton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label3.Location = new System.Drawing.Point(12, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(284, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "% of changed chromosome/Generation";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label2.Location = new System.Drawing.Point(65, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(231, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "No. of Chromosome/Generation";
            // 
            // UpdateRatetextBox
            // 
            this.UpdateRatetextBox.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UpdateRatetextBox.Location = new System.Drawing.Point(302, 107);
            this.UpdateRatetextBox.Name = "UpdateRatetextBox";
            this.UpdateRatetextBox.Size = new System.Drawing.Size(190, 23);
            this.UpdateRatetextBox.TabIndex = 3;
            // 
            // PopulationNumbertextBox
            // 
            this.PopulationNumbertextBox.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PopulationNumbertextBox.Location = new System.Drawing.Point(302, 67);
            this.PopulationNumbertextBox.Name = "PopulationNumbertextBox";
            this.PopulationNumbertextBox.Size = new System.Drawing.Size(190, 23);
            this.PopulationNumbertextBox.TabIndex = 4;
            // 
            // NumberGenerationtextBox
            // 
            this.NumberGenerationtextBox.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NumberGenerationtextBox.Location = new System.Drawing.Point(302, 26);
            this.NumberGenerationtextBox.Name = "NumberGenerationtextBox";
            this.NumberGenerationtextBox.Size = new System.Drawing.Size(190, 23);
            this.NumberGenerationtextBox.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(159, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "No. of Generations";
            // 
            // CrossoverMuatationSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(511, 221);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.NumberGenerationtextBox);
            this.Controls.Add(this.UpdateRatetextBox);
            this.Controls.Add(this.PopulationNumbertextBox);
            this.Controls.Add(this.Canclebutton);
            this.Controls.Add(this.Savebutton);
            this.Name = "CrossoverMuatationSettings";
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Canclebutton;
        private System.Windows.Forms.Button Savebutton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox UpdateRatetextBox;
        private System.Windows.Forms.TextBox PopulationNumbertextBox;
        private System.Windows.Forms.TextBox NumberGenerationtextBox;
        private System.Windows.Forms.Label label1;
    }
}