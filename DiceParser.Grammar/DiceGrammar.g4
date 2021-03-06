grammar DiceGrammar;

entry  : definition | definition label;
definition : roll | roll comparison roll;
roll : die | constant | roll operation roll;
die : DIE | NUMBER DIE;
constant : NUMBER;
comparison : LT | LE | EQ | GE | GT;
operation : PLUS | MINUS;
label : LABEL;
DIE : [0-9]*'d'[0-9]+;
NUMBER : [0-9]+;
LT : '<';
LE : '<=';
EQ : '=';
GE : '>=';
GT : '>';
PLUS : '+';
MINUS : '-';
WS : [ \t\r\n]+ -> skip;
LABEL : [A-Za-z ]+;