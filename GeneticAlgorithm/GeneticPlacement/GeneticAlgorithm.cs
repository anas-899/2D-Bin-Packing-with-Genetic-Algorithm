using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Linq.Expressions;
using System.Reflection;
using GeneticPlacement.SettingFiles;

namespace GeneticPlacement
{
    public class GeneticAlgorithm
    {
        public static bool IS_Finished = false;

        public List<Generation> GenerationCollection;
        int PopulationSize;
        double RateForMutations;
        double RateForCrossover;
        int NumberOfGenerations;
        int NumOfIndividualToUpdate;
        Module MainRectangle;
        List<Module> Rectangles;
        double totalFitness = 0;
        DataGridView MainDataGridView;
        PictureBox PictureBoxDraw;
        Random rand = new Random(DateTime.Now.Millisecond);

        public static Individual Individual_To_Draw;

        double Area_MainRectangle = Math.BigMul(MainForm.MainRectangle.Width, MainForm.MainRectangle.Height);

        SkyLineBinPack SkyLine;

        Label numberGenerationLabel;
        Label bestFitnesslabel;
        Label TimeElapsedlabel;

        ToolStripButton PlayPausetoolStripButton;
        ToolStripProgressBar toolStripProgressBar1;
        ToolStripStatusLabel toolStripStatusLabel1;

        ToolStripDropDownButton toolStripDropDownButton1;
        ToolStripButton DrawCharttoolStripButton;
        ToolStripButton BestPolishtoolStripButton;
        ToolStripButton CrossoverMutationtoolStripButton;
        ToolStripButton Compare2toolStripButton;
        Panel paintPanel;

        CheckBox EnterNewIndividualcheckBox = new CheckBox();

        public GeneticAlgorithm(int populationSize, double rateformutation, double rateforcrossover
            , int numberofgenerations, int numberofindividualtoupdate, Module mainrectangle
            , List<Module> rectangles, DataGridView mainDataGridView, PictureBox pictureBoxDraw
            , Label NumberGenerationLabel, ToolStripButton playPausetoolStripButton, ToolStripProgressBar ToolStripProgressBar1
            , ToolStripStatusLabel ToolStripStatusLabel1, ToolStripDropDownButton ToolStripDropDownButton1
            , ToolStripButton drawCharttoolStripButton, ToolStripButton bestPolishtoolStripButton
            , ToolStripButton crossoverMutationtoolStripButton, ToolStripButton compare2toolStripButton
            , CheckBox enterNewIndividualcheckBox, Label BestFitnesslabel, Panel PaintPanel, Label timeElapsedlabel)
        {
            this.PopulationSize = populationSize;
            this.RateForCrossover = rateforcrossover;
            this.RateForMutations = rateformutation;
            this.NumOfIndividualToUpdate = numberofindividualtoupdate;
            this.NumberOfGenerations = numberofgenerations;
            this.MainRectangle = mainrectangle;
            this.Rectangles = rectangles;
            this.MainDataGridView = mainDataGridView;
            this.PictureBoxDraw = pictureBoxDraw;
            this.PlayPausetoolStripButton = playPausetoolStripButton;
            this.toolStripProgressBar1 = ToolStripProgressBar1;
            this.toolStripStatusLabel1 = ToolStripStatusLabel1;
            this.toolStripDropDownButton1 = ToolStripDropDownButton1;
            this.DrawCharttoolStripButton = drawCharttoolStripButton;
            this.BestPolishtoolStripButton = bestPolishtoolStripButton;
            this.CrossoverMutationtoolStripButton = crossoverMutationtoolStripButton;
            this.Compare2toolStripButton = compare2toolStripButton;
            this.bestFitnesslabel = BestFitnesslabel;
            this.EnterNewIndividualcheckBox = enterNewIndividualcheckBox;

            this.numberGenerationLabel = NumberGenerationLabel;

            this.paintPanel = PaintPanel;

            this.TimeElapsedlabel = timeElapsedlabel;

            GenerationCollection = new List<Generation>();

        }

        private Dictionary<int, Module> FindHeighestModule(Individual Person)
        {
            int height = 0;
            Module m = new Module();
            int index = 0;
            Dictionary<int, Module> dic = new Dictionary<int, Module>();
            for (int i = 0; i < Person.chromosome.Count; i++)
            {
                if (Person.chromosome[i].Height > height)
                {
                    height = Person.chromosome[i].Height;
                    m = Person.chromosome[i];
                    index = i;
                }
            }
            dic.Add(index, m);
            return dic;
        }

        private Dictionary<int, Module> FindShortModule(Individual Person)
        {
            int height = int.MaxValue;
            Module m = new Module();
            int index = 0;
            Dictionary<int, Module> dic = new Dictionary<int, Module>();
            for (int i = 0; i < Person.chromosome.Count; i++)
            {
                if (Person.chromosome[i].Height < height)
                {
                    height = Person.chromosome[i].Height;
                    m = Person.chromosome[i];
                    index = i;
                }
            }
            dic.Add(index, m);
            return dic;
        }

        private Dictionary<int, Module> FindLargeModule(Individual Person)
        {
            int area = 0;
            Module m = new Module();
            int index = 0;
            Dictionary<int, Module> dic = new Dictionary<int, Module>();
            for (int i = 0; i < Person.chromosome.Count; i++)
            {
                if (Person.chromosome[i].Height > area)
                {
                    area = Person.chromosome[i].Height * Person.chromosome[i].Width;
                    m = Person.chromosome[i];
                    index = i;
                }
            }
            dic.Add(index, m);
            return dic;
        }

        private Dictionary<int, Module> FindSmallModule(Individual Person)
        {
            int area = int.MaxValue;
            Module m = new Module();
            int index = 0;
            Dictionary<int, Module> dic = new Dictionary<int, Module>();
            for (int i = 0; i < Person.chromosome.Count; i++)
            {
                if (Person.chromosome[i].Height < area)
                {
                    area = Person.chromosome[i].Height * Person.chromosome[i].Width;
                    m = Person.chromosome[i];
                    index = i;
                }
            }
            dic.Add(index, m);
            return dic;
        }

        private Dictionary<int, Module> FindWideModule(Individual Person)
        {
            int width = 0;
            Module m = new Module();
            int index = 0;
            Dictionary<int, Module> dic = new Dictionary<int, Module>();
            for (int i = 0; i < Person.chromosome.Count; i++)
            {
                if (Person.chromosome[i].Width > width)
                {
                    width = Person.chromosome[i].Width;
                    m = Person.chromosome[i];
                    index = i;
                }
            }
            dic.Add(index, m);
            return dic;
        }

        private Generation Initialization()
        {
            // Random Functions
            var rnd = new Random();

            Generation generation = new Generation();

            Individual individual = new Individual();
            individual.chromosome = new List<Module>();

            /// Try to Remove random individual!! 
            for (int i = 0; i < Rectangles.Count; i++)
            {
                Module new_Item = new Module();
                new_Item.Name = i.ToString();
                new_Item.Width = Rectangles[i].Width;
                new_Item.Height = Rectangles[i].Height;
                int red = rand.Next(0, byte.MaxValue + 1);
                int green = rand.Next(0, byte.MaxValue + 1);
                int blue = rand.Next(0, byte.MaxValue + 1);
                new_Item.brush_color = new System.Drawing.SolidBrush(Color.FromArgb(red, green, blue));
                individual.chromosome.Add(new_Item);
            }

            List<Individual> NewPeople_Settings = new List<Individual>();
            Individual Randomindividual = (Individual)individual.Clone();

            if (Convert.ToBoolean(ProgramSettings.Default.HeightModuleFirst))
            {
                List<Module> NewChromsome = new List<Module>();
                for (int n = 0; n < individual.chromosome.Count; n++)
                {
                    Dictionary<int, Module> dic = FindHeighestModule(Randomindividual);
                    Randomindividual.chromosome.RemoveAt(dic.Keys.ElementAt(0));

                    NewChromsome.Add(dic.Values.ElementAt(0));
                }

                Randomindividual.chromosome = NewChromsome;
                NewPeople_Settings.Add(Randomindividual);
                NewPeople_Settings.Add(Randomindividual);
                NewPeople_Settings.Add(Randomindividual);
            }

            if (Convert.ToBoolean(ProgramSettings.Default.ShortModuleFirst))
            {
                List<Module> NewChromsome = new List<Module>();
                for (int n = 0; n < individual.chromosome.Count; n++)
                {
                    Dictionary<int, Module> dic = FindShortModule(Randomindividual);
                    Randomindividual.chromosome.RemoveAt(dic.Keys.ElementAt(0));

                    NewChromsome.Add(dic.Values.ElementAt(0));
                }

                Randomindividual.chromosome = NewChromsome;
                NewPeople_Settings.Add(Randomindividual);
                NewPeople_Settings.Add(Randomindividual);
                NewPeople_Settings.Add(Randomindividual);
            }

            if (Convert.ToBoolean(ProgramSettings.Default.WideModuleFirst))
            {
                List<Module> NewChromsome = new List<Module>();
                for (int n = 0; n < individual.chromosome.Count; n++)
                {
                    Dictionary<int, Module> dic = FindWideModule(Randomindividual);
                    Randomindividual.chromosome.RemoveAt(dic.Keys.ElementAt(0));

                    NewChromsome.Add(dic.Values.ElementAt(0));
                }

                Randomindividual.chromosome = NewChromsome;
                NewPeople_Settings.Add(Randomindividual);
                NewPeople_Settings.Add(Randomindividual);
                NewPeople_Settings.Add(Randomindividual);
            }

            if (Convert.ToBoolean(ProgramSettings.Default.LargeModuleFirst))
            {
                List<Module> NewChromsome = new List<Module>();
                for (int n = 0; n < individual.chromosome.Count; n++)
                {
                    Dictionary<int, Module> dic = FindLargeModule(Randomindividual);
                    Randomindividual.chromosome.RemoveAt(dic.Keys.ElementAt(0));

                    NewChromsome.Add(dic.Values.ElementAt(0));
                }

                Randomindividual.chromosome = NewChromsome;
                NewPeople_Settings.Add(Randomindividual);
                NewPeople_Settings.Add(Randomindividual);
                NewPeople_Settings.Add(Randomindividual);
            }

            if (Convert.ToBoolean(ProgramSettings.Default.SmallModuleFirst))
            {
                List<Module> NewChromsome = new List<Module>();
                for (int n = 0; n < individual.chromosome.Count; n++)
                {
                    Dictionary<int, Module> dic = FindSmallModule(Randomindividual);
                    Randomindividual.chromosome.RemoveAt(dic.Keys.ElementAt(0));

                    NewChromsome.Add(dic.Values.ElementAt(0));
                }

                Randomindividual.chromosome = NewChromsome;
                NewPeople_Settings.Add(Randomindividual);
                NewPeople_Settings.Add(Randomindividual);
                NewPeople_Settings.Add(Randomindividual);
            }

            List<Individual> NewPeople = new List<Individual>();

            for (int i = 0; i < NewPeople_Settings.Count; i++)
            {
                NewPeople.Add(NewPeople_Settings[i]);
            }

            for (int n = 0; n < PopulationSize - NewPeople_Settings.Count; n++)
            {
                Randomindividual = new Individual();
                Randomindividual.chromosome = individual.chromosome.OrderBy(x => rnd.Next()).ToList();
                NewPeople.Add(Randomindividual);
            }

            generation.People = NewPeople;
            return generation;
        }
        private double CalcFitness(Individual person)
        {
            SkyLine = new SkyLineBinPack(MainForm.MainRectangle.Width, Convert.ToBoolean(ProgramSettings.Default.SkyLineWasteSpace));

            SkyLine.ApplySkyLine(person.chromosome);

            int Out_Module_Area = 0;
            for (int i = 0; i < person.chromosome.Count; i++)
            {
                if (person.chromosome[i].Y + person.chromosome[i].Height > MainForm.MainRectangle.Height && person.chromosome[i].Y <= MainForm.MainRectangle.Height)
                {
                    Out_Module_Area += ((person.chromosome[i].Y + person.chromosome[i].Height) - MainForm.MainRectangle.Height) * person.chromosome[i].Width;
                }
                else if (person.chromosome[i].Y > MainForm.MainRectangle.Height)
                {
                    Out_Module_Area += person.chromosome[i].Height * person.chromosome[i].Width;
                }
            }

            double _fitness = Out_Module_Area;
            return _fitness;
        }

        private void SortByFitness(Generation generation)
        {
            generation.People.Sort(delegate (Individual c1, Individual c2) { return c1.fitness.CompareTo(c2.fitness); });
        }
        private void Evaluate_Population(Generation generation)
        {
            for (int i = 0; i < PopulationSize; i++)
            {
                generation.People[i].fitness = CalcFitness(generation.People[i]);
            }
            SortByFitness(generation);
        }

        private Individual CrossOver(Individual Father, Individual Mother)
        {
            Individual copy_Father = (Individual)Father.Clone();
            Individual copy_Mother = (Individual)Mother.Clone();

            if (rand.NextDouble() < 0.5)
            {
                int index = rand.Next(0, Rectangles.Count - 1);

                copy_Father.chromosome.RemoveRange(index, copy_Father.chromosome.Count - 1 - index);

                bool found = false;
                for (int i = 0; i < copy_Mother.chromosome.Count; i++)
                {
                    found = false;
                    for (int j = 0; j < copy_Father.chromosome.Count; j++)
                    {
                        if (copy_Mother.chromosome[i].Name == copy_Father.chromosome[j].Name)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        copy_Father.chromosome.Insert(index, copy_Mother.chromosome[i]);
                    }
                }
            }
            else
            {
                int index = rand.Next(0, Rectangles.Count - 1);

                copy_Father.chromosome.RemoveRange(0, index);

                bool found = false;
                for (int i = 0; i < copy_Mother.chromosome.Count; i++)
                {
                    found = false;
                    for (int j = 0; j < copy_Father.chromosome.Count; j++)
                    {
                        if (copy_Mother.chromosome[i].Name == copy_Father.chromosome[j].Name)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        copy_Father.chromosome.Insert(0, copy_Mother.chromosome[i]);
                    }
                }
            }

            return copy_Father;
        }

        private Individual CrossOver2(Individual Father, Individual Mother)
        {
            Individual copy_Father = (Individual)Father.Clone();
            Individual copy_Mother = (Individual)Mother.Clone();
            List<Module> temp_modules = new List<Module>();

            if (rand.NextDouble() < 0.5)
            {
                int index = rand.Next(0, Rectangles.Count - 1);
                int index2 = rand.Next(0, Rectangles.Count - 1);

                while (index == index2)
                {
                    index2 = rand.Next(0, Rectangles.Count - 1);
                }

                if (index < index2)
                {
                    temp_modules = copy_Father.chromosome.GetRange(index, index2 - index);
                    copy_Father.chromosome.RemoveRange(index, index2 - index);
                }
                else
                {
                    temp_modules = copy_Father.chromosome.GetRange(index2, index - index2);
                    copy_Father.chromosome.RemoveRange(index2, index - index2);
                }

                copy_Father.chromosome = copy_Father.chromosome.OrderBy(x => rand.Next()).ToList();

                if (index < index2)
                {
                    copy_Father.chromosome.InsertRange(index, temp_modules);
                }
                else
                {
                    copy_Father.chromosome.InsertRange(index2, temp_modules);
                }
            }
            else
            {
                int index = rand.Next(0, Rectangles.Count - 1);
                int index2 = rand.Next(0, Rectangles.Count - 1);

                while (index == index2)
                {
                    index2 = rand.Next(0, Rectangles.Count - 1);
                }

                if (index < index2)
                {
                    temp_modules = copy_Father.chromosome.GetRange(index, index2 - index);
                    copy_Father.chromosome.RemoveRange(index, index2 - index);
                }
                else
                {
                    temp_modules = copy_Father.chromosome.GetRange(index2, index - index2);
                    copy_Father.chromosome.RemoveRange(index2, index - index2);
                }

                temp_modules = temp_modules.OrderBy(x => rand.Next()).ToList();

                if (index < index2)
                {
                    copy_Father.chromosome.InsertRange(index, temp_modules);
                }
                else
                {
                    copy_Father.chromosome.InsertRange(index2, temp_modules);
                }
            }

            return copy_Father;
        }

        private Individual Mutation(Individual individual)
        {
            Individual copy_Individual = (Individual)individual.Clone();
            int probability = rand.Next(1, 3);
            //mutation 1 : randomly exchange two items 0,1,2....n
            if (probability == 1)
            {
                int index = rand.Next(0, Rectangles.Count - 1);
                int index2 = rand.Next(0, Rectangles.Count - 1);
                while (index == index2)
                {
                    index = rand.Next(0, Rectangles.Count - 1);
                }

                //Swap between item in index and index2
                Module tempOperand = copy_Individual.chromosome.ElementAt(index);
                copy_Individual.chromosome[index] = copy_Individual.chromosome.ElementAt(index2);
                copy_Individual.chromosome[index2] = tempOperand;
            }
            //mutation 2 : move an item to another position
            else
            {
                int index = rand.Next(0, Rectangles.Count - 1);
                int index2 = rand.Next(0, Rectangles.Count - 1);
                while (index == index2)
                {
                    index = rand.Next(0, Rectangles.Count - 1);
                }
                Module temp = copy_Individual.chromosome.ElementAt(index);
                copy_Individual.chromosome.RemoveAt(index);
                copy_Individual.chromosome.Insert(index2, temp);
            }

            return copy_Individual;
        }

        public void Draw_Packing_ForGridView(Individual Best_Individual)
        {
            Individual_To_Draw = Best_Individual;

            SkyLine = new SkyLineBinPack(MainForm.MainRectangle.Width, Convert.ToBoolean(ProgramSettings.Default.SkyLineWasteSpace));
            SkyLine.ApplySkyLine(Best_Individual.chromosome);


            double Max_YY = 0;
            for (int i = 0; i < Best_Individual.chromosome.Count; i++)
            {
                if (Best_Individual.chromosome[i].Y + Best_Individual.chromosome[i].Height > Max_YY)
                {
                    Max_YY = Best_Individual.chromosome[i].Y + Best_Individual.chromosome[i].Height;
                }
            }


            double Max_XX = MainRectangle.Width;

            double num = 0;
            double num2 = 0;


            num = double.Parse(((double)paintPanel.Height / Max_YY).ToString()) - 0.05;

            num2 = double.Parse(((double)paintPanel.Width / Max_XX).ToString()) - 0.05;

            if (num > num2)
            {
                num = num2;
            }
            else
            {
                num2 = num;
            }

            double Max_Y = 0;
            for (int i = 0; i < Best_Individual.chromosome.Count; i++)
            {
                if (Best_Individual.chromosome[i].Y * num + Best_Individual.chromosome[i].Height * num > Max_Y)
                {
                    Max_Y = Best_Individual.chromosome[i].Y * num + Best_Individual.chromosome[i].Height * num;
                }
            }

            double floorPlanWidth = MainForm.MainRectangle.Width * num2 + 5;
            double floorPlanHeight = Max_Y;

            PictureBoxDraw.Width = Convert.ToInt32(floorPlanWidth);

            if (floorPlanHeight > MainRectangle.Height)
            {
                PictureBoxDraw.Height = Convert.ToInt32(floorPlanHeight) + 5;
            }
            else
            {
                PictureBoxDraw.Height = MainRectangle.Height + 5;
            }

            Bitmap bmp = new Bitmap(PictureBoxDraw.Width, PictureBoxDraw.Height);
            Brush brush = Brushes.Red;
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            bool color_or_not = Convert.ToBoolean(ProgramSettings.Default.ColorOrNot);
            for (int i = 0; i < Best_Individual.chromosome.Count; i++)
            {
                if (color_or_not)
                {
                    g.FillRectangle((Best_Individual.chromosome[i].brush_color), (float)(Best_Individual.chromosome[i].X * num2), (float)(Best_Individual.chromosome[i].Y * num), (float)(Best_Individual.chromosome[i].Width * num2), (float)(Best_Individual.chromosome[i].Height * num));
                }
                else
                {
                    g.DrawRectangle(new Pen(Color.Green, 2), (float)(Best_Individual.chromosome[i].X * num2), (float)(Best_Individual.chromosome[i].Y * num), (float)(Best_Individual.chromosome[i].Width * num2), (float)(Best_Individual.chromosome[i].Height * num));
                }

                if (!Convert.ToBoolean(ProgramSettings.Default.HideModuleNames))
                    g.DrawString(Best_Individual.chromosome[i].Name, new Font(FontFamily.GenericSerif, 15), brush, new PointF((float)(Best_Individual.chromosome[i].X * num2), (float)(Best_Individual.chromosome[i].Y * num)));
            }
            Rectangle mainRect = new Rectangle((int)(MainRectangle.X * num2), (int)(MainRectangle.Y * num), (int)(MainRectangle.Width * num2), (int)(MainRectangle.Height * num));
            g.DrawRectangle(new Pen(Color.Red, 2), mainRect);

            PictureBoxDraw.Image = bmp;
        }

        public void Draw_Packing(Individual Best_Individual)
        {
            Individual_To_Draw = Best_Individual;

            double Max_YY = 0;
            for (int i = 0; i < Best_Individual.chromosome.Count; i++)
            {
                if (Best_Individual.chromosome[i].Y + Best_Individual.chromosome[i].Height > Max_YY)
                {
                    Max_YY = Best_Individual.chromosome[i].Y + Best_Individual.chromosome[i].Height;
                }
            }

            double Max_XX = MainRectangle.Width;

            double num = 0;
            double num2 = 0;

            if (Convert.ToBoolean(ProgramSettings.Default.ShowAllRectangleWhileDrawing))
                num = double.Parse(((double)paintPanel.Height / Max_YY).ToString()) - 0.05;
            else
                num = double.Parse(((double)paintPanel.Height / MainForm.MainRectangle.Height).ToString()) - 0.05;

            if (Convert.ToBoolean(ProgramSettings.Default.ShowAllRectangleWhileDrawing))
                num2 = double.Parse(((double)paintPanel.Width / Max_XX).ToString()) - 0.05;
            else
                num2 = double.Parse(((double)paintPanel.Width / MainForm.MainRectangle.Width).ToString()) - 0.05;

            if (num > num2)
            {
                num = num2;
            }
            else
            {
                num2 = num;
            }

            double Max_Y = 0;
            for (int i = 0; i < Best_Individual.chromosome.Count; i++)
            {
                if (Best_Individual.chromosome[i].Y * num + Best_Individual.chromosome[i].Height * num > Max_Y)
                {
                    Max_Y = Best_Individual.chromosome[i].Y * num + Best_Individual.chromosome[i].Height * num;
                }
            }

            double floorPlanWidth = MainForm.MainRectangle.Width * num2 + 5;
            double floorPlanHeight = Max_Y;

            PictureBoxDraw.Width = Convert.ToInt32(floorPlanWidth);

            if (floorPlanHeight > MainRectangle.Height)
            {
                PictureBoxDraw.Height = Convert.ToInt32(floorPlanHeight) + 5;
            }
            else
            {
                PictureBoxDraw.Height = MainRectangle.Height + 5;
            }

            Bitmap bmp = new Bitmap(PictureBoxDraw.Width, PictureBoxDraw.Height);
            Brush brush = Brushes.Red;
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            bool color_or_not = Convert.ToBoolean(ProgramSettings.Default.ColorOrNot);
            for (int i = 0; i < Best_Individual.chromosome.Count; i++)
            {
                if (color_or_not)
                {
                    g.FillRectangle((Best_Individual.chromosome[i].brush_color), (float)(Best_Individual.chromosome[i].X * num2), (float)(Best_Individual.chromosome[i].Y * num), (float)(Best_Individual.chromosome[i].Width * num2), (float)(Best_Individual.chromosome[i].Height * num));
                }
                else
                {
                    g.DrawRectangle(new Pen(Color.Green, 2), (float)(Best_Individual.chromosome[i].X * num2), (float)(Best_Individual.chromosome[i].Y * num), (float)(Best_Individual.chromosome[i].Width * num2), (float)(Best_Individual.chromosome[i].Height * num));
                }

                if (!Convert.ToBoolean(ProgramSettings.Default.HideModuleNames))
                    g.DrawString(Best_Individual.chromosome[i].Name, new Font(FontFamily.GenericSerif, 15), brush, new PointF((float)(Best_Individual.chromosome[i].X * num2), (float)(Best_Individual.chromosome[i].Y * num)));
            }
            Rectangle mainRect = new Rectangle((int)(MainRectangle.X * num2), (int)(MainRectangle.Y * num), (int)(MainRectangle.Width * num2), (int)(MainRectangle.Height * num));
            g.DrawRectangle(new Pen(Color.Red, 2), mainRect);

            PictureBoxDraw.Image = bmp;
        }

        public int RouletteWheelSelection(double total_Fitness, Generation generation)
        {
            double rnd;
            int idx = 0;
            rnd = rand.NextDouble() * totalFitness;
            for (idx = 0; idx < PopulationSize && rnd > 0; idx++)
            {
                rnd -= generation.People.ElementAt(idx).fitness;
            }
            return idx - 1;
        }

        public int RouletteSelection(Generation generation)
        {
            double randomFitness = rand.NextDouble() * generation.People[PopulationSize - 1].cost;
            int idx = -1;
            int mid;
            int first = 0;
            int last = PopulationSize - 1;
            mid = (last - first) / 2;

            while (idx == -1 && first <= last)
            {
                if (randomFitness < generation.People.ElementAt(mid).cost)
                {
                    last = mid;
                }
                else if (randomFitness > generation.People.ElementAt(mid).cost)
                {
                    first = mid;
                }
                mid = (first + last) / 2;
                if ((last - first) == 1)
                    idx = last;
            }
            return idx;
        }

        public void Genetic_Algorithm()
        {
            Individual Random_individual;

            // Parent Generation
            Generation generation = Initialization();
            Evaluate_Population(generation);
            GenerationCollection.Add(generation);

            // Childs Generations     
            int Individual_1 = 0;
            int Individual_2 = 0;
            Individual New_Individual;
            Generation NewGeneration;

            int NO_Generation = 0;
            int Draw_Each_Num_Generation = Convert.ToInt32(ProgramSettings.Default.DrawEachNumGeneration);

            int Free_in_Generation = 0;
            if (MainForm.Rectangles.Count < 50)
                Free_in_Generation = 1000;
            else if (MainForm.Rectangles.Count < 100)
                Free_in_Generation = 500;
            else if (MainForm.Rectangles.Count < 200)
                Free_in_Generation = 300;
            else if (MainForm.Rectangles.Count < 400)
                Free_in_Generation = 100;
            else if (MainForm.Rectangles.Count < 1000)
                Free_in_Generation = 75;
            else
                Free_in_Generation = 30;

            while (true)
            {
                IS_Finished = false;

                NO_Generation++;
                int GenFree = 0;
                if (NO_Generation % Free_in_Generation == 0)
                {
                    for (int i = GenFree; i < GenerationCollection.Count - 2; i++)
                    {
                        for (int j = 1; j < GenerationCollection[i].People.Count; j++)
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
                    GenFree += 1000;
                    GC.Collect();
                }
                NewGeneration = (Generation)generation.Clone();
                double tempcost = 0;
                string msg = "";
                for (int i = 0; i < generation.People.Count; i++)
                {
                    tempcost += (double)(1 / generation.People[i].fitness);
                    generation.People[i].cost = tempcost;
                    msg += i.ToString() + generation.People[i].cost.ToString() + "  " + (1 / generation.People[i].fitness).ToString() + Environment.NewLine;
                }

                totalFitness = 0;
                for (int i = 0; i < generation.People.Count; i++)
                {
                    totalFitness += generation.People[i].fitness;
                }

                for (int i = 0; i < NumOfIndividualToUpdate; i++)
                {
                    Individual_1 = RouletteWheelSelection(totalFitness, generation);
                    Individual_2 = RouletteWheelSelection(totalFitness, generation);

                    if (rand.NextDouble() < 0.5)
                        New_Individual = CrossOver(GenerationCollection[GenerationCollection.Count - 1].People.ElementAt(Individual_1), GenerationCollection[GenerationCollection.Count - 1].People.ElementAt(Individual_2));
                    else if (rand.NextDouble() < 0.6)
                        New_Individual = CrossOver2(GenerationCollection[GenerationCollection.Count - 1].People.ElementAt(Individual_1), GenerationCollection[GenerationCollection.Count - 1].People.ElementAt(Individual_2));
                    else
                    {
                        New_Individual = GenerationCollection[GenerationCollection.Count - 1].People.ElementAt(0);
                        Random_individual = new Individual();
                        Random_individual.chromosome = New_Individual.chromosome.OrderBy(x => rand.Next()).ToList();
                        Random_individual.fitness = New_Individual.fitness;
                        New_Individual = (Individual)Random_individual.Clone();
                        Random_individual = null;
                    }

                    if (rand.NextDouble() < 0.8)
                        New_Individual = Mutation(New_Individual);

                    NewGeneration.People.ElementAt(generation.People.Count - 1 - i).chromosome = null;

                    NewGeneration.People.RemoveAt(generation.People.Count - 1 - i);
                    NewGeneration.People.Insert(generation.People.Count - 1 - i, New_Individual);
                    New_Individual.fitness = CalcFitness(New_Individual);

                }
                SortByFitness(NewGeneration);

                if (EnterNewIndividualcheckBox.Checked)
                {
                    int sizeofNew = (20 * Convert.ToInt32(ProgramSettings.Default.PopulationSize)) / 100;

                    NewGeneration.People.RemoveRange(NewGeneration.People.Count - 1 - sizeofNew, sizeofNew);

                    Individual Randomindividual = null;

                    for (int i = 0; i < sizeofNew; i++)
                    {
                        Randomindividual = new Individual();
                        Randomindividual.chromosome = NewGeneration.People[0].chromosome.OrderBy(x => rand.Next()).ToList();
                        Randomindividual.fitness = CalcFitness(Randomindividual);
                        New_Individual = (Individual)Randomindividual.Clone();

                        NewGeneration.People.Insert(NewGeneration.People.Count - 1, New_Individual);

                        Random_individual = null;
                    }
                    SortByFitness(NewGeneration);
                }

                GenerationCollection.Add(NewGeneration);

                generation = NewGeneration;

                MethodInvoker action = delegate
                {
                    numberGenerationLabel.Text = NO_Generation.ToString();
                    bestFitnesslabel.Text = (Math.Round((GenerationCollection[GenerationCollection.Count - 1].People[0].fitness + Area_MainRectangle) / Area_MainRectangle, 3)).ToString() + " %" + "  -  " + GenerationCollection[GenerationCollection.Count - 1].People[0].fitness;
                };
                numberGenerationLabel.BeginInvoke(action);

                Draw_Each_Num_Generation = Convert.ToInt32(ProgramSettings.Default.DrawEachNumGeneration);
                if (NO_Generation % Draw_Each_Num_Generation == 0)
                {
                    MethodInvoker action2 = delegate
                    {
                        Draw_Packing(NewGeneration.People[0]);
                    };
                    PictureBoxDraw.BeginInvoke(action2);
                }

                if (NewGeneration.People[0].fitness <= 0)
                {
                    break;
                }
            }
            IS_Finished = true;

            MethodInvoker action3 = delegate
            {
                numberGenerationLabel.Text = NO_Generation.ToString();
                PlayPausetoolStripButton.Checked = false;
                toolStripProgressBar1.Visible = false;
                toolStripStatusLabel1.Visible = false;

                toolStripDropDownButton1.Enabled = true;
                DrawCharttoolStripButton.Enabled = true;
                BestPolishtoolStripButton.Enabled = true;
                CrossoverMutationtoolStripButton.Enabled = true;
                Compare2toolStripButton.Enabled = true;

                MainForm.stopWatch.Stop();

                TimeSpan ts = MainForm.stopWatch.Elapsed;
                TimeElapsedlabel.Text = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                                         ts.Hours, ts.Minutes, ts.Seconds,
                                         ts.Milliseconds / 10);


                Draw_Packing(GenerationCollection[GenerationCollection.Count - 1].People[0]);
            };
            numberGenerationLabel.BeginInvoke(action3);
            MainForm.oThread.Abort();
        }

        public void FillDataGridView(List<Generation> Generation_Collection)
        {
            int row = 0;
            for (int n = 0; n < Generation_Collection.Count; n++)
            {
                int PolishExpressionLength = Generation_Collection[n].People[0].chromosome.Count;
                string s = "";
                for (int i = 0; i < PolishExpressionLength; i++)
                {
                    s += Generation_Collection[n].People[0].chromosome[i].Name;
                    if (i < PolishExpressionLength - 1)
                    {
                        s += " , ";
                    }
                }
                MainDataGridView.Rows.Add();
                MainDataGridView.Rows[row].Cells["GenerationNumber"].Value = n.ToString();
                MainDataGridView.Rows[row].Cells["PolishExpressionColumn"].Value = s;
                MainDataGridView.Rows[row].Cells["FitnessColumn"].Value = (Math.Round((Generation_Collection[n].People[0].fitness + Area_MainRectangle) / Area_MainRectangle, 3)) + " %" + "   -   " + Generation_Collection[n].People[0].fitness;
                row++;
            }
        }

    }
}
