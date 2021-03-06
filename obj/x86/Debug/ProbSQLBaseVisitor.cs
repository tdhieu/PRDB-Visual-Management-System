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
using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;

/// <summary>
/// This class provides an empty implementation of <see cref="IProbSQLVisitor{Result}"/>,
/// which can be extended to create a visitor which only needs to handle a subset
/// of the available methods.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.2.2-SNAPSHOT")]
[System.CLSCompliant(false)]
public partial class ProbSQLBaseVisitor<Result> : AbstractParseTreeVisitor<Result>, IProbSQLVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.where_clause"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitWhere_clause([NotNull] ProbSQLParser.Where_clauseContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.subquery"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitSubquery([NotNull] ProbSQLParser.SubqueryContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.subquery_operator"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitSubquery_operator([NotNull] ProbSQLParser.Subquery_operatorContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.boolean_literal"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitBoolean_literal([NotNull] ProbSQLParser.Boolean_literalContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.select_statement"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitSelect_statement([NotNull] ProbSQLParser.Select_statementContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.select_expression"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitSelect_expression([NotNull] ProbSQLParser.Select_expressionContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.compare_value_operator"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitCompare_value_operator([NotNull] ProbSQLParser.Compare_value_operatorContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.literal_value"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitLiteral_value([NotNull] ProbSQLParser.Literal_valueContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.expression_connector"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitExpression_connector([NotNull] ProbSQLParser.Expression_connectorContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.attribute_name"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitAttribute_name([NotNull] ProbSQLParser.Attribute_nameContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.attribute"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitAttribute([NotNull] ProbSQLParser.AttributeContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.selection_condition_atom"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitSelection_condition_atom([NotNull] ProbSQLParser.Selection_condition_atomContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.bit_literal"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitBit_literal([NotNull] ProbSQLParser.Bit_literalContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.expression_atom"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitExpression_atom([NotNull] ProbSQLParser.Expression_atomContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.init"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitInit([NotNull] ProbSQLParser.InitContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.relation_connector"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitRelation_connector([NotNull] ProbSQLParser.Relation_connectorContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.expression"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitExpression([NotNull] ProbSQLParser.ExpressionContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.join_relation_list"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitJoin_relation_list([NotNull] ProbSQLParser.Join_relation_listContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.join_operator"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitJoin_operator([NotNull] ProbSQLParser.Join_operatorContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.hex_literal"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitHex_literal([NotNull] ProbSQLParser.Hex_literalContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.scheme_name"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitScheme_name([NotNull] ProbSQLParser.Scheme_nameContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.string_literal"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitString_literal([NotNull] ProbSQLParser.String_literalContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.selection_condition"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitSelection_condition([NotNull] ProbSQLParser.Selection_conditionContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.attribute_list"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitAttribute_list([NotNull] ProbSQLParser.Attribute_listContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.condition_connector"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitCondition_connector([NotNull] ProbSQLParser.Condition_connectorContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.number_literal"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitNumber_literal([NotNull] ProbSQLParser.Number_literalContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.probabilistic_interval"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitProbabilistic_interval([NotNull] ProbSQLParser.Probabilistic_intervalContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.relation_name"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitRelation_name([NotNull] ProbSQLParser.Relation_nameContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="ProbSQLParser.compare_attribute_operator"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitCompare_attribute_operator([NotNull] ProbSQLParser.Compare_attribute_operatorContext context) { return VisitChildren(context); }
}
} // namespace PRDB_Visual_Management.ProbSQLCompiler
