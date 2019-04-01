using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeneticPlacement
{
   public class Individual:ICloneable
    {
        public List<Module> chromosome;
        public double fitness;
        public double cost;


        #region ICloneable Members

        public object Clone()
        {
            //Individual newindividual = (Individual)this.MemberwiseClone();
            Individual newindividual = new Individual();
            newindividual.chromosome = this.chromosome.Select(item => (Module)item.Clone()).ToList();
            newindividual.fitness = this.fitness;
            return newindividual;
        }

        #endregion
    }
}
