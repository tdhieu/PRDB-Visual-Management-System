using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace PRDB_Visual_Management.ProbSQLCompiler
{
    public class SelectStatement : ProbSQLContext
    {
        public List<SelectExpression> selectExpr { get; set; }
        public List<string> relConnectors { get; set; }

        public SelectStatement()
        {
            selectExpr = new List<SelectExpression>();
            relConnectors = new List<string>();
        }
    }
}
