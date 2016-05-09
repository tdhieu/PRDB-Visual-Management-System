grammar ProbSQL;

// starting rule for parsing ProbSQL
init: select_statement; 



// ---------------------------- basic character definition ----------------------------
fragment A_ 		: 'a' | 'A' ;
fragment B_ 		: 'b' | 'B' ;
fragment C_ 		: 'c' | 'C' ;
fragment D_ 		: 'd' | 'D' ;
fragment E_ 		: 'e' | 'E' ;
fragment F_ 		: 'f' | 'F' ;
fragment G_ 		: 'g' | 'G' ;
fragment H_ 		: 'h' | 'H' ;
fragment I_ 		: 'i' | 'I' ;
fragment J_ 		: 'j' | 'J' ;
fragment K_ 		: 'k' | 'K' ;
fragment L_ 		: 'l' | 'L' ;
fragment M_ 		: 'm' | 'M' ;
fragment N_ 		: 'n' | 'N' ;
fragment O_ 		: 'o' | 'O' ;
fragment P_ 		: 'p' | 'P' ;
fragment Q_ 		: 'q' | 'Q' ;
fragment R_ 		: 'r' | 'R' ;
fragment S_ 		: 's' | 'S' ;
fragment T_ 		: 't' | 'T' ;
fragment U_ 		: 'u' | 'U' ;
fragment V_ 		: 'v' | 'V' ;
fragment W_ 		: 'w' | 'W' ;
fragment X_ 		: 'x' | 'X' ;
fragment Y_ 		: 'y' | 'Y' ;
fragment Z_ 		: 'z' | 'Z' ;



// ---------------------------- basic operator definition ----------------------------
EQ					: '=' | '==' ;
NOT_EQ				: '<>' | '!=' ;
GTH					: '>' ;
GET					: '>=' ;
LTH					: '<' ;
LET					: '<=' ;

LTH_IN				: ( L_ T_ H_ '_' I_ N_ ) ;									// Less than (independence)
LTH_IG				: ( L_ T_ H_ '_' I_ G_ ) ;									// Less than (ignorance)
LTH_PC				: ( L_ T_ H_ '_' P_ C_ ) ;									// Less than (positive correlation)
LTH_ME				: ( L_ T_ H_ '_' M_ E_ ) ;									// Less than (mutual exclusion)

LET_IN				: ( L_ E_ T_ '_' I_ N_ ) ;									// Less than or equal to (independence)
LET_IG				: ( L_ E_ T_ '_' I_ G_ ) ;									// Less than or equal to (ignorance)
LET_PC				: ( L_ E_ T_ '_' P_ C_ ) ;									// Less than or equal to (positive correlation)
LET_ME				: ( L_ E_ T_ '_' M_ E_ ) ;									// Less than or equal to (mutual exclusion)

GTH_IN				: ( G_ T_ H_ '_' I_ N_ ) ;									// Greater than (independence)
GTH_IG				: ( G_ T_ H_ '_' I_ G_ ) ;									// Greater than (ignorance)
GTH_PC				: ( G_ T_ H_ '_' P_ C_ ) ;									// Greater than (positive correlation)
GTH_ME				: ( G_ T_ H_ '_' M_ E_ ) ;									// Greater than (mutual exclusion)

GET_IN				: ( G_ E_ T_ '_' I_ N_ ) ;									// Greater than or equal to (independence)
GET_IG				: ( G_ E_ T_ '_' I_ G_ ) ;									// Greater than or equal to (ignorance)
GET_PC				: ( G_ E_ T_ '_' P_ C_ ) ;									// Greater than or equal to (positive correlation)
GET_ME				: ( G_ E_ T_ '_' M_ E_ ) ;									// Greater than or equal to (mutual exclusion)

OR_SYM				: ( O_ R_ ) | '||' ;
AND_SYM				: ( A_ N_ D_ ) | '&&' ;
NOT_SYM				: ( N_ O_ T_ ) | ('!') ;

EQUAL_IN			: ( E_ Q_ U_ A_ L_ '_' I_ N_ ) ;							// Equal (independence)
EQUAL_IG			: ( E_ Q_ U_ A_ L_ '_' I_ G_ ) ;							// Equal (ignorance)
EQUAL_PC			: ( E_ Q_ U_ A_ L_ '_' P_ C_ ) ;							// Equal (positive correlation)
EQUAL_ME			: ( E_ Q_ U_ A_ L_ '_' M_ E_ ) ;							// Equal (mutual exclusion)

UNEQUAL_IN			: ( '!' E_ Q_ U_ A_ L_ '_' I_ N_ ) ;						// Unequal (independence)
UNEQUAL_IG			: ( '!' E_ Q_ U_ A_ L_ '_' I_ G_ ) ;						// Unequal (independence)
UNEQUAL_PC			: ( '!' E_ Q_ U_ A_ L_ '_' P_ C_ ) ;						// Unequal (independence)
UNEQUAL_ME			: ( '!' E_ Q_ U_ A_ L_ '_' M_ E_ ) ;						// Unequal (independence)

CONJ_IN				: ( C_ O_ N_ J_ '_' I_ N_ ) ;								// Conjunction (independence)
CONJ_IG				: ( C_ O_ N_ J_ '_' I_ G_ ) ;								// Conjunction (ignorance)
CONJ_PC				: ( C_ O_ N_ J_ '_' P_ C_ ) ;								// Conjunction (positive correlation)
CONJ_ME				: ( C_ O_ N_ J_ '_' M_ E_ ) ;								// Conjunction (mutual exclusion)

DISJ_IN				: ( D_ I_ S_ J_ '_' I_ N_ ) ;								// Disjunction (independence)
DISJ_IG				: ( D_ I_ S_ J_ '_' I_ G_ ) ;								// Disjunction (ignorance)
DISJ_PC				: ( D_ I_ S_ J_ '_' P_ C_ ) ;								// Disjunction (positive correlation)
DISJ_ME				: ( D_ I_ S_ J_ '_' M_ E_ ) ;								// Disjunction (mutual exclusion)

DIFF_IN				: ( D_ I_ F_ F_ '_' I_ N_ ) ;								// Difference (independence)
DIFF_IG				: ( D_ I_ F_ F_ '_' I_ G_ ) ;								// Difference (ignorance)
DIFF_PC				: ( D_ I_ F_ F_ '_' P_ C_ ) ;								// Difference (positive correlation)
DIFF_ME				: ( D_ I_ F_ F_ '_' M_ E_ ) ;								// Difference (mutual exclusion)

JOIN_IN				: ( J_ O_ I_ N_ '_' I_ N_ ) ;								// Join (independence)
JOIN_IG				: ( J_ O_ I_ N_ '_' I_ G_ ) ;								// Join (ignorance)
JOIN_PC				: ( J_ O_ I_ N_ '_' P_ C_ ) ;								// Join (positive correlation)
JOIN_ME				: ( J_ O_ I_ N_ '_' M_ E_ ) ;								// Join (mutual exclusion)

UNION_IN			: ( U_ N_ I_ O_ N_ '_' I_ N_ ) ;							// Union (independence)
UNION_IG			: ( U_ N_ I_ O_ N_ '_' I_ G_ ) ;							// Union (ignorance)
UNION_PC			: ( U_ N_ I_ O_ N_ '_' P_ C_ ) ;							// Union (positive correlation)
UNION_ME			: ( U_ N_ I_ O_ N_ '_' M_ E_ ) ;							// Union (mutual exclusion)

INTERSECT_IN		: ( I_ N_ T_ E_ R_ S_ E_ C_ T_ '_' I_ N_ ) ;				// INTERSECT (independence)
INTERSECT_IG		: ( I_ N_ T_ E_ R_ S_ E_ C_ T_ '_' I_ G_ ) ;				// INTERSECT (ignorance)
INTERSECT_PC		: ( I_ N_ T_ E_ R_ S_ E_ C_ T_ '_' P_ C_ ) ;				// INTERSECT (positive correlation)
INTERSECT_ME		: ( I_ N_ T_ E_ R_ S_ E_ C_ T_ '_' M_ E_ ) ;				// INTERSECT (mutual exclusion)

MINUS_IN			: ( M_ I_ N_ U_ S_ '_' I_ N_ ) ;							// MINUS (independence)
MINUS_IG			: ( M_ I_ N_ U_ S_ '_' I_ G_ ) ;							// MINUS (ignorance)
MINUS_PC			: ( M_ I_ N_ U_ S_  '_' P_ C_ ) ;							// MINUS (positive correlation)
MINUS_ME			: ( M_ I_ N_ U_ S_  '_' M_ E_ ) ;							// MINUS (mutual exclusion)

IN_IN				: ( I_ N_ '_' I_ N_ ) ;										// In subquery (independence)
IN_IG				: ( I_ N_ '_' I_ G_ ) ;										// In subquery (ignorance)
IN_PC				: ( I_ N_ '_' P_ C_ ) ;										// In subquery (positive correlation)
IN_ME				: ( I_ N_ '_' M_ E_ ) ;										// In subquery (mutual exclusion)

NOT_IN_IN			: ('!' I_ N_ '_' I_ N_ ) ;									// Not_In subquery (independence) 
NOT_IN_IG			: ('!' I_ N_ '_' I_ G_ ) ;									// Not_In subquery (ignorance) 
NOT_IN_PC			: ('!' I_ N_ '_' P_ C_ ) ;									// Not_In subquery (positive correlation) 
NOT_IN_ME			: ('!' I_ N_ '_' M_ E_ ) ;									// Not_In subquery (mutual exclusion) 



// ---------------------------- basic symbol definition ----------------------------
PLUS				: '+' ;
MINUS				: '-' ;
DOT					: '.' ;
COMMA				: ',' ;
ASTERISK			: '*' ;
RPAREN				: ')' ;
LPAREN				: '(' ;
RBRACK				: ']' ;
LBRACK				: '[' ;
WS  				: ('\t' | '\r' | '\n' | ' ')+ -> channel(HIDDEN) ;




// ---------------------------- basic token definition ----------------------------
SELECT				: S_ E_ L_ E_ C_ T_ ;
FROM				: F_ R_ O_ M_ ;
WHERE				: W_ H_ E_ R_ E_ ;

TRUE_SYM			: T_ R_ U_ E_ ;
FALSE_SYM			: F_ A_ L_ S_ E_ ;

INTEGER_NUM		: ('0'..'9')+ ;

fragment HEX_DIGIT_FRAGMENT: ( 'a'..'f' | 'A'..'F' | '0'..'9' ) ;
HEX_DIGIT:
	(  '0x'     (HEX_DIGIT_FRAGMENT)+  )
	|
	(  'X' '\'' (HEX_DIGIT_FRAGMENT)+ '\''  ) 
;

BIT_NUM:
	(  '0b'    ('0'|'1')+  )
	|
	(  B_ '\'' ('0'|'1')+ '\''  ) 
;

REAL_NUMBER:
	(  INTEGER_NUM DOT INTEGER_NUM | INTEGER_NUM DOT | DOT INTEGER_NUM | INTEGER_NUM  )
	(  ('E'|'e') ( PLUS | MINUS )? INTEGER_NUM  )? 
;

TEXT_STRING:
	( N_ | ('_' U_ T_ F_ '8') )?
	(
		(  '\'' ( ('\\' '\\') | ('\'' '\'') | ('\\' '\'') | ~('\'') )* '\''  )
		|
		(  '\"' ( ('\\' '\\') | ('\"' '\"') | ('\\' '\"') | ~('\"') )* '\"'  ) 
	)
;

ID:	
	( 'A'..'Z' | 'a'..'z' | '_' | '$') ( 'A'..'Z' | 'a'..'z' | '_' | '$' | '0'..'9' )*
;



// ---------------------------- operator rule definition ----------------------------
compare_value_operator: 
	EQ | LTH | GTH | NOT_EQ | LET | GET 	
;
	
compare_attribute_operator:
	EQUAL_IN | EQUAL_IG | EQUAL_PC | EQUAL_ME 
	| UNEQUAL_IN | UNEQUAL_IG | UNEQUAL_PC | UNEQUAL_ME
	| LTH_IN | LTH_IG | LTH_PC | LTH_ME
	| LET_IN | LET_IG | LET_PC | LET_ME
	| GTH_IN | GTH_IG | GTH_PC | GTH_ME
	| GET_IN | GET_IG | GET_PC | GET_ME
;

expression_connector:
	CONJ_IN	| CONJ_IG | CONJ_PC | CONJ_ME
	| DISJ_IN | DISJ_IG | DISJ_PC | DISJ_ME
	| DIFF_IN | DIFF_IG | DIFF_PC | DIFF_ME
;

condition_connector:
	AND_SYM | OR_SYM
;

join_operator:
	JOIN_IN | JOIN_IG | JOIN_PC | JOIN_ME
;

relation_connector:
	UNION_IN | UNION_IG | UNION_PC | UNION_ME
	| INTERSECT_IN | INTERSECT_IG | INTERSECT_PC | INTERSECT_ME
	| MINUS_IN | MINUS_IG | MINUS_PC | MINUS_ME
;

subquery_operator:
	IN_IN | IN_IG | IN_PC | IN_ME
	| NOT_IN_IN | NOT_IN_IG | NOT_IN_PC | NOT_IN_ME
;


// ---------------------------- basic constant data (value) definition ----------------------------
string_literal:		TEXT_STRING ;
number_literal:		(PLUS | MINUS)? (INTEGER_NUM | REAL_NUMBER) ;
hex_literal:		HEX_DIGIT ;
boolean_literal:	TRUE_SYM | FALSE_SYM ;
bit_literal:		BIT_NUM ;




// ---------------------------- basic literal value definition ----------------------------
literal_value:
        ( string_literal | number_literal | hex_literal | boolean_literal | bit_literal ) ;

		
		
		
// ---------------------------- function defintion ----------------------------
/*
group_functions:
	AVG | COUNT | MAX_SYM | MIN_SYM | SUM | BIT_AND | BIT_OR | BIT_XOR
;
*/




// ---------------------------- identifiers definition ----------------------------
scheme_name			: ID ;
attribute_name		: ID ;
relation_name		: ID ;




// ---------------------------- SELECT operation ----------------------------
select_statement:
        select_expression ( relation_connector select_expression )* 
;

select_expression:
	( LPAREN ) ?
	SELECT attribute_list
	FROM join_relation_list
	( where_clause )?
	( RPAREN ) ?
;

attribute_list:
	attribute ( COMMA attribute )*
	| scheme_name DOT ASTERISK
	| ASTERISK  
;

attribute:
	attribute_name
	| scheme_name DOT attribute_name
;

// join_relation_list is defined in JOIN query

where_clause:
	WHERE selection_condition
;




// ----------------------------  select condition ----------------------------
// remove left recursion
selection_condition:
	selection_condition_atom ( condition_connector selection_condition_atom )*
;
	
selection_condition_atom:
	( NOT_SYM )? LPAREN expression RPAREN probabilistic_interval
	| subquery_operator LPAREN attribute COMMA subquery RPAREN probabilistic_interval
;

subquery:
	select_statement
;	
	
probabilistic_interval:
	LBRACK REAL_NUMBER COMMA REAL_NUMBER RBRACK
;



	
// ---------------------------- select expression ----------------------------
// remove left recursion

expression:
	expression_atom ( expression_connector expression_atom )*
;


expression_atom:
		attribute compare_value_operator literal_value
	|	literal_value compare_value_operator attribute
	|	attribute compare_attribute_operator attribute
;
	
// ---------------------------- JOIN operation ----------------------------
join_relation_list:
		relation_name ( join_operator relation_name )*
;
