using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Threading;
using System.Linq.Expressions;
using System.Reflection;
using System.IO;
using GeneticPlacement.SettingFiles;

namespace GeneticPlacement
{
    public partial class MainForm : Form
    {
        int PopulationSize = Convert.ToInt32(ProgramSettings.Default.PopulationSize);
        double RateForMutations = Convert.ToDouble(ProgramSettings.Default.RateForMutations);
        double RateForCrossover = Convert.ToDouble(ProgramSettings.Default.RateForCrossover);
        int NumberOfGenerations = Convert.ToInt32(ProgramSettings.Default.NumberOfGenerations);
        int NumOfIndividualToUpdate = (Convert.ToInt32(ProgramSettings.Default.PercentOfUpdate) * Convert.ToInt32(ProgramSettings.Default.PopulationSize)) / 100;

        public static List<Module> Rectangles = new List<Module>();
        public static Module MainRectangle;

        public static System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
        private System.Windows.Forms.Panel Chartpanel;

        GeneticAlgorithm GA;
        List<Generation> GenerationCollection;

        public static Thread oThread;

        void initialize_chart_component()
        {
            this.chartImage = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.Chartpanel = new System.Windows.Forms.Panel();
            this.Chartpanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartImage)).BeginInit();
            // 
            // Chartpanel
            // 
            this.Chartpanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Chartpanel.AutoScroll = true;
            this.Chartpanel.BackColor = System.Drawing.Color.Transparent;
            this.Chartpanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Chartpanel.Controls.Add(this.chartImage);
            this.Chartpanel.Location = new System.Drawing.Point(644, 86);
            this.Chartpanel.Name = "Chartpanel";
            this.Chartpanel.Size = new System.Drawing.Size(610, 450);
            this.Chartpanel.TabIndex = 8;
            // 
            // chartImage
            // 
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend7 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title7 = new System.Windows.Forms.DataVisualization.Charting.Title();
            chartArea1.AxisX.LabelStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            chartArea1.AxisX.ScaleBreakStyle.Enabled = true;
            chartArea1.AxisX.Title = "Generation";
            chartArea1.AxisY.Title = "Fitness";
            chartArea1.Name = "ChartArea1";
            this.chartImage.ChartAreas.Add(chartArea1);
            legend7.Name = "Legend1";
            this.chartImage.Legends.Add(legend7);
            this.chartImage.Location = new System.Drawing.Point(0, 0);
            this.chartImage.Name = "chartImage";
            series7.BorderWidth = 3;
            series7.ChartArea = "ChartArea1";
            series7.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series7.Color = System.Drawing.Color.Red;
            series7.Legend = "Legend1";
            series7.Name = "Fitness";
            this.chartImage.Series.Add(series7);
            this.chartImage.Size = new System.Drawing.Size(605, 445);
            this.chartImage.TabIndex = 0;
            this.chartImage.Text = "chart1";
            title7.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            title7.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            title7.Name = "Title1";
            title7.Text = "Best_Fitness_Of_Each_Generation";
            this.chartImage.Titles.Add(title7);
            this.Controls.Add(this.Chartpanel);
            ((System.ComponentModel.ISupportInitialize)(this.chartImage)).EndInit();
            this.ResumeLayout(false);
        }

        public MainForm()
        {
            InitializeComponent();
            initialize_chart_component();
        }

        private void PlayPausetoolStripButton_Click(object sender, EventArgs e)
        {
            if (Rectangles.Count > 0)
            {
                if (PlayPausetoolStripButton.Checked)
                {
                    stopWatch.Start();

                    UpdateStatus(true);
                    if (oThread.ThreadState == ThreadState.Unstarted)
                    {
                        oThread.Start();
                    }
                    else if (oThread.ThreadState == ThreadState.Suspended)
                    {
                        oThread.Resume();
                    }


                }
                else if (!PlayPausetoolStripButton.Checked)
                {
                    if (oThread.ThreadState == ThreadState.Running)
                    {
                        UpdateStatus(false);

                        oThread.Suspend();

                        stopWatch.Stop();

                        TimeSpan ts = stopWatch.Elapsed;
                        TimeElapsedlabel.Text = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                                                 ts.Hours, ts.Minutes, ts.Seconds,
                                                 ts.Milliseconds / 10);

                        GC.Collect(10, GCCollectionMode.Forced);
                    }
                }
            }
            else
            {
                PlayPausetoolStripButton.Checked = false;
                MessageBox.Show(" You should choose XML file first ");
            }
        }

        private void ProgramsettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings ProgramSettingForm = new Settings(PopulationSize, NumberOfGenerations, NumOfIndividualToUpdate
                                                        , MaindataGridView);
            ProgramSettingForm.ShowDialog();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OpenXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stopWatch.Reset();
            TimeElapsedlabel.Text = "Time";

            if (GenerationCollection != null)
            {
                for (int i = 0; i < GenerationCollection.Count; i++)
                {
                    for (int j = 0; j < GenerationCollection[i].People.Count; j++)
                    {
                        for (int k = 0; k < GenerationCollection[i].People[j].chromosome.Count; k++)
                        {
                            GenerationCollection[i].People[j].chromosome[k] = null;
                            GenerationCollection[i].People[j].chromosome.RemoveAt(k);
                            k = 0;
                        }
                        GenerationCollection[i].People[j] = null;
                        GenerationCollection[i].People.RemoveAt(j);
                        j = 1;
                    }
                }
                GC.Collect();
            }

            List<Module> OpenXmlRectangles = new List<Module>();

            int Width = -1;
            int Height = -1;
            XMLopenFileDialog.Filter = "XML documents (*.xml)|*.xml";
            if (XMLopenFileDialog.ShowDialog() == DialogResult.OK && XMLopenFileDialog.FileName != "")
            {
                XmlTextReader textReader = new XmlTextReader(XMLopenFileDialog.FileName);
                OpenXmlRectangles = new List<Module>();
                while (textReader.Read())
                {
                    XmlNodeType nType = textReader.NodeType;
                    if (nType == XmlNodeType.Element)
                    {
                        if (textReader.Name == "Width" || textReader.Name == "Height")
                        {
                            if (textReader.Name == "Width")
                            {
                                textReader.Read();
                                Width = Convert.ToInt32(textReader.Value);
                            }
                            if (textReader.Name == "Height")
                            {
                                textReader.Read();
                                Height = Convert.ToInt32(textReader.Value);
                            }

                            if (Width != -1 && Height != -1)
                            {
                                Module newItem = new Module();
                                newItem.Width = Width;
                                newItem.Height = Height;
                                OpenXmlRectangles.Add(newItem);
                                Width = -1; Height = -1;
                            }
                        }
                    }
                }
                MainRectangle = new Module();
                MainRectangle.Width = OpenXmlRectangles[0].Width;
                MainRectangle.Height = OpenXmlRectangles[0].Height;
                Rectangles = OpenXmlRectangles.ToList<Module>();
                Rectangles.RemoveAt(0);

                PopulationSize = Convert.ToInt32(ProgramSettings.Default.PopulationSize);
                NumberOfGenerations = Convert.ToInt32(ProgramSettings.Default.NumberOfGenerations);
                NumOfIndividualToUpdate = (Convert.ToInt32(ProgramSettings.Default.PercentOfUpdate) * Convert.ToInt32(ProgramSettings.Default.PopulationSize)) / 100;
                MaindataGridView.Rows.Clear();
                chartImage.Series[0].Points.Clear();

                GA = new GeneticAlgorithm(PopulationSize, RateForMutations, RateForCrossover,
                    NumberOfGenerations, NumOfIndividualToUpdate, MainRectangle, Rectangles, MaindataGridView, pictureBoxDraw,
                    NumberGenerationLabel, PlayPausetoolStripButton, toolStripProgressBar1, toolStripStatusLabel1,
                    toolStripDropDownButton1, DrawCharttoolStripButton, BestPolishtoolStripButton, CrossoverMutationtoolStripButton, Compare2toolStripButton,
                    EnterNewIndividualcheckBox, BestFitnesslabel, panelPaint, TimeElapsedlabel);

                GenerationCollection = GA.GenerationCollection;
                oThread = new Thread(new ThreadStart(GA.Genetic_Algorithm));
            }
        }

        private void BestPolishtoolStripButton_Click(object sender, EventArgs e)
        {
            if (Rectangles.Count > 0)
            {
                if (GenerationCollection != null)
                {
                    GA.FillDataGridView(GenerationCollection);
                }
                else
                {
                    MessageBox.Show(" You should run Genetic Aglorithm first ");
                }
            }
            else
            {
                MessageBox.Show(" you should choose XML file first ");
            }
        }

        private void DrawCharttoolStripButton_Click(object sender, EventArgs e)
        {
            if (Rectangles.Count > 0)
            {
                if (GenerationCollection != null)
                {
                    chartImage.Series[0].Points.Clear();
                    for (int i = 0; i < GenerationCollection.Count; i++)
                    {
                        chartImage.Series[0].Points.AddY(GenerationCollection[i].People[0].fitness);
                    }
                    chartImage.ChartAreas[0].AxisX.TitleFont = new Font(FontFamily.GenericMonospace, 14, FontStyle.Bold);
                    chartImage.ChartAreas[0].AxisY.TitleFont = new Font(FontFamily.GenericMonospace, 14, FontStyle.Bold);
                }
                else
                {
                    MessageBox.Show(" You should run Genetic Aglorithm first ");
                }
            }
            else
            {
                MessageBox.Show(" you should choose XML file first ");
            }
        }

        public void UpdateStatus(bool StartWait)
        {
            toolStripProgressBar1.Visible = StartWait;
            toolStripStatusLabel1.Visible = StartWait;

            toolStripDropDownButton1.Enabled = !StartWait;
            DrawCharttoolStripButton.Enabled = !StartWait;
            BestPolishtoolStripButton.Enabled = !StartWait;
            CrossoverMutationtoolStripButton.Enabled = !StartWait;
            Compare2toolStripButton.Enabled = !StartWait;

        }

        private void MaindataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var selectedCell = MaindataGridView.Rows[e.RowIndex].Cells["PolishExpressionColumn"].Value;
            if (selectedCell == null)
                return;

            string[] All_items = selectedCell.ToString().Split(',');

            Individual individual = new Individual();
            individual.chromosome = new List<Module>();

            Random rand2 = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < All_items.Length; i++)
            {
                Module new_Item = new Module();

                new_Item.Name = All_items[i].Trim();
                new_Item.Width = Rectangles[Convert.ToInt32(All_items[i])].Width;
                new_Item.Height = Rectangles[Convert.ToInt32(All_items[i])].Height;
                int red = rand2.Next(0, byte.MaxValue + 1);
                int green = rand2.Next(0, byte.MaxValue + 1);
                int blue = rand2.Next(0, byte.MaxValue + 1);
                new_Item.brush_color = new System.Drawing.SolidBrush(Color.FromArgb(red, green, blue));
                individual.chromosome.Add(new_Item);

            }
            GA.Draw_Packing_ForGridView(individual);

        }

        private void BenchMarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Module> OpenXmlRectangles = new List<Module>();

            String All_File;
            string[] rects;

            XMLopenFileDialog.Filter = "txt documents (*.txt)|*.txt";
            if (XMLopenFileDialog.ShowDialog() == DialogResult.OK && XMLopenFileDialog.FileName != "")
            {
                StreamReader sr = new StreamReader(XMLopenFileDialog.FileName);

                All_File = sr.ReadToEnd();

                char[] param = { '\n', '\r' };
                rects = All_File.Split(param, StringSplitOptions.RemoveEmptyEntries);

                MainRectangle = new Module();
                MainRectangle.Width = Convert.ToInt32(rects[0].Remove(rects[0].Length - 4));
                MainRectangle.Height = Convert.ToInt32(rects[1].Remove(rects[1].Length - 4));

                string[] line;
                int width;
                int height;
                for (int i = 2; i < rects.Length; i++)
                {
                    line = rects[i].Split(' ');
                    height = Convert.ToInt32(line[0].Trim().Remove(line[0].Length - 4));
                    width = Convert.ToInt32(line[1].Trim().Remove(line[1].Length - 4));

                    Module newItem = new Module();
                    newItem.Width = width;
                    newItem.Height = height;
                    OpenXmlRectangles.Add(newItem);
                }

                Rectangles = OpenXmlRectangles.ToList<Module>();

                double sum_rectangles_Area = 0;
                for (int i = 0; i < Rectangles.Count; i++)
                {
                    sum_rectangles_Area += (Rectangles[i].Width * Rectangles[i].Height);
                }

                double Main_Rectangle_Area = Math.BigMul(MainRectangle.Width, MainRectangle.Height);

                PopulationSize = Convert.ToInt32(ProgramSettings.Default.PopulationSize);
                NumberOfGenerations = Convert.ToInt32(ProgramSettings.Default.NumberOfGenerations);
                NumOfIndividualToUpdate = (Convert.ToInt32(ProgramSettings.Default.PercentOfUpdate) * Convert.ToInt32(ProgramSettings.Default.PopulationSize)) / 100;
                MaindataGridView.Rows.Clear();
                chartImage.Series[0].Points.Clear();

                GA = new GeneticAlgorithm(PopulationSize, RateForMutations, RateForCrossover,
                    NumberOfGenerations, NumOfIndividualToUpdate, MainRectangle, Rectangles, MaindataGridView, pictureBoxDraw,
                    NumberGenerationLabel, PlayPausetoolStripButton, toolStripProgressBar1, toolStripStatusLabel1,
                    toolStripDropDownButton1, DrawCharttoolStripButton, BestPolishtoolStripButton, CrossoverMutationtoolStripButton, Compare2toolStripButton,
                    EnterNewIndividualcheckBox, BestFitnesslabel, panelPaint, TimeElapsedlabel);

                GenerationCollection = GA.GenerationCollection;
                oThread = new Thread(new ThreadStart(GA.Genetic_Algorithm));
            }
        }

        private void ZooMbutton_Click(object sender, EventArgs e)
        {
            Individual Best_Individual = GeneticAlgorithm.Individual_To_Draw;

            double Max_YY = 0;
            for (int i = 0; i < Best_Individual.chromosome.Count; i++)
            {
                if (Best_Individual.chromosome[i].Y + Best_Individual.chromosome[i].Height > Max_YY)
                {
                    Max_YY = Best_Individual.chromosome[i].Y + Best_Individual.chromosome[i].Height;
                }
            }

            double num = double.Parse(ZooMtextBox.Text);

            double Max_Y = 0;
            for (int i = 0; i < Best_Individual.chromosome.Count; i++)
            {
                if (Best_Individual.chromosome[i].Y * num + Best_Individual.chromosome[i].Height * num > Max_Y)
                {
                    Max_Y = Best_Individual.chromosome[i].Y * num + Best_Individual.chromosome[i].Height * num;
                }
            }

            double floorPlanWidth = MainForm.MainRectangle.Width * num;
            double floorPlanHeight = Max_Y;

            pictureBoxDraw.Width = Convert.ToInt32(floorPlanWidth);

            if (floorPlanHeight > MainRectangle.Height)
            {
                pictureBoxDraw.Height = Convert.ToInt32(floorPlanHeight) + 5;
            }
            else
            {
                pictureBoxDraw.Height = MainRectangle.Height + 5;
            }

            Bitmap bmp = new Bitmap(pictureBoxDraw.Width, pictureBoxDraw.Height);
            Brush brush = Brushes.Red;
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            bool color_or_not = Convert.ToBoolean(ProgramSettings.Default.ColorOrNot);
            for (int i = 0; i < Best_Individual.chromosome.Count; i++)
            {
                if (color_or_not)
                {
                    g.FillRectangle((Best_Individual.chromosome[i].brush_color), (float)(Best_Individual.chromosome[i].X * num), (float)(Best_Individual.chromosome[i].Y * num), (float)(Best_Individual.chromosome[i].Width * num), (float)(Best_Individual.chromosome[i].Height * num));
                }
                else
                {
                    g.DrawRectangle(new Pen(Color.Green, 2), (float)(Best_Individual.chromosome[i].X * num), (float)(Best_Individual.chromosome[i].Y * num), (float)(Best_Individual.chromosome[i].Width * num), (float)(Best_Individual.chromosome[i].Height * num));
                }
                g.DrawString(Best_Individual.chromosome[i].Name, new Font(FontFamily.GenericSerif, 15), brush, new PointF((float)(Best_Individual.chromosome[i].X * num), (float)(Best_Individual.chromosome[i].Y * num)));
            }
            Rectangle mainRect = new Rectangle((int)(MainRectangle.X * num), (int)(MainRectangle.Y * num), (int)(MainRectangle.Width * num), (int)(MainRectangle.Height * num));
            g.DrawRectangle(new Pen(Color.Red, 1), mainRect);

            pictureBoxDraw.Image = bmp;
        }

        private void CrossoverMutationtoolStripButton_Click(object sender, EventArgs e)
        {
            CrossOver_Mutation cross_Muatation_Form = new CrossOver_Mutation();
            cross_Muatation_Form.ShowDialog();
        }

        SkyLineBinPack SkyLine;
        private void DrawColorcheckBox()
        {
            Individual Individual_To_Draw = GeneticAlgorithm.Individual_To_Draw;

            SkyLine = new SkyLineBinPack(MainForm.MainRectangle.Width, Convert.ToBoolean(ProgramSettings.Default.SkyLineWasteSpace));
            SkyLine.ApplySkyLine(Individual_To_Draw.chromosome);


            double Max_YY = 0;
            for (int i = 0; i < Individual_To_Draw.chromosome.Count; i++)
            {
                if (Individual_To_Draw.chromosome[i].Y + Individual_To_Draw.chromosome[i].Height > Max_YY)
                {
                    Max_YY = Individual_To_Draw.chromosome[i].Y + Individual_To_Draw.chromosome[i].Height;
                }
            }


            double Max_XX = MainRectangle.Width;

            double num = 0;
            double num2 = 0;

            if (Convert.ToBoolean(ProgramSettings.Default.ShowAllRectangleWhileDrawing))
                num = double.Parse(((double)panelPaint.Height / Max_YY).ToString()) - 0.05;
            else
                num = double.Parse(((double)panelPaint.Height / MainForm.MainRectangle.Height).ToString()) - 0.05;

            if (Convert.ToBoolean(ProgramSettings.Default.ShowAllRectangleWhileDrawing))
                num2 = double.Parse(((double)panelPaint.Width / Max_XX).ToString()) - 0.05;
            else
                num2 = double.Parse(((double)panelPaint.Width / MainForm.MainRectangle.Width).ToString()) - 0.05;


            if (num > num2)
            {
                num = num2;
            }
            else
            {
                num2 = num;
            }

            //double num = double.Parse(textBox1.Text);

            double Max_Y = 0;
            for (int i = 0; i < Individual_To_Draw.chromosome.Count; i++)
            {
                if (Individual_To_Draw.chromosome[i].Y * num + Individual_To_Draw.chromosome[i].Height * num > Max_Y)
                {
                    Max_Y = Individual_To_Draw.chromosome[i].Y * num + Individual_To_Draw.chromosome[i].Height * num;
                }
            }

            double floorPlanWidth = MainForm.MainRectangle.Width * num2 + 5;
            double floorPlanHeight = Max_Y;

            pictureBoxDraw.Width = Convert.ToInt32(floorPlanWidth);

            if (floorPlanHeight > MainRectangle.Height)
            {
                pictureBoxDraw.Height = Convert.ToInt32(floorPlanHeight) + 5;
            }
            else
            {
                pictureBoxDraw.Height = MainRectangle.Height + 5;
            }

            Bitmap bmp = new Bitmap(pictureBoxDraw.Width, pictureBoxDraw.Height);
            Brush brush = Brushes.Red;
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            bool color_or_not = Convert.ToBoolean(ProgramSettings.Default.ColorOrNot);
            for (int i = 0; i < Individual_To_Draw.chromosome.Count; i++)
            {
                if (!ColorcheckBox.Checked)
                    g.FillRectangle((Individual_To_Draw.chromosome[i].brush_color), (float)(Individual_To_Draw.chromosome[i].X * num2), (float)(Individual_To_Draw.chromosome[i].Y * num), (float)(Individual_To_Draw.chromosome[i].Width * num2), (float)(Individual_To_Draw.chromosome[i].Height * num));
                else
                    g.DrawRectangle(new Pen(Color.Green, 2), (float)(Individual_To_Draw.chromosome[i].X * num2), (float)(Individual_To_Draw.chromosome[i].Y * num), (float)(Individual_To_Draw.chromosome[i].Width * num2), (float)(Individual_To_Draw.chromosome[i].Height * num));

                if (HideNamescheckBox.Checked)
                    g.DrawString(Individual_To_Draw.chromosome[i].Name, new Font(FontFamily.GenericSerif, 15), brush, new PointF((float)(Individual_To_Draw.chromosome[i].X * num2), (float)(Individual_To_Draw.chromosome[i].Y * num)));
            }
            Rectangle mainRect = new Rectangle((int)(MainRectangle.X * num2), (int)(MainRectangle.Y * num), (int)(MainRectangle.Width * num2), (int)(MainRectangle.Height * num));
            g.DrawRectangle(new Pen(Color.Red, 2), mainRect);

            pictureBoxDraw.Image = bmp;
        }

        private void HideNamesModulecheckBox()
        {
            Individual Individual_To_Draw = GeneticAlgorithm.Individual_To_Draw;

            SkyLine = new SkyLineBinPack(MainForm.MainRectangle.Width, Convert.ToBoolean(ProgramSettings.Default.SkyLineWasteSpace));
            SkyLine.ApplySkyLine(Individual_To_Draw.chromosome);


            double Max_YY = 0;
            for (int i = 0; i < Individual_To_Draw.chromosome.Count; i++)
            {
                if (Individual_To_Draw.chromosome[i].Y + Individual_To_Draw.chromosome[i].Height > Max_YY)
                {
                    Max_YY = Individual_To_Draw.chromosome[i].Y + Individual_To_Draw.chromosome[i].Height;
                }
            }


            double Max_XX = MainRectangle.Width;

            double num = 0;
            double num2 = 0;

            if (Convert.ToBoolean(ProgramSettings.Default.ShowAllRectangleWhileDrawing))
                num = double.Parse(((double)panelPaint.Height / Max_YY).ToString()) - 0.05;
            else
                num = double.Parse(((double)panelPaint.Height / MainForm.MainRectangle.Height).ToString()) - 0.05;

            if (Convert.ToBoolean(ProgramSettings.Default.ShowAllRectangleWhileDrawing))
                num2 = double.Parse(((double)panelPaint.Width / Max_XX).ToString()) - 0.05;
            else
                num2 = double.Parse(((double)panelPaint.Width / MainForm.MainRectangle.Width).ToString()) - 0.05;


            if (num > num2)
            {
                num = num2;
            }
            else
            {
                num2 = num;
            }

            double Max_Y = 0;
            for (int i = 0; i < Individual_To_Draw.chromosome.Count; i++)
            {
                if (Individual_To_Draw.chromosome[i].Y * num + Individual_To_Draw.chromosome[i].Height * num > Max_Y)
                {
                    Max_Y = Individual_To_Draw.chromosome[i].Y * num + Individual_To_Draw.chromosome[i].Height * num;
                }
            }

            double floorPlanWidth = MainForm.MainRectangle.Width * num2 + 5;
            double floorPlanHeight = Max_Y;

            pictureBoxDraw.Width = Convert.ToInt32(floorPlanWidth);

            if (floorPlanHeight > MainRectangle.Height)
            {
                pictureBoxDraw.Height = Convert.ToInt32(floorPlanHeight) + 5;
            }
            else
            {
                pictureBoxDraw.Height = MainRectangle.Height + 5;
            }

            Bitmap bmp = new Bitmap(pictureBoxDraw.Width, pictureBoxDraw.Height);
            Brush brush = Brushes.Red;
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            bool color_or_not = Convert.ToBoolean(ProgramSettings.Default.ColorOrNot);
            for (int i = 0; i < Individual_To_Draw.chromosome.Count; i++)
            {
                if (!ColorcheckBox.Checked)
                    g.FillRectangle((Individual_To_Draw.chromosome[i].brush_color), (float)(Individual_To_Draw.chromosome[i].X * num2), (float)(Individual_To_Draw.chromosome[i].Y * num), (float)(Individual_To_Draw.chromosome[i].Width * num2), (float)(Individual_To_Draw.chromosome[i].Height * num));
                else
                    g.DrawRectangle(new Pen(Color.Green, 2), (float)(Individual_To_Draw.chromosome[i].X * num2), (float)(Individual_To_Draw.chromosome[i].Y * num), (float)(Individual_To_Draw.chromosome[i].Width * num2), (float)(Individual_To_Draw.chromosome[i].Height * num));

                if (!HideNamescheckBox.Checked)
                    g.DrawString(Individual_To_Draw.chromosome[i].Name, new Font(FontFamily.GenericSerif, 15), brush, new PointF((float)(Individual_To_Draw.chromosome[i].X * num2), (float)(Individual_To_Draw.chromosome[i].Y * num)));
            }
            Rectangle mainRect = new Rectangle((int)(MainRectangle.X * num2), (int)(MainRectangle.Y * num), (int)(MainRectangle.Width * num2), (int)(MainRectangle.Height * num));
            g.DrawRectangle(new Pen(Color.Red, 2), mainRect);

            pictureBoxDraw.Image = bmp;
        }

        private void ColorcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            DrawColorcheckBox();
        }

        private void HideNamescheckBox_CheckedChanged(object sender, EventArgs e)
        {
            HideNamesModulecheckBox();
        }

        private void Compare2toolStripButton_Click(object sender, EventArgs e)
        {
            Compare_2 comp2 = new Compare_2();
            comp2.ShowDialog();
        }

    }
}
