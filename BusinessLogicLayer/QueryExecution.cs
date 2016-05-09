using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using PRDB_Visual_Management.ProbSQLCompiler;
using System.Windows.Forms;

namespace PRDB_Visual_Management.BusinessLogicLayer
{
    public static class QueryExecution
    {
        private static ProbDatabase probDB;
        private static List<string> cnstCompareValueOperators = new List<string>() { "=", "==", "<>", "!=", ">", ">=", "<", "<=" };
        private static List<string> cnstCompareAttributeOperators = new List<string>() { "EQUAL_IN", "EQUAL_IG", "EQUAL_PC", "EQUAL_ME",
                                                                                         "UNEQUAL_IN", "UNEQUAL_IG", "UNEQUAL_PC", "UNEQUAL_ME",
                                                                                         "LTH_IN", "LTH_IG", "LTH_PC", "LTH_ME",
                	                                                                     "LET_IN", "LET_IG", "LET_PC", "LET_ME",
	                                                                                     "GTH_IN", "GTH_IG", "GTH_PC", "GTH_ME",
	                                                                                     "GET_IN", "GET_IG", "GET_PC", "GET_ME" };
        public static string errMessage { get; set; }
        public static bool successful { get; set; }
        public static ProbRelation satisfiedRelation { get; set; }

        public static string StandardizeQuery(string strQuery)
            // Chuẩn hóa các phép toán trong truy vấn để phân tích bằng trình biên dịch
        {
            string queryString = strQuery;
            queryString = queryString.Replace("==⊗ig", "EQUAL_IG");
            queryString = queryString.Replace("==⊗in", "EQUAL_IN");
            queryString = queryString.Replace("==⊗me", "EQUAL_ME");
            queryString = queryString.Replace("==⊗pc", "EQUAL_PC");

            queryString = queryString.Replace("!=⊗ig", "!EQUAL_IG");
            queryString = queryString.Replace("!=⊗in", "!EQUAL_IN");
            queryString = queryString.Replace("!=⊗me", "!EQUAL_ME");
            queryString = queryString.Replace("!=⊗pc", "!EQUAL_PC");

            queryString = queryString.Replace("<⊗ig", "LTH_IG");
            queryString = queryString.Replace("<⊗in", "LTH_IN");
            queryString = queryString.Replace("<⊗me", "LTH_ME");
            queryString = queryString.Replace("<⊗pc", "LTH_PC");

            queryString = queryString.Replace("≤⊗ig", "LET_IG");
            queryString = queryString.Replace("≤⊗in", "LET_IN");
            queryString = queryString.Replace("≤⊗me", "LET_ME");
            queryString = queryString.Replace("≤⊗pc", "LET_PC");

            queryString = queryString.Replace(">⊗ig", "GTH_IG");
            queryString = queryString.Replace(">⊗in", "GTH_IN");
            queryString = queryString.Replace(">⊗me", "GTH_ME");
            queryString = queryString.Replace(">⊗pc", "GTH_PC");

            queryString = queryString.Replace("≥⊗ig", "GET_IG");
            queryString = queryString.Replace("≥⊗in", "GET_IN");
            queryString = queryString.Replace("≥⊗me", "GET_ME");
            queryString = queryString.Replace("≥⊗pc", "GET_PC");

            queryString = queryString.Replace("⨝⊗ig", "JOIN_IG");
            queryString = queryString.Replace("⨝⊗in", "JOIN_IN");
            queryString = queryString.Replace("⨝⊗me", "JOIN_ME");
            queryString = queryString.Replace("⨝⊗pc", "JOIN_PC");

            queryString = queryString.Replace("⋃⊕ig", "UNION_IG");
            queryString = queryString.Replace("⋃⊕in", "UNION_IN");
            queryString = queryString.Replace("⋃⊕me", "UNION_ME");
            queryString = queryString.Replace("⋃⊕pc", "UNION_PC");

            queryString = queryString.Replace("⋂⊗ig", "INTERSECT_IG");
            queryString = queryString.Replace("⋂⊗in", "INTERSECT_IN");
            queryString = queryString.Replace("⋂⊗me", "INTERSECT_ME");
            queryString = queryString.Replace("⋂⊗pc", "INTERSECT_PC");

            queryString = queryString.Replace("-⊖ig", "MINUS_IG");
            queryString = queryString.Replace("-⊖in", "MINUS_IN");
            queryString = queryString.Replace("-⊖me", "MINUS_ME");
            queryString = queryString.Replace("-⊖pc", "MINUS_PC");

            queryString = queryString.Replace("¬IN⊗ig", "!IN_IG");
            queryString = queryString.Replace("¬IN⊗in", "!IN_IN");
            queryString = queryString.Replace("¬IN⊗me", "!IN_ME");
            queryString = queryString.Replace("¬IN⊗pc", "!IN_PC");

            queryString = queryString.Replace("IN⊗ig", "IN_IG");
            queryString = queryString.Replace("IN⊗in", "IN_IN");
            queryString = queryString.Replace("IN⊗me", "IN_ME");
            queryString = queryString.Replace("IN⊗pc", "IN_PC");

            queryString = queryString.Replace("⊗ig", "CONJ_IG");
            queryString = queryString.Replace("⊗in", "CONJ_IN");
            queryString = queryString.Replace("⊗me", "CONJ_ME");
            queryString = queryString.Replace("⊗pc", "CONJ_PC");

            queryString = queryString.Replace("⊕ig", "DISJ_IG");
            queryString = queryString.Replace("⊕in", "DISJ_IN");
            queryString = queryString.Replace("⊕me", "DISJ_ME");
            queryString = queryString.Replace("⊕pc", "DISJ_PC");

            queryString = queryString.Replace("⊖ig", "DIFF_IG");
            queryString = queryString.Replace("⊖in", "DIFF_IN");
            queryString = queryString.Replace("⊖me", "DIFF_ME");
            queryString = queryString.Replace("⊖pc", "DIFF_PC");

            return queryString;
        }

        private static SelectStatement ParseQueryTree(string queryString)
            // Phân tích cây truy vấn ProbSQL
        {
            try
            {
                AntlrInputStream input = new AntlrInputStream(queryString);
                ProbSQLLexer lexer = new ProbSQLLexer(input);
                CommonTokenStream token = new CommonTokenStream(lexer);
                ProbSQLParser parser = new ProbSQLParser(token);
                IParseTree tree = parser.init();
                ParseTreeWalker walker = new ParseTreeWalker();
                ProbSQLListener listener = new ProbSQLListener();
                walker.Walk(listener, tree);
                return listener.selectStatement;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        private static ProbRelation ProcessSelectExpression(SelectExpression selectExpr)
            // Tìm quan hệ được sinh ra từ SelectExpression của cây truy vấn ProbSQL
        {
            try
            {
                ProbRelation relationResult, tmpRelation;

                // Thực hiện phép kết các quan hệ
                if (selectExpr.joinOperators.Count > 0)
                {
                    int i = 0;
                    ProbRelation probRelation1, probRelation2;
                    probRelation1 = GetRelationByName(selectExpr.joinRelations[i++]);
                    if (probRelation1 == null)
                        throw new Exception("Relation " + selectExpr.joinRelations[i - 1] + " is not found in the current Database");

                    foreach (string joinOperator in selectExpr.joinOperators)
                    {
                        probRelation2 = GetRelationByName(selectExpr.joinRelations[i++]);
                        if (probRelation2 == null)
                            throw new Exception("Relation " + selectExpr.joinRelations[i - 1] + " is not found in the current Database");

                        probRelation1 = PRDBAlgebra.Combine(probRelation1, probRelation2, joinOperator);
                        if (probRelation1 == null)
                            throw new Exception("Invalid " + joinOperator + " between relation \"" + selectExpr.joinRelations[i - 2] + " and relation " + selectExpr.joinRelations[i - 1] + "\"");
                    }
                    relationResult = probRelation1;
                }
                else
                {
                    relationResult = GetRelationByName(selectExpr.joinRelations[0]);
                    if (relationResult == null)
                        throw new Exception("There is no relation " + selectExpr.joinRelations[0] + " in the current Database!");
                }

                // Thực hiện phép chọn trên các bộ của quan hệ
                if (selectExpr.ContainSelectCondition())
                {
                    tmpRelation = new ProbRelation();
                    tmpRelation.GetTuplesOf(relationResult);
                    relationResult.tuples.Clear();

                    foreach (ProbTuple tuple in tmpRelation.tuples)
                    {
                        int i = 0;
                        bool result1, result2;
                        result1 = SatisfyProbabilisticInterval(tuple, selectExpr.selectionConditionAtoms[i++]);
                        foreach (string conditionConnector in selectExpr.conditionConnectors)
                        {
                            result2 = SatisfyProbabilisticInterval(tuple, selectExpr.selectionConditionAtoms[i++]);
                            result1 = SatisfyLogicalCondition(result1, result2, conditionConnector);
                        }

                        if (result1) relationResult.tuples.Add(tuple);
                    }
                }

                // Thực hiện phép chiếu trên quan hệ kết quả
                List<ProbAttribute> selectAttributes = new List<ProbAttribute>();

                // Lấy danh sách các thuộc tính được chọn đưa vào selectAttributes
                foreach(string attribute in selectExpr.attributeList)
                {
                    // Lấy tất cả các thuộc tính của quan hệ kết quả
                    if (attribute.Equals("*")) selectAttributes = relationResult.scheme.attributes;
                    else if (attribute.Contains("."))
                    // Trường hợp thuộc tính được chọn có dạng RelationName.AttributeName hoặc RelationName.* 
                    {
                        int dotPos = attribute.IndexOf(".");
                        string relationName = attribute.Substring(0, dotPos);
                        string attributeName = attribute.Substring(dotPos + 1);

                        ProbRelation referencedRelation = probDB.GetRelation(relationName);
                        if (referencedRelation == null) throw new Exception("Invalid relation name in "
                        + attribute + " of Select Expression \"" + selectExpr.context + "\"");

                        if (attributeName.Equals("*")) selectAttributes = referencedRelation.scheme.attributes;
                        else
                        {
                            ProbAttribute attr = referencedRelation.scheme.GetAttribute(attributeName);
                            selectAttributes.Add(attr);
                        }
                    }
                    else
                    // Trường hợp chỉ có tên thuộc tính
                    {
                        ProbAttribute attr = relationResult.scheme.GetAttribute(attribute);
                        selectAttributes.Add(attr);
                    }
                }

                tmpRelation = new ProbRelation();
                tmpRelation.GetAttributesOf(relationResult);

                foreach (ProbAttribute attr in tmpRelation.scheme.attributes)
                    if (attr.NotInList(selectAttributes))
                        relationResult.Remove(attr);

                return relationResult;
            }
            catch (Exception Ex)
            {
                errMessage = Ex.Message;
                return null;
            }
        }

        private static ProbRelation ProcessSelectStatement(SelectStatement selectStmnt)
            // Tìm quan hệ được sinh ra từ SelectStatement của cây truy vấn ProbSQL
        {
            try
            {
                ProbRelation probRelation1, probRelation2;
                if (selectStmnt.relConnectors.Count > 0)
                {
                    int i = 0;
                    probRelation1 = ProcessSelectExpression(selectStmnt.selectExpr[i++]);
                    if (probRelation1 == null) throw new Exception("Invalid Relation in context \"" + selectStmnt.selectExpr[i - 1].context + "\"");

                    foreach (string relConnector in selectStmnt.relConnectors)
                    {
                        probRelation2 = ProcessSelectExpression(selectStmnt.selectExpr[i++]);
                        if (probRelation2 == null) throw new Exception("Invalid Relation in context \"" + selectStmnt.selectExpr[i - 1].context + "\"");

                        probRelation1 = PRDBAlgebra.Combine(probRelation1, probRelation2, relConnector);
                        if (probRelation1 == null) throw new Exception("Invalid " + relConnector + " operation between Select Expression \"" 
                                                                       + selectStmnt.selectExpr[i - 2].context + "\"  and Select Expression \"" 
                                                                       + selectStmnt.selectExpr[i - 1].context + "\"");
                    }
                    return probRelation1;
                }
                else return ProcessSelectExpression(selectStmnt.selectExpr[0]);
            }
            catch (Exception Ex)
            {
                errMessage = Ex.Message;
                return null;
            }
        }

        private static ProbRelation GetRelationByName(string relationName)
            // Tìm quan hệ trong CSDL dựa theo tên
        {
            try
            {
                ProbRelation relationResult = new ProbRelation();
                foreach (ProbRelation probRelation in probDB.relations)
                    if (probRelation.relationname.Equals(relationName))
                    {
                        relationResult.AssignValue(probRelation);
                        return relationResult;
                    }
                return null;
            }
            catch (Exception Ex)
            {
            }
            return null;
        }

        public static ProbAttribute GetRelatedAttribute(string attrName, ProbTuple tuple)
            // Tìm thuộc tính tương ứng trên bộ
        {
            try
            {
                if (attrName.Contains("."))
                // Trường hợp thuộc tính trong biểu thức chọn subquery có dạng RelationName.AttributeName
                {
                    int pos = attrName.IndexOf(".");

                    // Get RelationName
                    string relationName = attrName.Substring(0, pos);
                    ProbRelation relatedRelation = probDB.GetRelation(relationName);

                    string attributeName = attrName.Substring(pos + 1);

                    foreach (ProbAttribute relatedAttribute in tuple.triples.Keys)
                        // Tìm thuộc tính trên bộ có cùng tên với thuộc tính trong biểu thức subquery
                        if (relatedAttribute.attributeName.Equals(attributeName))
                            // Kiểm tra xem thuộc tính trên có tồn tại trong quan hệ được chỉ định trong câu truy vấn 
                            // (Áp dụng cho trường hợp thuộc tính có dạng RelationName.AttributeName
                            if (relatedRelation.scheme.attributes.Contains(relatedAttribute))
                                return relatedAttribute;
                }
                else
                {
                    // Tìm thuộc tính trên bộ có cùng tên với thuộc tính trong biểu thức subquery
                    foreach (ProbAttribute relatedAttribute in tuple.triples.Keys)
                        if (relatedAttribute.attributeName.Equals(attrName))
                            return relatedAttribute;
                }
                return null;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        private static void AddListToList(List<ProbAttribute> l1, List<ProbAttribute> l2)
            // Gán một List<> cho một List<>
        {
            try
            {
                if (l1 == null || l2 == null) throw new Exception("List is NULL!");
                foreach (ProbAttribute item in l1) l2.Add(item);
            }
            catch (Exception Ex)
            {
            }
        }

        private static bool SatisfyProbabilisticInterval(ProbTuple tuple, SelectionConditionAtom selCndAtom)
            // Kiểm tra xem một bộ của quan hệ có thỏa mãn điều kiện chọn nguyên tố của truy vấn 
        {
            try
            {
                // Trường hợp SelectionConditionAtom không phải là Subquery
                if (!selCndAtom.isSubqueryCondition)
                {
                    Expression expr = selCndAtom.expression;
                    ProbInterval probInterval1, probInterval2, probResult;
                    int i = 0;
                    probInterval1 = CalculateProbInterval(tuple, expr.expressionAtoms[i++]);
                    foreach (string exprConnector in expr.expressionConnectors)
                    {
                        probInterval2 = CalculateProbInterval(tuple, expr.expressionAtoms[i++]);
                        probInterval1 = CombineProbabilisticInterval(probInterval1, probInterval2, exprConnector);
                    }

                    probResult = probInterval1;

                    // Kiểm tra xem khoảng xác suất tìm được có nằm trong khoảng xác suất điều kiện
                    if (probResult.Inside(selCndAtom.minProb, selCndAtom.maxProb))
                    {
                        return true;
                    }
                    else return false;
                }
                // Trường hợp điều kiện chọn nguyên tố là Subquery
                else
                {
                    Subquery subquery = selCndAtom.subQuery;

                    ProbAttribute subqueryAttribute = GetRelatedAttribute(selCndAtom.attribute, tuple);
                    if (subqueryAttribute == null) throw new Exception();

                    ProbRelation subqueryRelation = ProcessSelectStatement(subquery.selectStatement);
                    if (subqueryRelation == null) throw new Exception();

                    // Trường hợp subqueryRelation ko chứa subqueryAttribute ==> điều kiện truy vấn sai
                    if (!subqueryRelation.scheme.ContainAttribute(subqueryAttribute)) throw new Exception();

                    string subqueryOperator = selCndAtom.subQueryOperator;

                    string dataType = subqueryAttribute.type.typeName;

                    // Giá trị của thuộc tính subqueryAttribute tương ứng trên tuple đang xét
                    ProbTriple subqueryTriple = tuple.triples[subqueryAttribute];

                    // Giá trị của thuộc tính subqueryAttribute tương ứng trên quan hệ subqueryRelation
                    ProbTriple tmpTriple;

                    // Khoảng xác suất để (subqueryTriple == tmpTriple)
                    ProbInterval probInterval;

                    // So sánh subqueryTriple với tmpTriple trên từng bộ của quan hệ subqueryRelation
                    foreach (ProbTuple tmpTuple in subqueryRelation.tuples)
                    {
                        tmpTriple = tmpTuple.GetTriple(subqueryAttribute);
                        switch (subqueryOperator)
                        {
                            case "IN_IG":
                                probInterval = CompareProbTripleAndProbTriple(subqueryTriple, tmpTriple, "EQUAL_IG", dataType);
                                // Nếu tồn tại ProbInterval(subqueryTriple EQUAL_IG tmpTriple) nằm trong khoảng xác suất điều kiện ==> true
                                if (probInterval.Inside(selCndAtom.minProb, selCndAtom.maxProb)) return true;
                                break;

                            case "IN_IN":
                                probInterval = CompareProbTripleAndProbTriple(subqueryTriple, tmpTriple, "EQUAL_IN", dataType);
                                // Nếu tồn tại ProbInterval(subqueryTriple EQUAL_IN tmpTriple) nằm trong khoảng xác suất điều kiện ==> true
                                if (probInterval.Inside(selCndAtom.minProb, selCndAtom.maxProb)) return true;
                                break;

                            case "IN_ME":
                                probInterval = CompareProbTripleAndProbTriple(subqueryTriple, tmpTriple, "EQUAL_ME", dataType);
                                // Nếu tồn tại ProbInterval(subqueryTriple EQUAL_ME tmpTriple) nằm trong khoảng xác suất điều kiện ==> true
                                if (probInterval.Inside(selCndAtom.minProb, selCndAtom.maxProb)) return true;
                                break;

                            case "IN_PC":
                                probInterval = CompareProbTripleAndProbTriple(subqueryTriple, tmpTriple, "EQUAL_PC", dataType);
                                // Nếu tồn tại ProbInterval(subqueryTriple EQUAL_PC tmpTriple) nằm trong khoảng xác suất điều kiện ==> true
                                if (probInterval.Inside(selCndAtom.minProb, selCndAtom.maxProb)) return true;
                                break;

                            case "!IN_IG":
                                probInterval = CompareProbTripleAndProbTriple(subqueryTriple, tmpTriple, "UNEQUAL_IG", dataType);
                                // Nếu tồn tại ProbInterval(subqueryTriple UNEQUAL_IG tmpTriple) không nằm trong khoảng xác suất điều kiện ==> false
                                if (!probInterval.Inside(selCndAtom.minProb, selCndAtom.maxProb)) return false;
                                break;

                            case "!IN_IN":
                                probInterval = CompareProbTripleAndProbTriple(subqueryTriple, tmpTriple, "UNEQUAL_IN", dataType);
                                // Nếu tồn tại ProbInterval(subqueryTriple UNEQUAL_IN tmpTriple) không nằm trong khoảng xác suất điều kiện ==> false
                                if (!probInterval.Inside(selCndAtom.minProb, selCndAtom.maxProb)) return false;
                                break;

                            case "!IN_ME":
                                probInterval = CompareProbTripleAndProbTriple(subqueryTriple, tmpTriple, "UNEQUAL_ME", dataType);
                                // Nếu tồn tại ProbInterval(subqueryTriple UNEQUAL_ME tmpTriple) không nằm trong khoảng xác suất điều kiện ==> false
                                if (!probInterval.Inside(selCndAtom.minProb, selCndAtom.maxProb)) return false;
                                break;

                            case "!IN_PC":
                                probInterval = CompareProbTripleAndProbTriple(subqueryTriple, tmpTriple, "UNEQUAL_PC", dataType);
                                // Nếu tồn tại ProbInterval(subqueryTriple UNEQUAL_PC tmpTriple) không nằm trong khoảng xác suất điều kiện ==> false
                                if (!probInterval.Inside(selCndAtom.minProb, selCndAtom.maxProb)) return false;
                                break;

                            default: throw new Exception();
                        }
                    }

                    if (subqueryOperator.Contains("!IN"))
                        // SelectionCondition có dạng NOTIN(attribute, subquery)[minprob, maxprob]
                    {
                        // Mọi ProbInterval(tuple.subqueryAttribute != subqueryRelation.subqueryAttribute) đều nằm trong khoảng xác suất điều kiện ==> true
                        return true;
                    }
                    else
                    // SelectionCondition có dạng IN(attribute, subquery)[minprob, maxprob]
                    {
                        // Không tồn tại ProbInterval(tuple.subqueryAttribute == subqueryRelation.subqueryAttribute) nằm trong khoảng xác suất điều kiện ==> false
                        return false;
                    }
                }
            }
            catch (Exception Ex)
            {
                return false;
            }
        }

        private static bool SatisfyLogicalCondition(bool r1, bool r2, string conditionConnector)
            // Kiểm tra kết quả logic của 2 điều kiện chọn
        {
            try
            {
                switch (conditionConnector)
                {
                    case "&&" :    
                    case "AND": return (r1 && r2);
                    case "||":
                    case "OR": return (r1 || r2);
                    default: return false;
                }
            }
            catch (Exception Ex)
            {
                return false;
            }
        }

        private static ProbInterval CalculateProbInterval(ProbTuple tuple, ExpressionAtom exprAtom)
            // Tính khoảng xác suất sinh ra khi áp dụng một bộ trên một biểu thức chọn cơ sở
        {
            try
            {
                string compareOperator = exprAtom.compareOperator;
                if (cnstCompareAttributeOperators.Contains(compareOperator))
                {
                    ProbAttribute attr1 = GetAttributeByName(tuple, exprAtom.operand1);
                    if (attr1 == null) throw new Exception("Invalid attribute name in: " + exprAtom.context);

                    ProbAttribute attr2 = GetAttributeByName(tuple, exprAtom.operand2);
                    if (attr2 == null) throw new Exception("Invalid attribute name in: " + exprAtom.context);

                    string typeName1 = attr1.type.typeName;
                    string typeName2 = attr2.type.typeName;

                    if (!typeName1.Equals(typeName2)) throw new Exception("Invalid attribute type in: " + exprAtom.context);

                    ProbTriple probTriple1 = tuple.triples[attr1];
                    ProbTriple probTriple2 = tuple.triples[attr2];

                    return CompareProbTripleAndProbTriple(probTriple1, probTriple2, compareOperator, typeName1);
                }

                if (cnstCompareValueOperators.Contains(compareOperator))
                {
                    ProbAttribute attr;
                    ProbDataType dataType;
                    string value;

                    attr = GetAttributeByName(tuple, exprAtom.operand1.Trim());
                    // Trường hợp ExpressionAtom có dạng value 𝚹 Attribute
                    if (attr == null)
                    {
                        value = exprAtom.operand1.Trim();
                        attr = GetAttributeByName(tuple, exprAtom.operand2.Trim());
                        if (attr == null) throw new Exception("Invalid attribute name in: " + exprAtom.context);
                        dataType = attr.type;
                    }
                    // Trường hợp ExpressionAtom có dạng Attribute 𝚹 value
                    else
                    {
                        value = exprAtom.operand2.Trim();
                        dataType = attr.type;
                    }

                    if (value.Contains("\"")) value = value.Replace("\"", "");

                    if (!dataType.EquivalentDataType(value)) throw new Exception("Invalid value type in: " + exprAtom.context);

                    ProbTriple probTriple = tuple.triples[attr];

                    return CompareProbTripleAndValue(probTriple, value, compareOperator, dataType.typeName);
                }
            }
            catch (Exception Ex)
            {
                errMessage = Ex.Message;
                return null;
            }           
            
            return null;
        }       

        private static ProbAttribute GetAttributeByName(ProbTuple tuple, string attrName)
            // Tìm thuộc tính tương ứng dựa theo tên
        {
            try
            {
                // Trường hợp AttributeName có dạng SchemeName.AttributeName thì ta loại bỏ "SchemeName."
                if (attrName.Contains("."))
                {
                    int indexDOT = attrName.IndexOf(".");
                    attrName = attrName.Substring(0, indexDOT + 1);
                }

                foreach (ProbAttribute attribute in tuple.triples.Keys)
                    if (attribute.attributeName.Equals(attrName))
                        return attribute;
            }
            catch (Exception Ex)
            {
                return null;
            }
            return null;
        }

        private static ProbInterval CompareProbTripleAndProbTriple(ProbTriple triple1, ProbTriple triple2, string compareOperator, string dataType)
            // Tính xác suất đạt được khi so sánh hai bộ ba xác suất
        {
            string value1, value2;
            ProbInterval result = new ProbInterval();
            try
            {
                result.minprob = result.maxprob = 0;
                for (int i = 0; i < triple1.values.Count; i++)
                    for (int j = 0; j < triple2.values.Count; j++)
                    {
                        value1 = triple1.values[i].ToString().Trim();
                        value2 = triple2.values[i].ToString().Trim();

                        switch (compareOperator)
                        {
                            case "EQUAL_IN":
                                if (EQUAL(value1, value2, dataType))
                                {
                                    // ([L1, U1] ⊗in [L2, U2]) = [L1 . L2, U1 . U2]
                                    result.minprob += triple1.minprob[i] * triple2.minprob[j];
                                    result.maxprob += triple1.maxprob[i] * triple2.maxprob[j];
                                }
                                break;
                            case "EQUAL_IG":
                                if (EQUAL(value1, value2, dataType))
                                {
                                    // ([L1, U1] ⊗ig [L2, U2]) = [max(0, L1 + L2 – 1), min(U1, U2)]
                                    result.minprob += Math.Max(0, triple1.minprob[i] + triple2.minprob[j] - 1);
                                    result.maxprob += Math.Min(triple1.maxprob[i], triple2.maxprob[j]);
                                }
                                break;
                            case "EQUAL_ME":
                                if (EQUAL(value1, value2, dataType))
                                {
                                    // ([L1, U1] ⊗me [L2, U2]) = [0, 0]
                                    result.minprob += 0;
                                    result.maxprob += 0;
                                }
                                break;
                            case "EQUAL_PC":
                                if (EQUAL(value1, value2, dataType))
                                {
                                    // ([L1, U1] ⊗pc[L2, U2]) = [min(L1, L2), min(U1, U2)]
                                    result.minprob += Math.Min(triple1.minprob[i], triple2.minprob[j]);
                                    result.maxprob += Math.Min(triple1.maxprob[i], triple2.maxprob[j]);
                                }
                                break;
                            case "UNEQUAL_IN":
                                if (!EQUAL(value1, value2, dataType))
                                {
                                    // ([L1, U1] ⊗in [L2, U2]) = [L1 . L2, U1 . U2]
                                    result.minprob += triple1.minprob[i] * triple2.minprob[j];
                                    result.maxprob += triple1.maxprob[i] * triple2.maxprob[j];
                                }
                                break;
                            case "UNEQUAL_IG":
                                if (!EQUAL(value1, value2, dataType))
                                {
                                    // ([L1, U1] ⊗ig [L2, U2]) = [max(0, L1 + L2 – 1), min(U1, U2)]
                                    result.minprob += Math.Max(0, triple1.minprob[i] + triple2.minprob[j] - 1);
                                    result.maxprob += Math.Min(triple1.maxprob[i], triple2.maxprob[j]);
                                }
                                break;
                            case "UNEQUAL_ME":
                                if (!EQUAL(value1, value2, dataType))
                                {
                                    // ([L1, U1] ⊗me [L2, U2]) = [0, 0]
                                    result.minprob += 0;
                                    result.maxprob += 0;
                                }
                                break;
                            case "UNEQUAL_PC":
                                if (!EQUAL(value1, value2, dataType))
                                {
                                    // ([L1, U1] ⊗pc[L2, U2]) = [min(L1, L2), min(U1, U2)]
                                    result.minprob += Math.Min(triple1.minprob[i], triple2.minprob[j]);
                                    result.maxprob += Math.Min(triple1.maxprob[i], triple2.maxprob[j]);
                                }
                                break;
                            case "LTH_IN":
                                if (LESSTHAN(value1, value2, dataType))
                                {
                                    // ([L1, U1] ⊗in [L2, U2]) = [L1 . L2, U1 . U2]
                                    result.minprob += triple1.minprob[i] * triple2.minprob[j];
                                    result.maxprob += triple1.maxprob[i] * triple2.maxprob[j];
                                }
                                break;
                            case "LTH_IG":
                                if (LESSTHAN(value1, value2, dataType))
                                {
                                    // ([L1, U1] ⊗ig [L2, U2]) = [max(0, L1 + L2 – 1), min(U1, U2)]
                                    result.minprob += Math.Max(0, triple1.minprob[i] + triple2.minprob[j] - 1);
                                    result.maxprob += Math.Min(triple1.maxprob[i], triple2.maxprob[j]);
                                }
                                break;
                            case "LTH_ME":
                                if (LESSTHAN(value1, value2, dataType))
                                {
                                    // ([L1, U1] ⊗me [L2, U2]) = [0, 0]
                                    result.minprob += 0;
                                    result.maxprob += 0;
                                }
                                break;
                            case "LTH_PC":
                                if (LESSTHAN(value1, value2, dataType))
                                {
                                    // ([L1, U1] ⊗pc[L2, U2]) = [min(L1, L2), min(U1, U2)]
                                    result.minprob += Math.Min(triple1.minprob[i], triple2.minprob[j]);
                                    result.maxprob += Math.Min(triple1.maxprob[i], triple2.maxprob[j]);
                                }
                                break;
                            case "LET_IN":
                                if (LESSTHANOREQUAL(value1, value2, dataType))
                                {
                                    // ([L1, U1] ⊗in [L2, U2]) = [L1 . L2, U1 . U2]
                                    result.minprob += triple1.minprob[i] * triple2.minprob[j];
                                    result.maxprob += triple1.maxprob[i] * triple2.maxprob[j];
                                }
                                break;
                            case "LET_IG":
                                if (LESSTHANOREQUAL(value1, value2, dataType))
                                {
                                    // ([L1, U1] ⊗ig [L2, U2]) = [max(0, L1 + L2 – 1), min(U1, U2)]
                                    result.minprob += Math.Max(0, triple1.minprob[i] + triple2.minprob[j] - 1);
                                    result.maxprob += Math.Min(triple1.maxprob[i], triple2.maxprob[j]);
                                }
                                break;
                            case "LET_ME":
                                if (LESSTHANOREQUAL(value1, value2, dataType))
                                {
                                    // ([L1, U1] ⊗me [L2, U2]) = [0, 0]
                                    result.minprob += 0;
                                    result.maxprob += 0;
                                }
                                break;
                            case "LET_PC":
                                if (LESSTHANOREQUAL(value1, value2, dataType))
                                {
                                    // ([L1, U1] ⊗pc[L2, U2]) = [min(L1, L2), min(U1, U2)]
                                    result.minprob += Math.Min(triple1.minprob[i], triple2.minprob[j]);
                                    result.maxprob += Math.Min(triple1.maxprob[i], triple2.maxprob[j]);
                                }
                                break;
                            case "GTH_IN":
                                if (GREATERTHAN(value1, value2, dataType))
                                {
                                    // ([L1, U1] ⊗in [L2, U2]) = [L1 . L2, U1 . U2]
                                    result.minprob += triple1.minprob[i] * triple2.minprob[j];
                                    result.maxprob += triple1.maxprob[i] * triple2.maxprob[j];
                                }
                                break;
                            case "GTH_IG":
                                if (GREATERTHAN(value1, value2, dataType))
                                {
                                    // ([L1, U1] ⊗ig [L2, U2]) = [max(0, L1 + L2 – 1), min(U1, U2)]
                                    result.minprob += Math.Max(0, triple1.minprob[i] + triple2.minprob[j] - 1);
                                    result.maxprob += Math.Min(triple1.maxprob[i], triple2.maxprob[j]);
                                }
                                break;
                            case "GTH_ME":
                                if (GREATERTHAN(value1, value2, dataType))
                                {
                                    // ([L1, U1] ⊗me [L2, U2]) = [0, 0]
                                    result.minprob += 0;
                                    result.maxprob += 0;
                                }
                                break;
                            case "GTH_PC":
                                if (GREATERTHAN(value1, value2, dataType))
                                {
                                    // ([L1, U1] ⊗pc[L2, U2]) = [min(L1, L2), min(U1, U2)]
                                    result.minprob += Math.Min(triple1.minprob[i], triple2.minprob[j]);
                                    result.maxprob += Math.Min(triple1.maxprob[i], triple2.maxprob[j]);
                                }
                                break;
                            case "GET_IN":
                                if (GREATERTHANOREQUAL(value1, value2, dataType))
                                {
                                    // ([L1, U1] ⊗in [L2, U2]) = [L1 . L2, U1 . U2]
                                    result.minprob += triple1.minprob[i] * triple2.minprob[j];
                                    result.maxprob += triple1.maxprob[i] * triple2.maxprob[j];
                                }
                                break;
                            case "GET_IG":
                                if (GREATERTHANOREQUAL(value1, value2, dataType))
                                {
                                    // ([L1, U1] ⊗ig [L2, U2]) = [max(0, L1 + L2 – 1), min(U1, U2)]
                                    result.minprob += Math.Max(0, triple1.minprob[i] + triple2.minprob[j] - 1);
                                    result.maxprob += Math.Min(triple1.maxprob[i], triple2.maxprob[j]);
                                }
                                break;
                            case "GET_ME":
                                if (GREATERTHANOREQUAL(value1, value2, dataType))
                                {
                                    // ([L1, U1] ⊗me [L2, U2]) = [0, 0]
                                    result.minprob += 0;
                                    result.maxprob += 0;
                                }
                                break;
                            case "GET_PC":
                                if (GREATERTHANOREQUAL(value1, value2, dataType))
                                {
                                    // ([L1, U1] ⊗pc[L2, U2]) = [min(L1, L2), min(U1, U2)]
                                    result.minprob += Math.Min(triple1.minprob[i], triple2.minprob[j]);
                                    result.maxprob += Math.Min(triple1.maxprob[i], triple2.maxprob[j]);
                                }
                                break;
                            default:
                                result.minprob += 0;
                                result.maxprob += 0;
                                break;

                        }
                        result.maxprob = Math.Min(1, result.maxprob);
                    }
                return result;                
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        private static ProbInterval CompareProbTripleAndValue(ProbTriple triple, string value, string compareOperator, string dataType)
            // Tính xác suất đạt được khi so sánh một bộ ba xác suất với một giá trị
        {
            string probValue;
            ProbInterval result = new ProbInterval();
            try
            {
                result.minprob = result.maxprob = 0;
                for (int i = 0; i < triple.values.Count; i++)
                    {
                        probValue = triple.values[i].ToString().Trim();

                        switch (compareOperator)
                        {
                            // "=", "==", "<>", "!=", ">", ">=", "<", "<=" 
                            case "=":
                            case "==":
                                if (EQUAL(probValue, value, dataType))
                                {
                                    result.minprob += triple.minprob[i];
                                    result.maxprob += triple.maxprob[i];
                                }
                                break;
                            case "<>":
                            case "!=":
                                if (!EQUAL(probValue, value, dataType))
                                {
                                    result.minprob += triple.minprob[i];
                                    result.maxprob += triple.maxprob[i];
                                }
                                break;
                            case ">":
                                if (GREATERTHAN(probValue, value, dataType))
                                {
                                    result.minprob += triple.minprob[i];
                                    result.maxprob += triple.maxprob[i];
                                }
                                break;
                            case ">=":
                                if (GREATERTHANOREQUAL(probValue, value, dataType))
                                {
                                    result.minprob += triple.minprob[i];
                                    result.maxprob += triple.maxprob[i];
                                }
                                break;
                            case "<":
                                if (LESSTHAN(probValue, value, dataType))
                                {
                                    result.minprob += triple.minprob[i];
                                    result.maxprob += triple.maxprob[i];
                                }
                                break;
                            case "<=":
                                if (LESSTHANOREQUAL(probValue, value, dataType))
                                {
                                    result.minprob += triple.minprob[i];
                                    result.maxprob += triple.maxprob[i];
                                }
                                break;
                            default: 
                                result.minprob += 0;
                                result.maxprob += 0;
                                break;

                        }
                        result.maxprob = Math.Min(1, result.maxprob);
                    }
                return result;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        public static ProbInterval CombineProbabilisticInterval(ProbInterval prob1, ProbInterval prob2, string combStrategy)
            // Tính xác suất kết hợp của 2 khoảng xác suất
        {
            try
            {
                ProbInterval r = new ProbInterval();
                switch (combStrategy)
                {
                    //([L1, U1] CONJ_IG [L2, U2]) = [max(0, L1 + L2 – 1), min(U1, U2)]
                    case "CONJ_IG": r.minprob = Math.Max(0, prob1.minprob + prob2.minprob - 1); r.maxprob = Math.Min(prob1.maxprob, prob2.maxprob); break;

                    // ([L1, U1] CONJ_IN [L2, U2]) = [L1 . L2, U1 . U2]
                    case "CONJ_IN": r.minprob = prob1.minprob * prob2.minprob; r.maxprob = prob1.maxprob * prob2.maxprob; break;

                    // ([L1, U1] CONJ_ME [L2, U2]) = [0, 0]
                    case "CONJ_ME": r.minprob = 0; r.maxprob = 0; break;

                    // ([L1, U1] CONJ_PC [L2, U2]) = [min(L1, L2), min(U1, U2)]
                    case "CONJ_PC": r.minprob = Math.Min(prob1.minprob, prob2.minprob); r.maxprob = Math.Min(prob1.maxprob, prob2.maxprob); break;

                    // ([L1, U1] DISJ_IG [L2, U2]) = [max(L1, L2 ), min(1, U1 + U2)]
                    case "DISJ_IG": r.minprob = Math.Max(prob1.minprob, prob2.minprob); r.maxprob = Math.Min(1, prob1.maxprob + prob2.maxprob); break;

                    // ([L1, U1] DISJ_IN [L2, U2]) = [L1 + L2  – (L1 . L2), U1 + U2  – (U1 . U2)]
                    case "DISJ_IN": r.minprob = prob1.minprob + prob2.minprob - (prob1.minprob * prob2.minprob); r.maxprob = prob1.maxprob + prob2.maxprob - (prob1.maxprob * prob2.maxprob); break;

                    // ([L1, U1] DISJ_ME [L2, U2]) = [min(1, L1 + L2), min(1, U1 + U2)]
                    case "DISJ_ME": r.minprob = Math.Min(1, prob1.minprob + prob2.minprob); r.maxprob = Math.Min(1, prob1.maxprob + prob2.maxprob); break;

                    // ([L1, U1] DISJ_PC [L2, U2]) = [max(L1, L2), max(U1, U2)]
                    case "DISJ_PC": r.minprob = Math.Max(prob1.minprob, prob2.minprob); r.maxprob = Math.Max(prob1.maxprob, prob2.maxprob); break;

                    // ([L1, U1] ⊖ig [L2, U2]) = [max(0, L1 – U2 ), min(U1,1– L2)]
                    case "DIFF_IG": r.minprob = Math.Max(0, prob1.minprob - prob2.maxprob); r.maxprob = Math.Min(prob1.minprob, 1 - prob2.maxprob); break;

                    // ([L1, U1] ⊖in [L2, U2]) = [L1 . (1– U2), U1  . (1– L2)]
                    case "DIFF_IN": r.minprob = prob1.minprob * (1 - prob2.maxprob); r.maxprob = prob1.maxprob * (1 - prob2.minprob); break;

                    // ([L1, U1] ⊖me [L2, U2]) = [L1, min(U1, 1 – L2)]
                    case "DIFF_ME": r.minprob = prob1.minprob; r.maxprob = Math.Min(prob1.maxprob, 1 - prob2.minprob); break;

                    // ([L1, U1] ⊖pc [L2, U2]) = [max(0, L1 – U2), max(0, U1 –L2)]
                    case "DIFF_PC": r.minprob = Math.Max(0, prob1.minprob - prob2.maxprob); r.maxprob = Math.Max(0, prob1.maxprob - prob2.minprob); break;

                    default: return null;
                }
                return r;

            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        public static bool EQUAL(object a, object b, string type)
            // Phép so sánh bằng 2 giá trị dựa theo kiểu dữ liệu của chúng
        {
            switch (type)
            {
                case "int":
                case "int16":
                case "int64":
                case "int32":
                case "byte":
                case "currency":
                case "Int":
                case "Int16":
                case "Int64":
                case "Int32":
                case "Byte":
                case "Currency": return (int.Parse(a.ToString()) == int.Parse(b.ToString()));
                case "string":
                case "datetime":
                case "userdefined":
                case "binary":
                case "String":
                case "DateTime":
                case "UserDefined":
                case "Binary": return a.ToString().Equals(b.ToString());
                case "decimal":
                case "single":
                case "double":
                case "boolean":
                case "Decimal":
                case "Single":
                case "Double": return (Math.Abs(Double.Parse(a.ToString()) - Double.Parse(b.ToString())) < 0.001);
                case "Boolean": return (Boolean.Parse(a.ToString()) == Boolean.Parse(b.ToString()));
                default: return a.ToString().Equals(b.ToString()); // Coi như là kiểu UserDefined
            }
        }

        public static bool LESSTHAN(object a, object b, string type)
            // Phép so sánh nhỏ hơn giữa 2 giá trị dựa theo kiểu dữ liệu của chúng
        {
            switch (type)
            {
                case "int":
                case "int16":
                case "int64":
                case "int32":
                case "byte":
                case "currency":
                case "Int":
                case "Int16":
                case "Int64":
                case "Int32":
                case "Byte":
                case "Currency": return (int.Parse(a.ToString()) < int.Parse(b.ToString()));
                case "string":
                case "datetime":
                case "userdefined":
                case "binary":
                case "String":
                case "DateTime":
                case "UserDefined":
                case "Binary": return (a.ToString().CompareTo(b.ToString()) < 0);
                case "decimal":
                case "single":
                case "double":
                case "boolean":
                case "Decimal":
                case "Single":
                case "Double": return (Double.Parse(a.ToString()) < Double.Parse(b.ToString()));
                case "Boolean": return (Boolean.Parse(a.ToString()) != Boolean.Parse(b.ToString()));
                default: return (a.ToString().CompareTo(b.ToString()) < 0); // Coi như là kiểu UserDefined
            }
        }

        public static bool LESSTHANOREQUAL(object a, object b, string type)
            // Phép so sánh nhỏ hơn hoặc bằng giữa 2 giá trị dựa theo kiểu dữ liệu của chúng
        {
            switch (type)
            {
                case "int":
                case "int16":
                case "int64":
                case "int32":
                case "byte":
                case "currency":
                case "Int":
                case "Int16":
                case "Int64":
                case "Int32":
                case "Byte":
                case "Currency": return (int.Parse(a.ToString()) <= int.Parse(b.ToString()));
                case "string":
                case "datetime":
                case "userdefined":
                case "binary":
                case "String":
                case "DateTime":
                case "UserDefined":
                case "Binary": return ((a.ToString().CompareTo(b.ToString()) < 0) || (a.ToString().CompareTo(b.ToString()) == 0));
                case "decimal":
                case "single":
                case "double":
                case "boolean":
                case "Decimal":
                case "Single":
                case "Double": return (Double.Parse(a.ToString()) <= Double.Parse(b.ToString()));
                case "Boolean": return true;
                default: return ((a.ToString().CompareTo(b.ToString()) < 0) || (a.ToString().CompareTo(b.ToString()) == 0));
            }
        }

        public static bool GREATERTHAN(object a, object b, string type)
            // Phép so sánh lớn hơn giữa 2 giá trị dựa theo kiểu dữ liệu của chúng
        {
            switch (type)
            {
                case "int":
                case "int16":
                case "int64":
                case "int32":
                case "byte":
                case "currency":
                case "Int":
                case "Int16":
                case "Int64":
                case "Int32":
                case "Byte":
                case "Currency": return (int.Parse(a.ToString()) > int.Parse(b.ToString()));
                case "string":
                case "datetime":
                case "userdefined":
                case "binary":
                case "String":
                case "DateTime":
                case "UserDefined":
                case "Binary": return (a.ToString().CompareTo(b.ToString()) > 0);
                case "decimal":
                case "single":
                case "double":
                case "boolean":
                case "Decimal":
                case "Single":
                case "Double": return (Double.Parse(a.ToString()) > Double.Parse(b.ToString()));
                case "Boolean": return (Boolean.Parse(a.ToString()) != Boolean.Parse(b.ToString()));
                default: return (a.ToString().CompareTo(b.ToString()) > 0);
            }
        }

        public static bool GREATERTHANOREQUAL(object a, object b, string type)
            // Phép so sánh lớn hơn hoặc bằng giữa 2 giá trị dựa theo kiểu dữ liệu của chúng
        {
            switch (type)
            {
                case "int":
                case "int16":
                case "int64":
                case "int32":
                case "byte":
                case "currency":
                case "Int":
                case "Int16":
                case "Int64":
                case "Int32":
                case "Byte":
                case "Currency": return (int.Parse(a.ToString()) >= int.Parse(b.ToString()));
                case "string":
                case "datetime":
                case "userdefined":
                case "binary":
                case "String":
                case "DateTime":
                case "UserDefined":
                case "Binary": return ((a.ToString().CompareTo(b.ToString()) > 0) || (a.ToString().CompareTo(b.ToString()) == 0));
                case "decimal":
                case "single":
                case "double":
                case "boolean":
                case "Decimal":
                case "Single":
                case "Double": return (Double.Parse(a.ToString()) >= Double.Parse(b.ToString()));
                case "Boolean": return true;
                default: return ((a.ToString().CompareTo(b.ToString()) > 0) || (a.ToString().CompareTo(b.ToString()) == 0));
            }
        }

        public static ProbTriple CONJ_IG_2ProbTriples(ProbTriple triple1, ProbTriple triple2, string dataType)
        // Kết hợp 2 bộ ba xác suất bằng phép Hội theo chiến lược Ignorance
        {
            try
            {
                // Bộ ba xác suất kết quả của phép Hội
                ProbTriple triple = new ProbTriple();

                for (int i = 0; i < triple1.values.Count; i++)
                {
                    for (int j = 0; j < triple2.values.Count; j++)
                        if (EQUAL(triple1.values[i], triple2.values[j], dataType))
                        {
                            triple.values.Add(triple1.values[i]);

                            // ([L1, U1] CONJ_IG [L2, U2]) = [max(0, L1 + L2 – 1), min(U1, U2)]
                            triple.minprob.Add(Math.Max(0, triple1.minprob[i] + triple2.minprob[j] - 1));
                            triple.maxprob.Add(Math.Min(triple1.maxprob[i], triple2.maxprob[j]));

                            break;
                        }
                }

                return triple;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        public static ProbTriple CONJ_IN_2ProbTriples(ProbTriple triple1, ProbTriple triple2, string dataType)
        // Kết hợp 2 bộ ba xác suất bằng phép Hội theo chiến lược Independence
        {
            try
            {
                // Bộ ba xác suất kết quả của phép Hội
                ProbTriple triple = new ProbTriple();

                for (int i = 0; i < triple1.values.Count; i++)
                {
                    for (int j = 0; j < triple2.values.Count; j++)
                        if (EQUAL(triple1.values[i], triple2.values[j], dataType))
                        {
                            triple.values.Add(triple1.values[i]);

                            // ([L1, U1] CONJ_IN [L2, U2]) = [L1 . L2, U1 . U2]
                            triple.minprob.Add(triple1.minprob[i] * triple2.minprob[j]);
                            triple.maxprob.Add(triple1.maxprob[i] * triple2.maxprob[j]);

                            break;
                        }
                }

                return triple;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        public static ProbTriple CONJ_ME_2ProbTriples(ProbTriple triple1, ProbTriple triple2, string dataType)
        // Kết hợp 2 bộ ba xác suất bằng phép Hội theo chiến lược Mutual Exclusion
        {
            try
            {
                // Bộ ba xác suất kết quả của phép Hội
                ProbTriple triple = new ProbTriple();

                for (int i = 0; i < triple1.values.Count; i++)
                {
                    for (int j = 0; j < triple2.values.Count; j++)
                        if (EQUAL(triple1.values[i], triple2.values[j], dataType))
                        {
                            triple.values.Add(triple1.values[i]);

                            // ([L1, U1] CONJ_ME [L2, U2]) = [0, 0]
                            triple.minprob.Add(0);
                            triple.maxprob.Add(0);

                            break;
                        }
                }

                return triple;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        public static ProbTriple CONJ_PC_2ProbTriples(ProbTriple triple1, ProbTriple triple2, string dataType)
        // Kết hợp 2 bộ ba xác suất bằng phép Hội theo chiến lược Positive Correlation
        {
            try
            {
                // Bộ ba xác suất kết quả của phép Hội
                ProbTriple triple = new ProbTriple();

                for (int i = 0; i < triple1.values.Count; i++)
                {
                    for (int j = 0; j < triple2.values.Count; j++)
                        if (EQUAL(triple1.values[i], triple2.values[j], dataType))
                        {
                            triple.values.Add(triple1.values[i]);

                            // ([L1, U1] CONJ_PC [L2, U2]) = [min(L1, L2), min(U1, U2)]
                            triple.minprob.Add(Math.Min(triple1.minprob[i], triple2.minprob[j]));
                            triple.maxprob.Add(Math.Min(triple1.maxprob[i], triple2.maxprob[j]));

                            break;
                        }
                }

                return triple;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        public static ProbTriple DISJ_IG_2ProbTriples(ProbTriple triple1, ProbTriple triple2, string dataType)
            // Kết hợp 2 bộ ba xác suất bằng phép Tuyển theo chiến lược Ignorance
        {
            try
            {
                // Bộ ba xác suất kết quả của phép Tuyển
                ProbTriple triple = new ProbTriple();

                // Ban đầu triple = triple1
                triple.Assign(triple1);

                bool equalValue;
                // Xét từng giá trị trong triple2
                for (int j = 0; j < triple2.values.Count; j++)                                
                {
                    equalValue = false;
                    // Với mỗi giá trị của triple2, xét lại từng giá trị của triple1 xem có giá trị nào bằng nhau hay không
                    for (int i = 0; i < triple1.values.Count; i++)
                        if (EQUAL(triple1.values[i], triple2.values[j], dataType))
                        // Trường hợp triple2.value == triple1.value ==> giữ nguyên triple.value, tính lại triple.minprob và triple.maxprob
                        {
                            // ([L1, U1] DISJ_IG [L2, U2]) = [max(L1, L2 ), min(1, U1 + U2)]
                            triple.minprob[i] = Math.Max(triple1.minprob[i], triple2.minprob[j]);
                            triple.maxprob[i] = Math.Min(1, triple1.maxprob[i] + triple2.maxprob[j]);

                            equalValue = true;
                            break;
                        }

                    // Trường hợp triple2.value không nằm trong triple.values ==> add triple2.{value, minprob, maxprob} vào triple
                    if (!equalValue)
                    {
                        triple.values.Add(triple2.values[j]);
                        triple.minprob.Add(triple2.minprob[j]);
                        triple.maxprob.Add(triple2.maxprob[j]);
                    }
                }
                return triple;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        public static ProbTriple DISJ_IN_2ProbTriples(ProbTriple triple1, ProbTriple triple2, string dataType)
        // Kết hợp 2 bộ ba xác suất bằng phép Tuyển theo chiến lược Independence
        {
            try
            {
                // Bộ ba xác suất kết quả của phép Tuyển
                ProbTriple triple = new ProbTriple();

                // Ban đầu triple = triple1
                triple.Assign(triple1);

                bool equalValue;
                // Xét từng giá trị trong triple2
                for (int j = 0; j < triple2.values.Count; j++)
                {
                    equalValue = false;
                    // Với mỗi giá trị của triple2, xét lại từng giá trị của triple1 xem có giá trị nào bằng nhau hay không
                    for (int i = 0; i < triple1.values.Count; i++)
                        if (EQUAL(triple1.values[i], triple2.values[j], dataType))
                        // Trường hợp triple2.value == triple1.value ==> giữ nguyên triple.value, tính lại triple.minprob và triple.maxprob
                        {
                            // ([L1, U1] DISJ_IN [L2, U2]) = [L1 + L2  – (L1 . L2), U1 + U2  – (U1 . U2)]
                            triple.minprob[i] = triple1.minprob[i] + triple2.minprob[j] - (triple1.minprob[i] * triple2.minprob[j]);
                            triple.maxprob[i] = triple1.maxprob[i] + triple2.maxprob[j] - (triple1.maxprob[i] * triple2.maxprob[j]);

                            equalValue = true;
                            break;
                        }

                    // Trường hợp triple2.value không nằm trong triple.values ==> add triple2.{value, minprob, maxprob} vào triple
                    if (!equalValue)
                    {
                        triple.values.Add(triple2.values[j]);
                        triple.minprob.Add(triple2.minprob[j]);
                        triple.maxprob.Add(triple2.maxprob[j]);
                    }
                }
                return triple;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        public static ProbTriple DISJ_ME_2ProbTriples(ProbTriple triple1, ProbTriple triple2, string dataType)
        // Kết hợp 2 bộ ba xác suất bằng phép Tuyển theo chiến lược Mutual Exclusion
        {
            try
            {
                // Bộ ba xác suất kết quả của phép Tuyển
                ProbTriple triple = new ProbTriple();

                // Ban đầu triple = triple1
                triple.Assign(triple1);

                bool equalValue;
                // Xét từng giá trị trong triple2
                for (int j = 0; j < triple2.values.Count; j++)
                {
                    equalValue = false;
                    // Với mỗi giá trị của triple2, xét lại từng giá trị của triple1 xem có giá trị nào bằng nhau hay không
                    for (int i = 0; i < triple1.values.Count; i++)
                        if (EQUAL(triple1.values[i], triple2.values[j], dataType))
                        // Trường hợp triple2.value == triple1.value ==> giữ nguyên triple.value, tính lại triple.minprob và triple.maxprob
                        {
                            // ([L1, U1] DISJ_ME [L2, U2]) = [min(1, L1 + L2), min(1, U1 + U2)]
                            triple.minprob[i] = Math.Min(1, triple1.minprob[i] + triple2.minprob[j]);
                            triple.maxprob[i] = Math.Min(1, triple1.maxprob[i] + triple2.maxprob[j]);

                            equalValue = true;
                            break;
                        }

                    // Trường hợp triple2.value không nằm trong triple.values ==> add triple2.{value, minprob, maxprob} vào triple
                    if (!equalValue)
                    {
                        triple.values.Add(triple2.values[j]);
                        triple.minprob.Add(triple2.minprob[j]);
                        triple.maxprob.Add(triple2.maxprob[j]);
                    }
                }
                return triple;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        public static ProbTriple DISJ_PC_2ProbTriples(ProbTriple triple1, ProbTriple triple2, string dataType)
        // Kết hợp 2 bộ ba xác suất bằng phép Tuyển theo chiến lược Positive Correlation
        {
            try
            {
                // Bộ ba xác suất kết quả của phép Tuyển
                ProbTriple triple = new ProbTriple();

                // Ban đầu triple = triple1
                triple.Assign(triple1);

                bool equalValue;
                // Xét từng giá trị trong triple2
                for (int j = 0; j < triple2.values.Count; j++)
                {
                    equalValue = false;
                    // Với mỗi giá trị của triple2, xét lại từng giá trị của triple1 xem có giá trị nào bằng nhau hay không
                    for (int i = 0; i < triple1.values.Count; i++)
                        if (EQUAL(triple1.values[i], triple2.values[j], dataType))
                        // Trường hợp triple2.value == triple1.value ==> giữ nguyên triple.value, tính lại triple.minprob và triple.maxprob
                        {
                            // ([L1, U1] DISJ_PC [L2, U2]) = [max(L1, L2), max(U1, U2)]
                            triple.minprob[i] = Math.Max(triple1.minprob[i], triple2.minprob[j]);
                            triple.maxprob[i] = Math.Max(triple1.maxprob[i], triple2.maxprob[j]);

                            equalValue = true;
                            break;
                        }

                    // Trường hợp triple2.value không nằm trong triple.values ==> add triple2.{value, minprob, maxprob} vào triple
                    if (!equalValue)
                    {
                        triple.values.Add(triple2.values[j]);
                        triple.minprob.Add(triple2.minprob[j]);
                        triple.maxprob.Add(triple2.maxprob[j]);
                    }
                }
                return triple;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        public static ProbTriple DIFF_IG_2ProbTriples(ProbTriple triple1, ProbTriple triple2, string dataType)
        // Chiến lược Trừ của  2 bộ ba xác suất: triple1 ⊖ig triple2 theo Ignorance
        {
            try
            {
                // Bộ ba xác suất kết quả của phép Hiệu
                ProbTriple triple = new ProbTriple();

                bool equalValue;
                double minprob, maxprob;
                for (int i = 0; i < triple1.values.Count; i++)
                {
                    equalValue = false;
                    // Với mỗi giá trị trong triple1, xét từng giá trị của triple2
                    for (int j = 0; j < triple2.values.Count; j++)
                        // Nếu tồn tại triple2.value[j] == triple1.value[i] ==> kiểm tra khoảng xác suất kết hợp của 2 giá trị có khác [0, 0]
                        if (EQUAL(triple1.values[i], triple2.values[j], dataType))
                        {
                            // ([L1, U1] ⊖ig [L2, U2]) = [max(0, L1 – U2 ), min(U1,1– L2)]
                            minprob = Math.Max(0, triple1.minprob[i] - triple2.maxprob[j]);
                            maxprob = Math.Min(triple1.maxprob[i], 1 - triple2.minprob[j]);
                            
                            // Loại bỏ các giá trị mà khoảng xác suất của chúng là [0, 0]
                            if (minprob != 0 || maxprob != 0)
                            {
                                triple.values.Add(triple1.values[i]);
                                triple.minprob.Add(minprob);
                                triple.maxprob.Add(maxprob);
                            }
                            equalValue = true;
                            break;
                        }

                    // Nếu triple1.value không nằm trong triple2.values ==> add triple1.{value, minprob, maxprob} vào triple
                    if (!equalValue)
                    {
                        triple.values.Add(triple1.values[i]);
                        triple.minprob.Add(triple1.minprob[i]);
                        triple.maxprob.Add(triple1.maxprob[i]);
                    }
                }

                return triple;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        public static ProbTriple DIFF_IN_2ProbTriples(ProbTriple triple1, ProbTriple triple2, string dataType)
        // Chiến lược Trừ của  2 bộ ba xác suất: triple1 ⊖in triple2 theo Independence
        {
            try
            {
                // Bộ ba xác suất kết quả của phép Hiệu
                ProbTriple triple = new ProbTriple();

                bool equalValue;
                double minprob, maxprob;
                for (int i = 0; i < triple1.values.Count; i++)
                {
                    equalValue = false;
                    // Với mỗi giá trị trong triple1, xét từng giá trị của triple2
                    for (int j = 0; j < triple2.values.Count; j++)
                        // Nếu tồn tại triple2.value[j] == triple1.value[i] ==> kiểm tra khoảng xác suất kết hợp của 2 giá trị có khác [0, 0]
                        if (EQUAL(triple1.values[i], triple2.values[j], dataType))
                        {
                            // ([L1, U1] ⊖in [L2, U2]) = [L1 . (1– U2), U1  . (1– L2)]
                            minprob = triple1.minprob[i] * (1 - triple2.maxprob[j]);
                            maxprob = triple1.maxprob[i] * (1 - triple2.minprob[j]);

                            // Loại bỏ các giá trị mà khoảng xác suất của chúng là [0, 0]
                            if (minprob != 0 || maxprob != 0)
                            {
                                triple.values.Add(triple1.values[i]);
                                triple.minprob.Add(minprob);
                                triple.maxprob.Add(maxprob);
                            }
                            equalValue = true;
                            break;
                        }

                    // Nếu triple1.value không nằm trong triple2.values ==> add triple1.{value, minprob, maxprob} vào triple
                    if (!equalValue)
                    {
                        triple.values.Add(triple1.values[i]);
                        triple.minprob.Add(triple1.minprob[i]);
                        triple.maxprob.Add(triple1.maxprob[i]);
                    }
                }

                return triple;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        public static ProbTriple DIFF_ME_2ProbTriples(ProbTriple triple1, ProbTriple triple2, string dataType)
        // Chiến lược Trừ của  2 bộ ba xác suất: triple1 ⊖me triple2 theo Mutual Exclusion
        {
            try
            {
                // Bộ ba xác suất kết quả của phép Hiệu
                ProbTriple triple = new ProbTriple();

                bool equalValue;
                double minprob, maxprob;
                for (int i = 0; i < triple1.values.Count; i++)
                {
                    equalValue = false;
                    // Với mỗi giá trị trong triple1, xét từng giá trị của triple2
                    for (int j = 0; j < triple2.values.Count; j++)
                        // Nếu tồn tại triple2.value[j] == triple1.value[i] ==> kiểm tra khoảng xác suất kết hợp của 2 giá trị có khác [0, 0]
                        if (EQUAL(triple1.values[i], triple2.values[j], dataType))
                        {
                            // ([L1, U1] ⊖me [L2, U2]) = [L1, min(U1, 1 – L2)]
                            minprob = triple1.minprob[i];
                            maxprob = Math.Min(triple1.maxprob[i], 1 - triple2.minprob[j]);

                            // Loại bỏ các giá trị mà khoảng xác suất của chúng là [0, 0]
                            if (minprob != 0 || maxprob != 0)
                            {
                                triple.values.Add(triple1.values[i]);
                                triple.minprob.Add(minprob);
                                triple.maxprob.Add(maxprob);
                            }
                            equalValue = true;
                            break;
                        }

                    // Nếu triple1.value không nằm trong triple2.values ==> add triple1.{value, minprob, maxprob} vào triple
                    if (!equalValue)
                    {
                        triple.values.Add(triple1.values[i]);
                        triple.minprob.Add(triple1.minprob[i]);
                        triple.maxprob.Add(triple1.maxprob[i]);
                    }
                }

                return triple;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        public static ProbTriple DIFF_PC_2ProbTriples(ProbTriple triple1, ProbTriple triple2, string dataType)
        // Chiến lược Trừ của  2 bộ ba xác suất: triple1 ⊖pc triple2 theo Positive Correlation
        {
            try
            {
                // Bộ ba xác suất kết quả của phép Hiệu
                ProbTriple triple = new ProbTriple();

                bool equalValue;
                double minprob, maxprob;
                for (int i = 0; i < triple1.values.Count; i++)
                {
                    equalValue = false;
                    // Với mỗi giá trị trong triple1, xét từng giá trị của triple2
                    for (int j = 0; j < triple2.values.Count; j++)
                        // Nếu tồn tại triple2.value[j] == triple1.value[i] ==> kiểm tra khoảng xác suất kết hợp của 2 giá trị có khác [0, 0]
                        if (EQUAL(triple1.values[i], triple2.values[j], dataType))
                        {
                            // ([L1, U1] ⊖pc [L2, U2]) = [max(0, L1 – U2), max(0, U1 –L2)]
                            minprob = Math.Max(0, triple1.minprob[i] - triple2.maxprob[j]);
                            maxprob = Math.Max(0, triple1.maxprob[i] - triple2.minprob[j]);

                            // Loại bỏ các giá trị mà khoảng xác suất của chúng là [0, 0]
                            if (minprob != 0 || maxprob != 0)
                            {
                                triple.values.Add(triple1.values[i]);
                                triple.minprob.Add(minprob);
                                triple.maxprob.Add(maxprob);
                            }
                            equalValue = true;
                            break;
                        }

                    // Nếu triple1.value không nằm trong triple2.values ==> add triple1.{value, minprob, maxprob} vào triple
                    if (!equalValue)
                    {
                        triple.values.Add(triple1.values[i]);
                        triple.minprob.Add(triple1.minprob[i]);
                        triple.maxprob.Add(triple1.maxprob[i]);
                    }
                }

                return triple;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        public static ProbTuple CombineProbabilisticTuple(ProbTuple tuple1, ProbTuple tuple2, string combOperator)
        // Hàm kết hợp các bộ của quan hệ theo các phép toán UNION, INTERSECT, MINUS
        {
            try
            {
                ProbTuple tuple = new ProbTuple();
                ProbTriple triple1, triple2, triple;
                string dataType;
                // Lấy ra từng ProbTriple có cùng ProbAttribute của 2 bộ để kết hợp lại với nhau
                foreach (ProbAttribute attr1 in tuple1.triples.Keys)
                {
                    foreach (ProbAttribute attr2 in tuple2.triples.Keys)
                        if (attr1.IsEqualTo(attr2))
                        {
                            triple1 = tuple1.triples[attr1];
                            triple2 = tuple2.triples[attr2];
                            triple = new ProbTriple();
                            dataType = attr1.type.typeName;
                            switch (combOperator)
                            {
                                case "UNION_IG": triple = DISJ_IG_2ProbTriples(triple1, triple2, dataType); break;
                                case "UNION_IN": triple = DISJ_IN_2ProbTriples(triple1, triple2, dataType); break;
                                case "UNION_ME": triple = DISJ_ME_2ProbTriples(triple1, triple2, dataType); break;
                                case "UNION_PC": triple = DISJ_PC_2ProbTriples(triple1, triple2, dataType); break;
                                case "INTERSECT_IG": triple = CONJ_IG_2ProbTriples(triple1, triple2, dataType); break;
                                case "INTERSECT_IN": triple = CONJ_IN_2ProbTriples(triple1, triple2, dataType); break;
                                case "INTERSECT_ME": triple = CONJ_ME_2ProbTriples(triple1, triple2, dataType); break;
                                case "INTERSECT_PC": triple = CONJ_PC_2ProbTriples(triple1, triple2, dataType); break;
                                case "MINUS_IG": triple = DIFF_IG_2ProbTriples(triple1, triple2, dataType); break;
                                case "MINUS_IN": triple = DIFF_IN_2ProbTriples(triple1, triple2, dataType); break;
                                case "MINUS_ME": triple = DIFF_ME_2ProbTriples(triple1, triple2, dataType); break;
                                case "MINUS_PC": triple = DIFF_PC_2ProbTriples(triple1, triple2, dataType); break;
                                default: break;
                            }

                            // Add triple vừa tạo vào tuple mới
                            if (triple.values.Count == 0) return null;
                            tuple.triples.Add(attr1, triple);
                            break;
                        }
                }
                return tuple;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        public static void Execute(string strQuery, ProbDatabase probDatabase)
            // Thực thi câu truy vấn
        {
            try
            {
                string queryString = StandardizeQuery(strQuery);
                probDB = probDatabase;
                SelectStatement selectStatement = ParseQueryTree(queryString);
                satisfiedRelation = ProcessSelectStatement(selectStatement);
            }
            catch (Exception Ex)
            {
            }
        }

    }
}
