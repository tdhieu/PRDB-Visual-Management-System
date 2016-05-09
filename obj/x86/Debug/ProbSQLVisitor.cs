//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.2.2-SNAPSHOT
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from D:\Study Folder\CHBK\Cao Hoc\Master Thesis\Phan Mem\PRDB Visual Management\ProbSQLCompiler\ProbSQL.g4 by ANTLR 4.2.2-SNAPSHOT

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591

namespace PRDB_Visual_Management.ProbSQLCompiler {
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="ProbSQLParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.2.2-SNAPSHOT")]
[System.CLSCompliant(false)]
public interface IProbSQLVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.where_clause"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitWhere_clause([NotNull] ProbSQLParser.Where_clauseContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.subquery"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSubquery([NotNull] ProbSQLParser.SubqueryContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.subquery_operator"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSubquery_operator([NotNull] ProbSQLParser.Subquery_operatorContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.boolean_literal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBoolean_literal([NotNull] ProbSQLParser.Boolean_literalContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.select_statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSelect_statement([NotNull] ProbSQLParser.Select_statementContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.select_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSelect_expression([NotNull] ProbSQLParser.Select_expressionContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.compare_value_operator"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCompare_value_operator([NotNull] ProbSQLParser.Compare_value_operatorContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.literal_value"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLiteral_value([NotNull] ProbSQLParser.Literal_valueContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.expression_connector"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpression_connector([NotNull] ProbSQLParser.Expression_connectorContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.attribute_name"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAttribute_name([NotNull] ProbSQLParser.Attribute_nameContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.attribute"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAttribute([NotNull] ProbSQLParser.AttributeContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.selection_condition_atom"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSelection_condition_atom([NotNull] ProbSQLParser.Selection_condition_atomContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.bit_literal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBit_literal([NotNull] ProbSQLParser.Bit_literalContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.expression_atom"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpression_atom([NotNull] ProbSQLParser.Expression_atomContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.init"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitInit([NotNull] ProbSQLParser.InitContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.relation_connector"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRelation_connector([NotNull] ProbSQLParser.Relation_connectorContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpression([NotNull] ProbSQLParser.ExpressionContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.join_relation_list"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitJoin_relation_list([NotNull] ProbSQLParser.Join_relation_listContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.join_operator"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitJoin_operator([NotNull] ProbSQLParser.Join_operatorContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.hex_literal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitHex_literal([NotNull] ProbSQLParser.Hex_literalContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.scheme_name"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitScheme_name([NotNull] ProbSQLParser.Scheme_nameContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.string_literal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitString_literal([NotNull] ProbSQLParser.String_literalContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.selection_condition"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSelection_condition([NotNull] ProbSQLParser.Selection_conditionContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.attribute_list"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAttribute_list([NotNull] ProbSQLParser.Attribute_listContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.condition_connector"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCondition_connector([NotNull] ProbSQLParser.Condition_connectorContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.number_literal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNumber_literal([NotNull] ProbSQLParser.Number_literalContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.probabilistic_interval"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitProbabilistic_interval([NotNull] ProbSQLParser.Probabilistic_intervalContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.relation_name"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRelation_name([NotNull] ProbSQLParser.Relation_nameContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.compare_attribute_operator"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCompare_attribute_operator([NotNull] ProbSQLParser.Compare_attribute_operatorContext context);
}
} // namespace PRDB_Visual_Management.ProbSQLCompiler