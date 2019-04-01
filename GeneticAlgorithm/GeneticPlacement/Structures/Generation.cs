using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeneticPlacement
{
    public class Generation:ICloneable
    {
        public List<Individual> People;

        #region ICloneable Members

        public object Clone()
        {
            Generation newGeneration = new Generation();
            newGeneration.People = this.People.Select(item => (Individual)item.Clone()).ToList();
            return newGeneration;
        }
        #endregion
    }
}
