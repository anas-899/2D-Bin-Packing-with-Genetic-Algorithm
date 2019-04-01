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
    public partial class Settings : Form
    {
        int PopulationSize;
        int NumberOfGenerations;
        int NumOfIndividualToUpdate;
        DataGridView MainDataGrid;

        public Settings(int populationSize, int numberOfGenerations, int numOfIndividualToUpdate, DataGridView mainDataGrid)
        {
            InitializeComponent();
            PopulationNumbertextBox.Text = ProgramSettings.Default.PopulationSize;
            UpdateRatetextBox.Text = ProgramSettings.Default.PercentOfUpdate;
            ColorcheckBox.Checked = Convert.ToBoolean(ProgramSettings.Default.ColorOrNot);
            HideNamesRectanglescheckBox.Checked = Convert.ToBoolean(ProgramSettings.Default.HideModuleNames);
            ShowAllRectanglecheckBox.Checked = Convert.ToBoolean(ProgramSettings.Default.ShowAllRectangleWhileDrawing);

            HeightModuleFirstcheckBox.Checked = Convert.ToBoolean(ProgramSettings.Default.HeightModuleFirst);
            ShortModuleFirstcheckBox.Checked = Convert.ToBoolean(ProgramSettings.Default.ShortModuleFirst);
            WideModuleFirstcheckBox.Checked = Convert.ToBoolean(ProgramSettings.Default.WideModuleFirst);
            LargeModuleFirstcheckBox.Checked = Convert.ToBoolean(ProgramSettings.Default.LargeModuleFirst);
            SmallModuleFirstcheckBox.Checked = Convert.ToBoolean(ProgramSettings.Default.SmallModuleFirst);


            WasteSpacecheckBox.Checked = Convert.ToBoolean(ProgramSettings.Default.SkyLineWasteSpace);
            DrawEachtextBox.Text = ProgramSettings.Default.DrawEachNumGeneration;

            PopulationSize = populationSize;
            NumberOfGenerations = numberOfGenerations;
            NumOfIndividualToUpdate = numOfIndividualToUpdate;
            MainDataGrid = mainDataGrid;
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            try
            {
                if (PopulationNumbertextBox.Text != ""
                     && UpdateRatetextBox.Text != ""
                     && DrawEachtextBox.Text != "")
                {
                ProgramSettings.Default.PopulationSize = PopulationNumbertextBox.Text;
                ProgramSettings.Default.PercentOfUpdate = UpdateRatetextBox.Text;
                ProgramSettings.Default.ColorOrNot = ColorcheckBox.Checked.ToString();
                ProgramSettings.Default.HideModuleNames = HideNamesRectanglescheckBox.Checked.ToString();
                ProgramSettings.Default.ShowAllRectangleWhileDrawing = ShowAllRectanglecheckBox.Checked.ToString();

                ProgramSettings.Default.HeightModuleFirst = HeightModuleFirstcheckBox.Checked.ToString();
                ProgramSettings.Default.ShortModuleFirst = ShortModuleFirstcheckBox.Checked.ToString();
                ProgramSettings.Default.WideModuleFirst = WideModuleFirstcheckBox.Checked.ToString();
                ProgramSettings.Default.LargeModuleFirst = LargeModuleFirstcheckBox.Checked.ToString();
                ProgramSettings.Default.SmallModuleFirst = SmallModuleFirstcheckBox.Checked.ToString();

                ProgramSettings.Default.SkyLineWasteSpace = WasteSpacecheckBox.Checked.ToString();
                ProgramSettings.Default.DrawEachNumGeneration = DrawEachtextBox.Text;

                ProgramSettings.Default.Save();
                this.Close();
                }
               
                PopulationSize = Convert.ToInt32(ProgramSettings.Default.PopulationSize);
                NumberOfGenerations = Convert.ToInt32(ProgramSettings.Default.NumberOfGenerations);
                NumOfIndividualToUpdate = (Convert.ToInt32(ProgramSettings.Default.PercentOfUpdate) * Convert.ToInt32(ProgramSettings.Default.PopulationSize)) / 100;
                MainDataGrid.Rows.Clear();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Be sure that input values are numbers and are not empty");
            }
        }

        private void Canclebutton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
