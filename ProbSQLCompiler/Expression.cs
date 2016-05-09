using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PRDB_Visual_Management.ProbSQLCompiler
{
    public class Expression : ProbSQLContext
    {
        public List<string> expressionConnectors { get; set; }
        public List<ExpressionAtom> expressionAtoms { get; set; }

        public Expression()
        {
            expressionConnectors = new List<string>();
            expressionAtoms = new List<ExpressionAtom>();            
        }
    }
}
