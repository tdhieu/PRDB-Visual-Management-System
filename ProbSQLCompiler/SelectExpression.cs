using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PRDB_Visual_Management.ProbSQLCompiler
{
    public class SelectExpression : ProbSQLContext
    {
        public List<string> attributeList { get; set; }
        public List<string> joinOperators { get; set; }
        public List<string> joinRelations { get; set; }
        public List<string> conditionConnectors { get; set; }
        public List<SelectionConditionAtom> selectionConditionAtoms { get; set; }        

        public SelectExpression()
        {
            attributeList = new List<string>();
            joinOperators = new List<string>();
            joinRelations = new List<string>();
            conditionConnectors = new List<string>();
            selectionConditionAtoms = new List<SelectionConditionAtom>();
        }

        public bool ContainSelectCondition()
            // Kiểm tra xem query có chứa điều kiện hay không
        {
            return (this.selectionConditionAtoms.Count > 0);
        }
    }
}
