package com.maddyhome.idea.copyright.language;

import com.intellij.lexer.FlexLexer;
import com.intellij.psi.tree.IElementType;
import com.maddyhome.idea.copyright.language.psi.SimpleTypes;
import com.intellij.psi.TokenType;

%%

%{
  StringBuffer sb = new StringBuffer();
%}

%class SimpleLexer
%implements FlexLexer
%unicode
%function advance
%type IElementType
%eof{ return;
%eof}

ESCAPES = [abfnrtv]
WHITE_SPACE=[\ \t\f\r\n]
NUMBER=-?(0|[1-9][0-9]*)(\.[0-9]+)?([eE][+-]?[0-9]*)?
FIRST_VALUE_CHARACTER=[a-z_]
VALUE_CHARACTER=[A-Za-z0-9_']
TYPE_IDENTIFIER_FIRST_CHAR=[A-Z]
TYPE_IDENTIFIER_CHAR=[A-Za-z0-9_'.]
END_OF_LINE_COMMENT=("--")[^\r\n]*
DQ="\""
STRLINE={DQ} ( [^\"\\\n\r] | "\\" ("\\" | {DQ} | {ESCAPES} | [0-8xuU] ) )* {DQ}?
MULTILINE_COMMENT="{-" ( ([^"-"]|[\r\n])* ("-"+ [^"-""}"] )? )* ("-" | "-"+"}")?


%state WAITING_VALUE

%%

<YYINITIAL> {MULTILINE_COMMENT} { yybegin(YYINITIAL); return SimpleTypes.MULTILINE_COMMENT; }

<YYINITIAL> {END_OF_LINE_COMMENT} { yybegin(YYINITIAL); return SimpleTypes.COMMENT; }

<YYINITIAL> {STRLINE} { yybegin(YYINITIAL); return SimpleTypes.STR; }
<YYINITIAL> {NUMBER} { yybegin(YYINITIAL); return SimpleTypes.NUM; }


<YYINITIAL> {
  "List" {yybegin(YYINITIAL); return SimpleTypes.LIST_TYPENAME; }
  "(" {yybegin(YYINITIAL); return SimpleTypes.L_PAREN; }
  ")" {yybegin(YYINITIAL); return SimpleTypes.R_PAREN; }
  "," {yybegin(YYINITIAL); return SimpleTypes.COMMA; }
"{"                                       { return SimpleTypes.LBRACE; }
"}"                                       { return SimpleTypes.RBRACE; }

"["                                       { return SimpleTypes.LBRACK; }
"]"                                       { return SimpleTypes.RBRACK; }

"|"                                      { return SimpleTypes.BIT_OR; }

":"                                       { return SimpleTypes.COLON; }

"=="                                      { return SimpleTypes.EQ; }
"="                                       { return SimpleTypes.ASSIGN; }

"!="                                      { return SimpleTypes.NOT_EQ; }
"!"                                       { return SimpleTypes.NOT; }

"+"                                       { return SimpleTypes.PLUS; }

"-"                                       { return SimpleTypes.MINUS; }

"||"                                      { return SimpleTypes.COND_OR; }

"&&"                                      { return SimpleTypes.COND_AND; }

"&"                                       { return SimpleTypes.BIT_AND; }

"<|"                                      { return SimpleTypes.PIPE_BACK; }
"<="                                      { return SimpleTypes.LE; }
"<<"                                      { return SimpleTypes.COMBINE_BACK; }
"<~"                                      { return SimpleTypes.MULTIMAP; }
"<-"                                      { return SimpleTypes.SEND_CHANNEL; }
"<"                                       { return SimpleTypes.LESS; }

"^"                                       { return SimpleTypes.BIT_XOR; }

"*"                                       { return SimpleTypes.MUL; }

"//"                                      { return SimpleTypes.INT_DIVIDE; }
"/"                                       { return SimpleTypes.QUOTIENT; }

"%"                                       { return SimpleTypes.REMAINDER; }

">="                                      { return SimpleTypes.GE; }
"->"                                      { return SimpleTypes.RETURN; }
">>"                                      { return SimpleTypes.COMBINE; }
"|>"                                      { return SimpleTypes.PIPE_FORWARD; }
">"                                       { return SimpleTypes.GREATER; }

"~"                                      { return SimpleTypes.TILDE; }
"`"                                      { return SimpleTypes.BACK_TICK; }
"::"                                      { return SimpleTypes.COLON_COLON; }
".."                                      { return SimpleTypes.DOT_DOT; }
"."                                      { return SimpleTypes.DOT; }


"alias"                                   {  return SimpleTypes.ALIAS;  }
"as"                                      {  return SimpleTypes.AS;  }
"case"                                    {  return SimpleTypes.CASE;  }
"else"                                    {  return SimpleTypes.ELSE;  }
"exposing"                                {  return SimpleTypes.EXPOSING;  }
"in"                                      {  return SimpleTypes.IN;  }
"if"                                      {  return SimpleTypes.IF ;  }
"import"                                  {  return SimpleTypes.IMPORT ;  }
"let"                                     {  return SimpleTypes.LET;  }
"module"                                  {  return SimpleTypes.MODULE ;  }
"of"                                      {  return SimpleTypes.OF;  }
"otherwise"                               {  return SimpleTypes.OTHERWISE;  }
"port"                                    {  return SimpleTypes.PORT;  }
"then"                                    {  return SimpleTypes.THEN;  }
"type"                                    {  return SimpleTypes.TYPE;  }
"where"                                   {  return SimpleTypes.WHERE; }

  ^{FIRST_VALUE_CHARACTER}{VALUE_CHARACTER}* { yybegin(YYINITIAL); return SimpleTypes.FUN_NAME; }
  {FIRST_VALUE_CHARACTER}{VALUE_CHARACTER}* { yybegin(YYINITIAL); return SimpleTypes.IDENTIFIER; }
  {TYPE_IDENTIFIER_FIRST_CHAR}{TYPE_IDENTIFIER_CHAR}* { yybegin(YYINITIAL); return SimpleTypes.TYPE_IDENTIFIER; }
}

{WHITE_SPACE}+  {yybegin(YYINITIAL); return TokenType.WHITE_SPACE; }

. {return TokenType.BAD_CHARACTER;}





