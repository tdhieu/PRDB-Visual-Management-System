using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PRDB_Visual_Management.BusinessLogicLayer
{
    public class ProbInterval
    {
        // ---------------------------- Vùng khai báo các thuộc tính ----------------------------

        public double minprob { get; set; }

        public double maxprob { get; set; }

        public ProbInterval()
        {
            minprob = maxprob = 0;
        }

        public bool Inside(ProbInterval probInterval)
        {
            return (this.minprob >= probInterval.minprob && this.maxprob <= probInterval.maxprob);
        }

        public bool Inside(double tmpMinprob, double tmpMaxprob)
        {
            return (this.minprob >= tmpMinprob && this.maxprob <= tmpMaxprob);
        }

    }
}
