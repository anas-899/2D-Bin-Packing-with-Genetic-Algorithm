using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GeneticPlacement
{
   public class Module:ICloneable
    {
        public string Name;
        public int X;
        public int Y;
        public int Width;
        public int Height;
        public Brush brush_color;

        #region ICloneable Members

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
