// Generated from ProbSQL.g4 by ANTLR 4.2
import org.antlr.v4.runtime.misc.NotNull;
import org.antlr.v4.runtime.tree.ParseTreeListener;

/**
 * This interface defines a complete listener for a parse tree produced by
 * {@link ProbSQLParser}.
 */
public interface ProbSQLListener extends ParseTreeListener {
	/**
	 * Enter a parse tree produced by {@link ProbSQLParser#where_clause}.
	 * @param ctx the parse tree
	 */
	void enterWhere_clause(@NotNull ProbSQLParser.Where_clauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link ProbSQLParser#where_clause}.
	 * @param ctx the parse tree
	 */
	void exitWhere_clause(@NotNull ProbSQLParser.Where_clauseContext ctx);

	/**
	 * Enter a parse tree produced by {@link ProbSQLParser#subquery}.
	 * @param ctx the parse tree
	 */
	void enterSubquery(@NotNull ProbSQLParser.SubqueryContext ctx);
	/**
	 * Exit a parse tree produced by {@link ProbSQLParser#subquery}.
	 * @param ctx the parse tree
	 */
	void exitSubquery(@NotNull ProbSQLParser.SubqueryContext ctx);

	/**
	 * Enter a parse tree produced by {@link ProbSQLParser#subquery_operator}.
	 * @param ctx the parse tree
	 */
	void enterSubquery_operator(@NotNull ProbSQLParser.Subquery_operatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link ProbSQLParser#subquery_operator}.
	 * @param ctx the parse tree
	 */
	void exitSubquery_operator(@NotNull ProbSQLParser.Subquery_operatorContext ctx);

	/**
	 * Enter a parse tree produced by {@link ProbSQLParser#boolean_literal}.
	 * @param ctx the parse tree
	 */
	void enterBoolean_literal(@NotNull ProbSQLParser.Boolean_literalContext ctx);
	/**
	 * Exit a parse tree produced by {@link ProbSQLParser#boolean_literal}.
	 * @param ctx the parse tree
	 */
	void exitBoolean_literal(@NotNull ProbSQLParser.Boolean_literalContext ctx);

	/**
	 * Enter a parse tree produced by {@link ProbSQLParser#select_statement}.
	 * @param ctx the parse tree
	 */
	void enterSelect_statement(@NotNull ProbSQLParser.Select_statementContext ctx);
	/**
	 * Exit a parse tree produced by {@link ProbSQLParser#select_statement}.
	 * @param ctx the parse tree
	 */
	void exitSelect_statement(@NotNull ProbSQLParser.Select_statementContext ctx);

	/**
	 * Enter a parse tree produced by {@link ProbSQLParser#select_expression}.
	 * @param ctx the parse tree
	 */
	void enterSelect_expression(@NotNull ProbSQLParser.Select_expressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link ProbSQLParser#select_expression}.
	 * @param ctx the parse tree
	 */
	void exitSelect_expression(@NotNull ProbSQLParser.Select_expressionContext ctx);

	/**
	 * Enter a parse tree produced by {@link ProbSQLParser#compare_value_operator}.
	 * @param ctx the parse tree
	 */
	void enterCompare_value_operator(@NotNull ProbSQLParser.Compare_value_operatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link ProbSQLParser#compare_value_operator}.
	 * @param ctx the parse tree
	 */
	void exitCompare_value_operator(@NotNull ProbSQLParser.Compare_value_operatorContext ctx);

	/**
	 * Enter a parse tree produced by {@link ProbSQLParser#literal_value}.
	 * @param ctx the parse tree
	 */
	void enterLiteral_value(@NotNull ProbSQLParser.Literal_valueContext ctx);
	/**
	 * Exit a parse tree produced by {@link ProbSQLParser#literal_value}.
	 * @param ctx the parse tree
	 */
	void exitLiteral_value(@NotNull ProbSQLParser.Literal_valueContext ctx);

	/**
	 * Enter a parse tree produced by {@link ProbSQLParser#expression_connector}.
	 * @param ctx the parse tree
	 */
	void enterExpression_connector(@NotNull ProbSQLParser.Expression_connectorContext ctx);
	/**
	 * Exit a parse tree produced by {@link ProbSQLParser#expression_connector}.
	 * @param ctx the parse tree
	 */
	void exitExpression_connector(@NotNull ProbSQLParser.Expression_connectorContext ctx);

	/**
	 * Enter a parse tree produced by {@link ProbSQLParser#attribute_name}.
	 * @param ctx the parse tree
	 */
	void enterAttribute_name(@NotNull ProbSQLParser.Attribute_nameContext ctx);
	/**
	 * Exit a parse tree produced by {@link ProbSQLParser#attribute_name}.
	 * @param ctx the parse tree
	 */
	void exitAttribute_name(@NotNull ProbSQLParser.Attribute_nameContext ctx);

	/**
	 * Enter a parse tree produced by {@link ProbSQLParser#attribute}.
	 * @param ctx the parse tree
	 */
	void enterAttribute(@NotNull ProbSQLParser.AttributeContext ctx);
	/**
	 * Exit a parse tree produced by {@link ProbSQLParser#attribute}.
	 * @param ctx the parse tree
	 */
	void exitAttribute(@NotNull ProbSQLParser.AttributeContext ctx);

	/**
	 * Enter a parse tree produced by {@link ProbSQLParser#selection_condition_atom}.
	 * @param ctx the parse tree
	 */
	void enterSelection_condition_atom(@NotNull ProbSQLParser.Selection_condition_atomContext ctx);
	/**
	 * Exit a parse tree produced by {@link ProbSQLParser#selection_condition_atom}.
	 * @param ctx the parse tree
	 */
	void exitSelection_condition_atom(@NotNull ProbSQLParser.Selection_condition_atomContext ctx);

	/**
	 * Enter a parse tree produced by {@link ProbSQLParser#bit_literal}.
	 * @param ctx the parse tree
	 */
	void enterBit_literal(@NotNull ProbSQLParser.Bit_literalContext ctx);
	/**
	 * Exit a parse tree produced by {@link ProbSQLParser#bit_literal}.
	 * @param ctx the parse tree
	 */
	void exitBit_literal(@NotNull ProbSQLParser.Bit_literalContext ctx);

	/**
	 * Enter a parse tree produced by {@link ProbSQLParser#expression_atom}.
	 * @param ctx the parse tree
	 */
	void enterExpression_atom(@NotNull ProbSQLParser.Expression_atomContext ctx);
	/**
	 * Exit a parse tree produced by {@link ProbSQLParser#expression_atom}.
	 * @param ctx the parse tree
	 */
	void exitExpression_atom(@NotNull ProbSQLParser.Expression_atomContext ctx);

	/**
	 * Enter a parse tree produced by {@link ProbSQLParser#init}.
	 * @param ctx the parse tree
	 */
	void enterInit(@NotNull ProbSQLParser.InitContext ctx);
	/**
	 * Exit a parse tree produced by {@link ProbSQLParser#init}.
	 * @param ctx the parse tree
	 */
	void exitInit(@NotNull ProbSQLParser.InitContext ctx);

	/**
	 * Enter a parse tree produced by {@link ProbSQLParser#relation_connector}.
	 * @param ctx the parse tree
	 */
	void enterRelation_connector(@NotNull ProbSQLParser.Relation_connectorContext ctx);
	/**
	 * Exit a parse tree produced by {@link ProbSQLParser#relation_connector}.
	 * @param ctx the parse tree
	 */
	void exitRelation_connector(@NotNull ProbSQLParser.Relation_connectorContext ctx);

	/**
	 * Enter a parse tree produced by {@link ProbSQLParser#expression}.
	 * @param ctx the parse tree
	 */
	void enterExpression(@NotNull ProbSQLParser.ExpressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link ProbSQLParser#expression}.
	 * @param ctx the parse tree
	 */
	void exitExpression(@NotNull ProbSQLParser.ExpressionContext ctx);

	/**
	 * Enter a parse tree produced by {@link ProbSQLParser#join_relation_list}.
	 * @param ctx the parse tree
	 */
	void enterJoin_relation_list(@NotNull ProbSQLParser.Join_relation_listContext ctx);
	/**
	 * Exit a parse tree produced by {@link ProbSQLParser#join_relation_list}.
	 * @param ctx the parse tree
	 */
	void exitJoin_relation_list(@NotNull ProbSQLParser.Join_relation_listContext ctx);

	/**
	 * Enter a parse tree produced by {@link ProbSQLParser#join_operator}.
	 * @param ctx the parse tree
	 */
	void enterJoin_operator(@NotNull ProbSQLParser.Join_operatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link ProbSQLParser#join_operator}.
	 * @param ctx the parse tree
	 */
	void exitJoin_operator(@NotNull ProbSQLParser.Join_operatorContext ctx);

	/**
	 * Enter a parse tree produced by {@link ProbSQLParser#hex_literal}.
	 * @param ctx the parse tree
	 */
	void enterHex_literal(@NotNull ProbSQLParser.Hex_literalContext ctx);
	/**
	 * Exit a parse tree produced by {@link ProbSQLParser#hex_literal}.
	 * @param ctx the parse tree
	 */
	void exitHex_literal(@NotNull ProbSQLParser.Hex_literalContext ctx);

	/**
	 * Enter a parse tree produced by {@link ProbSQLParser#scheme_name}.
	 * @param ctx the parse tree
	 */
	void enterScheme_name(@NotNull ProbSQLParser.Scheme_nameContext ctx);
	/**
	 * Exit a parse tree produced by {@link ProbSQLParser#scheme_name}.
	 * @param ctx the parse tree
	 */
	void exitScheme_name(@NotNull ProbSQLParser.Scheme_nameContext ctx);

	/**
	 * Enter a parse tree produced by {@link ProbSQLParser#string_literal}.
	 * @param ctx the parse tree
	 */
	void enterString_literal(@NotNull ProbSQLParser.String_literalContext ctx);
	/**
	 * Exit a parse tree produced by {@link ProbSQLParser#string_literal}.
	 * @param ctx the parse tree
	 */
	void exitString_literal(@NotNull ProbSQLParser.String_literalContext ctx);

	/**
	 * Enter a parse tree produced by {@link ProbSQLParser#selection_condition}.
	 * @param ctx the parse tree
	 */
	void enterSelection_condition(@NotNull ProbSQLParser.Selection_conditionContext ctx);
	/**
	 * Exit a parse tree produced by {@link ProbSQLParser#selection_condition}.
	 * @param ctx the parse tree
	 */
	void exitSelection_condition(@NotNull ProbSQLParser.Selection_conditionContext ctx);

	/**
	 * Enter a parse tree produced by {@link ProbSQLParser#attribute_list}.
	 * @param ctx the parse tree
	 */
	void enterAttribute_list(@NotNull ProbSQLParser.Attribute_listContext ctx);
	/**
	 * Exit a parse tree produced by {@link ProbSQLParser#attribute_list}.
	 * @param ctx the parse tree
	 */
	void exitAttribute_list(@NotNull ProbSQLParser.Attribute_listContext ctx);

	/**
	 * Enter a parse tree produced by {@link ProbSQLParser#condition_connector}.
	 * @param ctx the parse tree
	 */
	void enterCondition_connector(@NotNull ProbSQLParser.Condition_connectorContext ctx);
	/**
	 * Exit a parse tree produced by {@link ProbSQLParser#condition_connector}.
	 * @param ctx the parse tree
	 */
	void exitCondition_connector(@NotNull ProbSQLParser.Condition_connectorContext ctx);

	/**
	 * Enter a parse tree produced by {@link ProbSQLParser#number_literal}.
	 * @param ctx the parse tree
	 */
	void enterNumber_literal(@NotNull ProbSQLParser.Number_literalContext ctx);
	/**
	 * Exit a parse tree produced by {@link ProbSQLParser#number_literal}.
	 * @param ctx the parse tree
	 */
	void exitNumber_literal(@NotNull ProbSQLParser.Number_literalContext ctx);

	/**
	 * Enter a parse tree produced by {@link ProbSQLParser#probabilistic_interval}.
	 * @param ctx the parse tree
	 */
	void enterProbabilistic_interval(@NotNull ProbSQLParser.Probabilistic_intervalContext ctx);
	/**
	 * Exit a parse tree produced by {@link ProbSQLParser#probabilistic_interval}.
	 * @param ctx the parse tree
	 */
	void exitProbabilistic_interval(@NotNull ProbSQLParser.Probabilistic_intervalContext ctx);

	/**
	 * Enter a parse tree produced by {@link ProbSQLParser#relation_name}.
	 * @param ctx the parse tree
	 */
	void enterRelation_name(@NotNull ProbSQLParser.Relation_nameContext ctx);
	/**
	 * Exit a parse tree produced by {@link ProbSQLParser#relation_name}.
	 * @param ctx the parse tree
	 */
	void exitRelation_name(@NotNull ProbSQLParser.Relation_nameContext ctx);

	/**
	 * Enter a parse tree produced by {@link ProbSQLParser#compare_attribute_operator}.
	 * @param ctx the parse tree
	 */
	void enterCompare_attribute_operator(@NotNull ProbSQLParser.Compare_attribute_operatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link ProbSQLParser#compare_attribute_operator}.
	 * @param ctx the parse tree
	 */
	void exitCompare_attribute_operator(@NotNull ProbSQLParser.Compare_attribute_operatorContext ctx);
}