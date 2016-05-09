using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PRDB_Visual_Management.ProbSQLCompiler
{
    class RelationConnector : ProbSQLContext
    {
        public SelectExpression selExpr1 { get; set; }

        public SelectExpression selExpr2 { get; set; }

        public string strValue { get; set; }

        public RelationConnector()
        {
            selExpr1 = new SelectExpression();

            selExpr2 = new SelectExpression();

            strValue = string.Empty;
        }
    }
}
