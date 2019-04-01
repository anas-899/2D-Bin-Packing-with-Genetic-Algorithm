using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeneticPlacement
{
    public class SkyLineBinPack
    {
        public class SkylineNode
        {
            /// The starting x-coordinate (leftmost).
            public int x;

            /// The y-coordinate of the skyline level line.
            public int y;

            /// The line width. The ending coordinate (inclusive) will be x+width-1.
            public int width;
        };

        public SkyLineBinPack(int binWidth, bool useWasteMap_)
        {
            BinWidth = binWidth;
            UseWasteMap = useWasteMap_;
            skyLine.Clear();
            SkylineNode first = new SkylineNode();
            first.x = 0;
            first.y = 0;
            first.width = binWidth;
            skyLine.Add(first);

            if (UseWasteMap)
            {
                wasteMap.Clear();
            }
        }

        public List<SkylineNode> skyLine = new List<SkylineNode>();

        public List<Module> wasteMap = new List<Module>();

        public int BinWidth;

        public bool UseWasteMap;

        private bool RectangleFits(int skylineNodeIndex, int width, int height, ref int y)
        {
            int x = skyLine[skylineNodeIndex].x;
            if (x + width > BinWidth)
                return false;
            int widthLeft = width;
            int i = skylineNodeIndex;
            y = skyLine[skylineNodeIndex].y;
            while (widthLeft > 0)
            {
                y = max(y, skyLine[i].y);
                widthLeft -= skyLine[i].width;
                ++i;
            }
            return true;
        }

        public void AddSkylineLevel(int skylineNodeIndex, ref Module rect)
        {
            if (UseWasteMap)
                AddWasteMapArea(skylineNodeIndex, rect.Width, rect.Height, rect.Y);

            SkylineNode newNode = new SkylineNode();
            newNode.x = rect.X;
            newNode.y = rect.Y + rect.Height;
            newNode.width = rect.Width;
            skyLine.Insert(skylineNodeIndex, newNode);

            for (int i = skylineNodeIndex + 1; i < skyLine.Count; ++i)
            {
                if (skyLine[i].x < skyLine[i - 1].x + skyLine[i - 1].width)
                {
                    int shrink = skyLine[i - 1].x + skyLine[i - 1].width - skyLine[i].x;

                    skyLine[i].x += shrink;
                    skyLine[i].width -= shrink;

                    if (skyLine[i].width <= 0)
                    {
                        skyLine.RemoveAt(i);
                        --i;
                    }
                    else
                        break;
                }
                else
                    break;
            }
            MergeSkylines();
        }

        private void MergeSkylines()
        {
            for (int i = 0; i < skyLine.Count - 1; ++i)
                if (skyLine[i].y == skyLine[i + 1].y)
                {
                    skyLine[i].width += skyLine[i + 1].width;
                    skyLine.RemoveAt(i + 1);
                    --i;
                }
        }

        public Module InsertBottomLeft(int width, int height)
        {
            int bestHeight;
            int bestWidth;
            int bestIndex;
            Module newNode = FindPositionForNewNodeBottomLeft(width, height, out bestHeight, out bestWidth, out bestIndex);

            if (bestIndex != -1)
            {
                AddSkylineLevel(bestIndex, ref newNode);
            }
            return newNode;
        }

        public Module FindPositionForNewNodeBottomLeft(int width, int height, out int bestHeight, out int bestWidth, out int bestIndex)
        {
            bestHeight = int.MaxValue;
            bestIndex = -1;
            bestWidth = int.MaxValue;
            Module newNode = new Module();

            for (int i = 0; i < skyLine.Count; ++i)
            {
                int y = 0;
                if (RectangleFits(i, width, height, ref y))
                {
                    if (y + height < bestHeight || (y + height == bestHeight && skyLine[i].width < bestWidth))
                    {
                        bestHeight = y + height;
                        bestIndex = i;
                        bestWidth = skyLine[i].width;
                        newNode.X = skyLine[i].x;
                        newNode.Y = y;
                        newNode.Width = width;
                        newNode.Height = height;
                    }
                }
            }

            return newNode;

        }

        public void ApplySkyLine(List<Module> AfterChange)
        {
            if (!UseWasteMap)
            {
                Module temp = new Module();
                for (int i = 0; i < AfterChange.Count; i++)
                {
                    Module m1 = AfterChange.ElementAt(i);
                    temp = InsertBottomLeft(m1.Width, m1.Height);
                    m1.X = temp.X;
                    m1.Y = temp.Y;
                }
            }
            else
            {
                Module temp = new Module();

                for (int i = 0; i < AfterChange.Count; i++)
                {
                    Module m1 = AfterChange.ElementAt(i);
                    temp = InsertMinWaste(m1.Width, m1.Height);
                    m1.X = temp.X;
                    m1.Y = temp.Y;
                }
            }
        }

        public int ComputeWastedArea(int skylineNodeIndex, int width, int height, int y)
        {
            int wastedArea = 0;
            int rectLeft = skyLine[skylineNodeIndex].x;
            int rectRight = rectLeft + width;
            for (; skylineNodeIndex < (int)skyLine.Count && skyLine[skylineNodeIndex].x < rectRight; ++skylineNodeIndex)
            {
                if (skyLine[skylineNodeIndex].x >= rectRight || skyLine[skylineNodeIndex].x + skyLine[skylineNodeIndex].width <= rectLeft)
                    break;

                int leftSide = skyLine[skylineNodeIndex].x;
                int rightSide = min(rectRight, leftSide + skyLine[skylineNodeIndex].width);
                wastedArea += (rightSide - leftSide) * (y - skyLine[skylineNodeIndex].y);
            }
            return wastedArea;
        }

        public bool RectangleFits(int skylineNodeIndex, int width, int height, ref int y, ref int wastedArea)
        {
            bool fits = RectangleFits(skylineNodeIndex, width, height, ref y);
            if (fits)
                wastedArea = ComputeWastedArea(skylineNodeIndex, width, height, y);

            return fits;
        }

        public void AddWasteMapArea(int skylineNodeIndex, int width, int height, int y)
        {
            int rectLeft = skyLine[skylineNodeIndex].x;
            int rectRight = rectLeft + width;
            for (; skylineNodeIndex < skyLine.Count && skyLine[skylineNodeIndex].x < rectRight; ++skylineNodeIndex)
            {
                if (skyLine[skylineNodeIndex].x >= rectRight || skyLine[skylineNodeIndex].x + skyLine[skylineNodeIndex].width <= rectLeft)
                    break;

                int leftSide = skyLine[skylineNodeIndex].x;
                int rightSide = min(rectRight, leftSide + skyLine[skylineNodeIndex].width);

                Module waste = new Module();
                waste.X = leftSide;
                waste.Y = skyLine[skylineNodeIndex].y;
                waste.Width = rightSide - leftSide;
                waste.Height = y - skyLine[skylineNodeIndex].y;

                wasteMap.Add(waste);
            }
        }

        public Module InsertMinWaste(int width, int height)
        {
            int bestHeight;
            int bestWastedArea;
            int bestIndex;
            Module newNode = FindPositionForNewNodeMinWaste(width, height, out bestHeight, out bestWastedArea, out bestIndex);

            if (bestIndex != -1)
            {
                AddSkylineLevel(bestIndex, ref newNode);
            }

            return newNode;
        }

        public Module FindPositionForNewNodeMinWaste(int width, int height, out int bestHeight, out int bestWastedArea, out int bestIndex)
        {
            bestHeight = int.MaxValue;
            bestWastedArea = int.MaxValue;
            bestIndex = -1;
            Module newNode = new Module();

            for (int i = 0; i < skyLine.Count; ++i)
            {
                int y = 0;
                int wastedArea = 0;
                if (RectangleFits(i, width, height, ref y, ref wastedArea))
                {
                    if (wastedArea < bestWastedArea || (wastedArea == bestWastedArea && y + height < bestHeight))
                    {
                        bestHeight = y + height;
                        bestWastedArea = wastedArea;
                        bestIndex = i;
                        newNode.X = skyLine[i].x;
                        newNode.Y = y;
                        newNode.Width = width;
                        newNode.Height = height;
                    }
                }
            }

            return newNode;
        }

        private int max(int y, int p)
        {
            if (y > p)
                return y;
            else
                return p;
        }

        private int min(int h, int j)
        {
            if (h < j)
                return h;
            else
                return j;
        }

        public int Height()
        {
            int y = 0;
            for (int i = 0; i < skyLine.Count; i++)
            {
                if (skyLine[i].y > y)
                    y = skyLine[i].y;
            }
            return y;
        }

    }
}
