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
CRLF=\n|\r|\r\n
WHITE_SPACE=[\ \t\f]
FIRST_VALUE_CHARACTER=[A-Za-z_]
VALUE_CHARACTER=[A-Za-z0-9_]
END_OF_LINE_COMMENT=("--")[^\r\n]*
SEPARATOR=[:=]
KEY_CHARACTER=[^:=\ \n\r\t\f\\] | "\\"{CRLF}
DQ="\""
STRLINE={DQ} ( [^\"\\\n\r] | "\\" ("\\" | {DQ} | {ESCAPES} | [0-8xuU] ) )* {DQ}?
MULTILINE_COMMENT="{-" ( ([^"-"]|[\r\n])* ("-"+ [^"-""}"] )? )* ("-" | "-"+"}")?

%state WAITING_VALUE

%%

<YYINITIAL> {MULTILINE_COMMENT} { yybegin(YYINITIAL); return SimpleTypes.MULTILINE_COMMENT; }

<YYINITIAL> {END_OF_LINE_COMMENT} { yybegin(YYINITIAL); return SimpleTypes.COMMENT; }

<YYINITIAL> {STRLINE} { yybegin(YYINITIAL); return SimpleTypes.STR; }

<YYINITIAL> {
  "(" {yybegin(YYINITIAL); return SimpleTypes.L_PAREN; }
  ")" {yybegin(YYINITIAL); return SimpleTypes.R_PAREN; }
  "," {yybegin(YYINITIAL); return SimpleTypes.COMMA; }
"{"                                       { return SimpleTypes.LBRACE; }
"}"                                       { return SimpleTypes.RBRACE; }

"["                                       { return SimpleTypes.LBRACK; }
"]"                                       { return SimpleTypes.RBRACK; }

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
"port"                                    {  return SimpleTypes.PORT;  }
"then"                                    {  return SimpleTypes.THEN;  }
"type"                                    {  return SimpleTypes.TYPE;  }
"where"                                   {  return SimpleTypes.WHERE; }

  {FIRST_VALUE_CHARACTER}{VALUE_CHARACTER}* { yybegin(YYINITIAL); return SimpleTypes.VALUE; }
}

//<YYINITIAL> {KEY_CHARACTER}+ { yybegin(YYINITIAL); return SimpleTypes.KEY; }

//<YYINITIAL> {SEPARATOR} { yybegin(WAITING_VALUE); return SimpleTypes.SEPARATOR; }

//<WAITING_VALUE> {CRLF} { yybegin(YYINITIAL); return SimpleTypes.CRLF; }

//<WAITING_VALUE> {WHITE_SPACE}+  { yybegin(WAITING_VALUE); return TokenType.WHITE_SPACE; }


{CRLF} { yybegin(YYINITIAL); return SimpleTypes.CRLF; }

{WHITE_SPACE}+  {yybegin(YYINITIAL); return TokenType.WHITE_SPACE; }

. {return TokenType.BAD_CHARACTER;}





