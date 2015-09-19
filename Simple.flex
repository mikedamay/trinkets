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
  "import"  {yybegin(YYINITIAL); return SimpleTypes.IMPORT; }
  "where"   {yybegin(YYINITIAL); return SimpleTypes.WHERE; }
  "(" {yybegin(YYINITIAL); return SimpleTypes.L_PAREN; }
  ")" {yybegin(YYINITIAL); return SimpleTypes.R_PAREN; }
  "," {yybegin(YYINITIAL); return SimpleTypes.COMMA; }
  {FIRST_VALUE_CHARACTER}{VALUE_CHARACTER}* { yybegin(YYINITIAL); return SimpleTypes.VALUE; }
}

//<YYINITIAL> {KEY_CHARACTER}+ { yybegin(YYINITIAL); return SimpleTypes.KEY; }

//<YYINITIAL> {SEPARATOR} { yybegin(WAITING_VALUE); return SimpleTypes.SEPARATOR; }

//<WAITING_VALUE> {CRLF} { yybegin(YYINITIAL); return SimpleTypes.CRLF; }

//<WAITING_VALUE> {WHITE_SPACE}+  { yybegin(WAITING_VALUE); return TokenType.WHITE_SPACE; }


{CRLF} { yybegin(YYINITIAL); return SimpleTypes.CRLF; }

{WHITE_SPACE}+  {yybegin(YYINITIAL); return TokenType.WHITE_SPACE; }

. {return TokenType.BAD_CHARACTER;}





