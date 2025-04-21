grammar MathGrammar;

start: expression EOF;

expression:
  | NUMBER
  | variable
  | expression '*' expression
  | expression '/' expression
  | expression '+' expression
  | expression '-' expression
  | '(' expression ')'
  | expression 'of' expression;

variable:
  | 'Basic Salary'
  | 'Base Hourly Rate'
  | 'Number Of Days Per Month'
  | 'Number Of Hours Per Day'
  | 'Percentage'
  | 'Percent'
  | 'Fixed Amount'
  | 'Fixed Rate'
  | 'Rate Per Kilometer'
  | 'Rate Per Litre'
  | 'Number Of Kilometers'
  | 'Number Of Litres'
  | 'Number Of Months Served'
  | 'Number Of Years Served'
  | 'Number Of Leave Days Accrued'
  | 'Number Of Leave Days Commuted'
  | 'Gross Pay'
  | 'Total Hours Worked'
  | 'Total Sales'
  | 'Multiplier';

NUMBER: [0-9]+;
WS: [ \t\r\n]+ -> skip;
