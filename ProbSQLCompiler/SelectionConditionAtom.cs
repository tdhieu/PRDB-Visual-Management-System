using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PRDB_Visual_Management.ProbSQLCompiler
{
    public class SelectionConditionAtom : ProbSQLContext
    {
        public bool isPositive { get; set; }
        public Expression expression { get; set; }

        public bool isSubqueryCondition { get; set; }
        public string subQueryOperator { get; set; }
        public string attribute { get; set; }
        public Subquery subQuery { get; set; }

        public double minProb { get; set; }
        public double maxProb { get; set; }

        public SelectionConditionAtom()
        {
            isPositive = true;
            isSubqueryCondition = false;
        }

        public void GetProbInterval(string probInterval)
        {
            string tmp = probInterval;

            // Loại bỏ left bracket, right bracket và comma
            tmp = tmp.Replace("[", "");
            tmp = tmp.Replace("]", "");
            string[] value = tmp.Split(',');

            // value[0] --> minProb, value[1] --> maxProb
            this.minProb = Convert.ToDouble(value[0]);
            this.maxProb = Convert.ToDouble(value[1]);
        }
    }
}
