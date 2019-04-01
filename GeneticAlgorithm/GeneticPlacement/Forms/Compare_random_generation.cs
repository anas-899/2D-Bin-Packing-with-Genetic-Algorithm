using GeneticPlacement.SettingFiles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace GeneticPlacement
{
    public partial class Compare_2 : Form
    {
        public List<Generation> GenerationCollection = new List<Generation>();

        List<double> Initialization_Random_Fitness = new List<double>();
        List<double> Initialization_Order_Fitness = new List<double>();

        List<Module> Rectangles = new List<Module>();
        public static Module MainRectangle;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartImage;

        SkyLineBinPack SkyLine;

        int PopulationSize = 0;
        int NumberOfGeneration = 0;
        int UpdatePercentage = 0;

        Random rand = new Random(DateTime.Now.Millisecond);

        void initialize_chart_component()
        {
            this.chartImage = new System.Windows.Forms.DataVisualization.Charting.Chart();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
            ((System.ComponentModel.ISupportInitialize)(this.chartImage)).BeginInit();
            this.toolStrip1.SuspendLayout();
            // 
            // chartImage
            // 
            chartArea2.AxisX.Title = "Generations";
            chartArea2.AxisY.Title = "Fitness";
            chartArea2.Name = "ChartArea1";
            this.chartImage.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chartImage.Legends.Add(legend2);
            this.chartImage.Location = new System.Drawing.Point(81, 82);
            this.chartImage.Name = "chartImage";
            series3.BorderColor = System.Drawing.Color.Red;
            series3.BorderWidth = 3;
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.Color = System.Drawing.Color.Red;
            series3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            series3.Legend = "Legend1";
            series3.Name = "Random";
            series4.BorderWidth = 3;
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series4.Color = System.Drawing.Color.ForestGreen;
            series4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            series4.Legend = "Legend1";
            series4.Name = "Based_on_settings";
            this.chartImage.Series.Add(series3);
            this.chartImage.Series.Add(series4);
            this.chartImage.Size = new System.Drawing.Size(536, 422);
            this.chartImage.TabIndex = 0;
            this.chartImage.Text = "chartImage";
            title2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            title2.Name = "Title1";
            title2.Text = "Check the effect of initial generation (Random/Based_on_settings)";
            this.chartImage.Titles.Add(title2);
            this.Controls.Add(this.chartImage);
            ((System.ComponentModel.ISupportInitialize)(this.chartImage)).EndInit();
        }

        public Compare_2()
        {
            InitializeComponent();
            initialize_chart_component();
        }

        private void OpenNewXMLtoolStripButton_Click(object sender, EventArgs e)
        {
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

                chartImage.Series[0].Points.Clear();
                chartImage.Series[1].Points.Clear();
            }
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

        public int RouletteSelection(Generation generation)
        {
            double randomFitness = rand.NextDouble() * generation.People[PopulationSize - 1].cost;
            int idx = -1;
            int mid;
            int first = 0;
            int last = PopulationSize - 1;
            mid = (last - first) / 2;

            //  ArrayList's BinarySearch is for exact values only
            //  so do this by hand.
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
                //  lies between i and i+1
                if ((last - first) == 1)
                    idx = last;
            }
            return idx;
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

            Individual Randomindividual = (Individual)individual.Clone();

            List<Individual> NewPeople = new List<Individual>();

            for (int n = 0; n < PopulationSize; n++)
            {
                Randomindividual = new Individual();
                Randomindividual.chromosome = individual.chromosome.OrderBy(x => rnd.Next()).ToList();
                NewPeople.Add(Randomindividual);
            }

            generation.People = NewPeople;
            return generation;
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

        private Generation Initialization_With_Order()
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
            NewPeople_Settings.Add(Randomindividual);


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
            NewPeople_Settings.Add(Randomindividual);


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
            NewPeople_Settings.Add(Randomindividual);

            NewChromsome = new List<Module>();
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
            NewPeople_Settings.Add(Randomindividual);

            NewChromsome = new List<Module>();
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
            NewPeople_Settings.Add(Randomindividual);

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
            SkyLine = new SkyLineBinPack(MainRectangle.Width, Convert.ToBoolean(CompareCrossOver_Mutation.Default.SkyLineWasteSpace));

            SkyLine.ApplySkyLine(person.chromosome);

            int Out_Module_Area = 0;
            for (int i = 0; i < person.chromosome.Count; i++)
            {
                if (person.chromosome[i].Y + person.chromosome[i].Height > MainRectangle.Height)
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

        public void Genetic_Algorithm_With_RandomInitalization()
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

            for (int n = 0; n < NumberOfGeneration; n++)
            {
                NewGeneration = (Generation)generation.Clone();
                double tempcost = 0;
                string msg = "";
                for (int i = 0; i < generation.People.Count; i++)
                {
                    tempcost += (double)(1 / generation.People[i].fitness);
                    generation.People[i].cost = tempcost;
                    msg += i.ToString() + generation.People[i].cost.ToString() + "  " + (1 / generation.People[i].fitness).ToString() + Environment.NewLine;
                }

                for (int i = 0; i < UpdatePercentage; i++)
                {
                    Individual_1 = RouletteSelection(generation);
                    Individual_2 = RouletteSelection(generation);
                    while (Individual_1 == Individual_2)
                        Individual_2 = RouletteSelection(generation);


                    // New_Individual = GenerationCollection[GenerationCollection.Count - 1].People.ElementAt(0);
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

                    NewGeneration.People.ElementAt(generation.People.Count - 1 - i).chromosome = null;

                    NewGeneration.People.RemoveAt(generation.People.Count - 1 - i);
                    NewGeneration.People.Insert(generation.People.Count - 1 - i, New_Individual);
                    New_Individual.fitness = CalcFitness(New_Individual);

                }
                SortByFitness(NewGeneration);
                GenerationCollection.Add(NewGeneration);

                Initialization_Random_Fitness.Add(NewGeneration.People[0].fitness);

                generation = NewGeneration;

                if (NewGeneration.People[0].fitness <= 0)
                {
                    break;
                }
            }
        }

        public void Genetic_Algorithm_With_Order_Initialization()
        {
            // Parent Generation - initial Generation
            Generation generation = Initialization_With_Order();
            Evaluate_Population(generation);
            GenerationCollection.Add(generation);

            // Childs Generation            
            int Individual_1 = 0;
            Individual New_Individual;
            Generation NewGeneration;

            for (int n = 0; n < NumberOfGeneration; n++)
            {
                NewGeneration = (Generation)generation.Clone();
                double tempcost = 0;
                string msg = "";
                for (int i = 0; i < generation.People.Count; i++)
                {
                    tempcost += (double)(1 / generation.People[i].fitness);
                    generation.People[i].cost = tempcost;
                    msg += i.ToString() + generation.People[i].cost.ToString() + "  " + (1 / generation.People[i].fitness).ToString() + Environment.NewLine;
                }

                for (int i = 0; i < UpdatePercentage; i++)
                {
                    Individual_1 = RouletteSelection(generation);

                    New_Individual = GenerationCollection[GenerationCollection.Count - 1].People.ElementAt(Individual_1);
                    New_Individual = Mutation(New_Individual);

                    NewGeneration.People.ElementAt(generation.People.Count - 1 - i).chromosome = null;

                    NewGeneration.People.RemoveAt(generation.People.Count - 1 - i);
                    NewGeneration.People.Insert(generation.People.Count - 1 - i, New_Individual);
                    New_Individual.fitness = CalcFitness(New_Individual);

                }
                SortByFitness(NewGeneration);
                GenerationCollection.Add(NewGeneration);

                Initialization_Order_Fitness.Add(NewGeneration.People[0].fitness);

                generation = NewGeneration;

                if (NewGeneration.People[0].fitness <= 0)
                {
                    break;
                }
            }
        }

        private void ExecutetoolStripButton_Click(object sender, EventArgs e)
        {
            chartImage.Series[0].Points.Clear();
            chartImage.Series[1].Points.Clear();

            Initialization_Random_Fitness.Clear();
            Initialization_Order_Fitness.Clear();

            for (int i = 0; i < GenerationCollection.Count - 1; i++)
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

            PopulationSize = Convert.ToInt32(CompareCrossOver_Mutation.Default.PopulationSize);
            NumberOfGeneration = Convert.ToInt32(CompareCrossOver_Mutation.Default.NumberOfGenerations);
            UpdatePercentage = (Convert.ToInt32(CompareCrossOver_Mutation.Default.UpdatePercentage) * Convert.ToInt32(CompareCrossOver_Mutation.Default.PopulationSize)) / 100;

            Genetic_Algorithm_With_RandomInitalization();

            GenerationCollection.Clear();

            Genetic_Algorithm_With_Order_Initialization();

            for (int i = 0; i < Initialization_Random_Fitness.Count; i++)
            {
                chartImage.Series[0].Points.AddY(Initialization_Random_Fitness[i]);
            }

            for (int i = 0; i < Initialization_Order_Fitness.Count; i++)
            {
                chartImage.Series[1].Points.AddY(Initialization_Order_Fitness[i]);
            }
            chartImage.ChartAreas[0].AxisX.TitleFont = new Font(FontFamily.GenericMonospace, 14, FontStyle.Bold);
            chartImage.ChartAreas[0].AxisY.TitleFont = new Font(FontFamily.GenericMonospace, 14, FontStyle.Bold);

        }

        private void settingstoolStripButton_Click(object sender, EventArgs e)
        {
            CrossoverMuatationSettings setting = new CrossoverMuatationSettings();
            setting.ShowDialog();
        }

        private void Compare_2_FormClosing(object sender, FormClosingEventArgs e)
        {
            for (int i = 0; i < GenerationCollection.Count - 1; i++)
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

    }
}
