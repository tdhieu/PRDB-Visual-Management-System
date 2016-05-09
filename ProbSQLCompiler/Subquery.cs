using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PRDB_Visual_Management.ProbSQLCompiler
{
    public class Subquery : ProbSQLContext
    {
        public SelectStatement selectStatement { get; set; }

        public Subquery()
        {
            selectStatement = new SelectStatement();
        }
    }
}
