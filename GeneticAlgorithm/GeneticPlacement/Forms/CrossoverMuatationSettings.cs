using GeneticPlacement.SettingFiles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GeneticPlacement
{
    public partial class CrossoverMuatationSettings : Form
    {
        public CrossoverMuatationSettings()
        {
            InitializeComponent();

            NumberGenerationtextBox.Text = CompareCrossOver_Mutation.Default.NumberOfGenerations;
            PopulationNumbertextBox.Text = CompareCrossOver_Mutation.Default.PopulationSize;
            UpdateRatetextBox.Text = CompareCrossOver_Mutation.Default.UpdatePercentage;
        }

        private void Canclebutton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            try
            {
                if (PopulationNumbertextBox.Text != ""
                     && UpdateRatetextBox.Text != ""
                     && NumberGenerationtextBox.Text != "")
                {
                    CompareCrossOver_Mutation.Default.NumberOfGenerations = NumberGenerationtextBox.Text;                    
                    CompareCrossOver_Mutation.Default.PopulationSize = PopulationNumbertextBox.Text;
                    CompareCrossOver_Mutation.Default.UpdatePercentage = UpdateRatetextBox.Text;

                    ProgramSettings.Default.Save();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Be sure that input values are numbers and are not empty");
            }
        }
    }
}
