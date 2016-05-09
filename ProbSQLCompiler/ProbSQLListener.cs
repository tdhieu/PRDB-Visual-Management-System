using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace PRDB_Visual_Management.ProbSQLCompiler
{    
    public class ProbSQLListener : ProbSQLBaseListener
    {
        // ---------------------------- Vùng khai báo thuộc tính ----------------------------

        private List<ProbSQLContext> listValue;
        private List<string> listType;
        private List<string> constJoinOperators = new List<string>() { "JOIN_IN", "JOIN_IG", "JOIN_PC", "JOIN_ME" };
        private List<string> constConditionConnectors = new List<string>() { "AND", "&&", "OR", "||" };
        private List<string> constSubqueryOperators = new List<string>() { "IN_IN", "IN_IG", "IN_PC", "IN_ME", 
                                                                           "NOT_IN_IN", "NOT_IN_IG", "NOT_IN_PC", "NOT_IN_ME", 
                                                                           "!IN_IN", "!IN_IG", "!IN_PC", "!IN_ME" };
        private List<string> constNegativeOperator = new List<string>() { "NOT", "!" };
        private List<string> constExpressionConnector = new List<string>() { "CONJ_IN", "CONJ_IG", "CONJ_PC", "CONJ_ME",
	                                                                         "DISJ_IN", "DISJ_IG", "DISJ_PC", "DISJ_ME",
	                                                                         "DIFF_IN", "DIFF_IG", "DIFF_PC", "DIFF_ME" };
        private string initContext;
        public SelectStatement selectStatement { get; set; }
        public string test { get; set; }

        // ---------------------------- Vùng định nghĩa các phương thức ----------------------------


        public ProbSQLListener()
        {
            listValue = new List<ProbSQLContext>();
            listType = new List<string>();
            initContext = string.Empty;
        }

        public override void EnterInit(ProbSQLParser.InitContext context)
        {
            initContext = context.GetText();
            base.EnterInit(context);
        }

        public override void EnterSelect_statement(ProbSQLParser.Select_statementContext context)
        {
            // Tạo mới SelectStatement và lưu vào list
            SelectStatement curSelectStatement = new SelectStatement();
            curSelectStatement.context = context.GetText();
            listValue.Add(curSelectStatement);
            listType.Add("SelectStatement");

            // Tìm Subquery là cha của SelectStatement
            for (int i = listValue.Count - 1; i >= 0; i--)
                if (listType[i].Equals("Subquery"))
                    if (listValue[i].context.Contains(context.GetText()))
                    {
                        Subquery subquery = (Subquery)listValue[i];
                        subquery.selectStatement = curSelectStatement;
                        break;
                    }

            base.EnterSelect_statement(context);
        }

        public override void ExitSelect_statement(ProbSQLParser.Select_statementContext context)
        {            
            // Xóa SelectExpression đã lưu trong list
            int last = listValue.Count - 1;
            for (int i = last; i >= 0; i--)
                if (listType[i].Equals("SelectStatement"))
                    if (listValue[i].context.Equals(context.GetText()))
                    {
                        // Nếu SelectStatement có context == initContext thì lưu lại SelectStatement là kết quả phân tích cây truy vấn
                        if (context.GetText().Equals(initContext))
                        {
                            selectStatement = (SelectStatement)listValue[i];
                        }

                        listType.RemoveAt(i);
                        listValue.RemoveAt(i);
                        break;
                    }

            base.ExitSelect_statement(context);
        }

        public override void EnterRelation_connector(ProbSQLParser.Relation_connectorContext context)
        {
            // Tìm SelectStatement chứa RelationConnector
            for (int i = listValue.Count - 1; i >= 0; i--)
                if (listType[i].Equals("SelectStatement"))
                    if (listValue[i].context.Contains(context.GetText()))
                    {
                        SelectStatement selectStatement = (SelectStatement)listValue[i];
                        selectStatement.relConnectors.Add(context.GetText().ToUpper());
                        break;
                    }

            base.EnterRelation_connector(context);
        }

        public override void EnterSelect_expression(ProbSQLParser.Select_expressionContext context)
        {
            // Tạo mới SelectExpression và lưu vào list
            SelectExpression curSelectExpression = new SelectExpression();
            curSelectExpression.context = context.GetText();
            listValue.Add(curSelectExpression);
            listType.Add("SelectExpression");

            // Tìm SelectStatement chứa SelectExpression
            for (int i=listValue.Count-1; i>=0; i--)
                if (listType[i].Equals("SelectStatement"))
                    if (listValue[i].context.Contains(context.GetText()))
                    {
                        SelectStatement selectStatement = (SelectStatement)listValue[i];
                        selectStatement.selectExpr.Add(curSelectExpression);
                        break;
                    }
            base.EnterSelect_expression(context);
        }

        public override void ExitSelect_expression(ProbSQLParser.Select_expressionContext context)
        {
            // Xóa SelectExpression đã lưu trong list
            int last = listValue.Count - 1;
            for (int i = last; i >= 0; i--)
                if (listType[i].Equals("SelectExpression"))
                    if (listValue[i].context.Equals(context.GetText()))
                    {
                        listType.RemoveAt(i);
                        listValue.RemoveAt(i);
                        break;
                    }

            base.ExitSelect_expression(context);
        }

        public override void EnterAttribute_list(ProbSQLParser.Attribute_listContext context)
        {
            // Tìm SelectExpression chứa danh sách các thuộc tính
            string [] attribute = context.GetText().Split(',');
            for (int i = listValue.Count - 1; i >= 0; i--)
                if (listType[i].Equals("SelectExpression"))
                    if (listValue[i].context.Contains(context.GetText()))
                    {
                        SelectExpression selectExpr = (SelectExpression)listValue[i];

                        // Lưu các thuộc tính vào SelectExpression
                        for (int j = 0; j < attribute.Length; j++)
                        {
                            selectExpr.attributeList.Add(attribute[j]);
                        }
                        break;
                    }

            base.EnterAttribute_list(context);
        }

        public override void EnterJoin_relation_list(ProbSQLParser.Join_relation_listContext context)
        {
            // Tìm SelectExpression chứa JoinRelationList
            for (int i = listValue.Count - 1; i >= 0; i--)
                if (listType[i].Equals("SelectExpression"))
                    if (listValue[i].context.Contains(context.GetText()))
                    {
                        SelectExpression selectExpr = (SelectExpression)listValue[i];

                        // Thêm các JoinOperators và JoinedRelations vào SelectExpression
                        for (int j = 0; j < context.ChildCount; j++)
                        {
                            if (constJoinOperators.Contains(context.GetChild(j).GetText().ToUpper()))
                                selectExpr.joinOperators.Add(context.GetChild(j).GetText().ToUpper());
                            else
                                selectExpr.joinRelations.Add(context.GetChild(j).GetText());
                        }
                        break;
                    }
            base.EnterJoin_relation_list(context);
        }

        public override void EnterSelection_condition(ProbSQLParser.Selection_conditionContext context)
        {
            // Tìm SelectExpression chứa SelectionCondition
            for (int i = listValue.Count - 1; i >= 0; i--)
                if (listType[i].Equals("SelectExpression"))
                    if (listValue[i].context.Contains(context.GetText()))
                    {
                        SelectExpression selectExpr = (SelectExpression)listValue[i];

                        // Lưu các ConditionConnector vào SelectExpression
                        for (int j = 0; j < context.ChildCount; j++)
                        {
                            if (constConditionConnectors.Contains(context.GetChild(j).GetText().ToUpper()))
                                selectExpr.conditionConnectors.Add(context.GetChild(j).GetText().ToUpper());
                        }
                        break;
                    }
 	        base.EnterSelection_condition(context);
        }

        public override void EnterSelection_condition_atom(ProbSQLParser.Selection_condition_atomContext context)
        {
            // Tạo mới SelectionConditionAtom và lưu vào list
            SelectionConditionAtom curSelCondAtom = new SelectionConditionAtom();
            curSelCondAtom.context = context.GetText();
            listValue.Add(curSelCondAtom);
            listType.Add("SelectionConditionAtom");

            // Nếu toán hạng bên trái của SelectionConditionAtom là phép phủ định logic
            if (constNegativeOperator.Contains(context.GetChild(0).GetText().ToUpper()))
            {
                curSelCondAtom.isSubqueryCondition = false;
                curSelCondAtom.isPositive = false;
            }
            else // Nếu toán hạng bên trái của SelectionConditionAtom là phép toán IN hoặc NOT_IN của subquery
                if (constSubqueryOperators.Contains(context.GetChild(0).GetText().ToUpper()))
                {
                    curSelCondAtom.isSubqueryCondition = true;
                    curSelCondAtom.subQueryOperator = context.GetChild(0).GetText().ToUpper();
                    curSelCondAtom.attribute = context.attribute().GetText();
                }
                else // SelectionConditionAtom là một điều kiện truy vấn thông thường không chứa subquery
                {
                    curSelCondAtom.isSubqueryCondition = false;
                    curSelCondAtom.isPositive = true;
                }

            // Lấy khoảng xác suất
            curSelCondAtom.GetProbInterval(context.probabilistic_interval().GetText());


            // Tìm SelectExpression chứa SelectionConditionAtom
            for (int i = listValue.Count - 1; i >= 0; i--)
                if (listType[i].Equals("SelectExpression"))
                    if (listValue[i].context.Contains(context.GetText()))
                    {
                        SelectExpression selectExpr = (SelectExpression)listValue[i];
                        selectExpr.selectionConditionAtoms.Add(curSelCondAtom);
                        break;
                    }

            base.EnterSelection_condition_atom(context);
        }

        public override void ExitSelection_condition_atom(ProbSQLParser.Selection_condition_atomContext context)
        {
            // Xóa SelectionConditionAtom khỏi list
            int last = listValue.Count - 1;
            for (int i = last; i >= 0; i--)
                if (listType[i].Equals("SelectionConditionAtom"))
                    if (listValue[i].context.Equals(context.GetText()))
                    {
                        listType.RemoveAt(i);
                        listValue.RemoveAt(i);
                        break;
                    }

            base.ExitSelection_condition_atom(context);
        }

        public override void EnterExpression(ProbSQLParser.ExpressionContext context)
        {
            // Tạo mới và lưu Expression vào list
            Expression curExpression = new Expression();
            curExpression.context = context.GetText();
            listValue.Add(curExpression);
            listType.Add("Expression");

            // Lưu các ExpressionConnector vào Expression
            for (int j = 0; j < context.ChildCount; j++)
            {
                if (constExpressionConnector.Contains(context.GetChild(j).GetText().ToUpper()))
                    curExpression.expressionConnectors.Add(context.GetChild(j).GetText().ToUpper());
            }

            // Lưu Expression vào SelectionConditionAtom
            for (int i = listValue.Count - 1; i >= 0; i--)
                if (listType[i].Equals("SelectionConditionAtom"))
                    if (listValue[i].context.Contains(context.GetText()))
                    {
                        SelectionConditionAtom selectionConditionAtom = (SelectionConditionAtom)listValue[i];
                        selectionConditionAtom.expression = curExpression;
                        break;
                    }

            base.EnterExpression(context);
        }

        public override void ExitExpression(ProbSQLParser.ExpressionContext context)
        {
            // Xóa Expression khỏi list
            int last = listValue.Count - 1;
            for (int i = last; i >= 0; i--)
                if (listType[i].Equals("Expression"))
                    if (listValue[i].context.Equals(context.GetText()))
                    {
                        listType.RemoveAt(i);
                        listValue.RemoveAt(i);
                        break;
                    }

            base.ExitExpression(context);
        }

        public override void EnterExpression_atom(ProbSQLParser.Expression_atomContext context)
        {
            // Tạo mới và lưu ExpressionAtom vào list
            ExpressionAtom curExprAtom = new ExpressionAtom();
            curExprAtom.context = context.GetText();
            listValue.Add(curExprAtom);
            listType.Add("ExpressionAtom");

            // Lưu các toán hạng và toán tử của ExpressionAtom
            curExprAtom.compareOperator = context.GetChild(1).GetText().ToUpper();
            curExprAtom.operand1 = context.GetChild(0).GetText();
            curExprAtom.operand2 = context.GetChild(2).GetText();

            // Tìm Expression chứa ExpressionAtom
            for (int i = listValue.Count - 1; i >= 0; i--)
                if (listType[i].Equals("Expression"))
                    if (listValue[i].context.Contains(context.GetText()))
                    {
                        Expression expression = (Expression)listValue[i];
                        expression.expressionAtoms.Add(curExprAtom);
                        break;
                    }

            base.EnterExpression_atom(context);
        }

        public override void ExitExpression_atom(ProbSQLParser.Expression_atomContext context)
        {
            // Xóa ExpressionAtom khỏi list
            int last = listValue.Count - 1;
            for (int i = last; i >= 0; i--)
                if (listType[i].Equals("ExpressionAtom"))
                    if (listValue[i].context.Equals(context.GetText()))
                    {
                        listType.RemoveAt(i);
                        listValue.RemoveAt(i);
                        break;
                    }

            base.ExitExpression_atom(context);
        }

        public override void EnterSubquery(ProbSQLParser.SubqueryContext context)
        {
            // Tạo mới và lưu Subquery vào list
            Subquery subquery = new Subquery();
            subquery.context = context.GetText();
            listValue.Add(subquery);
            listType.Add("Subquery");

            // Tìm SelectionConditionAtom chứa Subquery
            for (int i = listValue.Count - 1; i >= 0; i--)
                if (listType[i].Equals("SelectionConditionAtom"))
                    if (listValue[i].context.Contains(context.GetText()))
                    {
                        SelectionConditionAtom selectionConditionAtom = (SelectionConditionAtom)listValue[i];
                        selectionConditionAtom.subQuery = subquery;
                        break;
                    }

                base.EnterSubquery(context);
        }

        public override void ExitSubquery(ProbSQLParser.SubqueryContext context)
        {
            // Xóa Subquery khỏi list
            int last = listValue.Count - 1;
            for (int i = last; i >= 0; i--)
                if (listType[i].Equals("Subquery"))
                    if (listValue[i].context.Equals(context.GetText()))
                    {
                        listType.RemoveAt(i);
                        listValue.RemoveAt(i);
                        break;
                    }
            base.ExitSubquery(context);
        }
    }
}
