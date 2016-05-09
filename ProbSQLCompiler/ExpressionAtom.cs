using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PRDB_Visual_Management.ProbSQLCompiler
{
    public class ExpressionAtom : ProbSQLContext
    {
        public string operand1 { get; set; }
        public string operand2 { get; set; }
        public string compareOperator { get; set; }

        public ExpressionAtom()
        {
            operand1 = string.Empty;
            operand2 = string.Empty;
            compareOperator = string.Empty;
        }
    }
}
