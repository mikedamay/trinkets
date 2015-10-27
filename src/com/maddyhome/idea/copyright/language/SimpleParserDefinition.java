/*
 * Copyright 2000-2015 JetBrains s.r.o.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
package com.maddyhome.idea.copyright.language;

import com.intellij.lang.ASTNode;
import com.intellij.lang.Language;
import com.intellij.lang.ParserDefinition;
import com.intellij.lang.PsiParser;
import com.intellij.lexer.FlexAdapter;
import com.intellij.lexer.Lexer;
import com.intellij.openapi.project.Project;
import com.intellij.psi.FileViewProvider;
import com.intellij.psi.PsiElement;
import com.intellij.psi.PsiFile;
import com.intellij.psi.TokenType;
import com.intellij.psi.tree.IFileElementType;
import com.intellij.psi.tree.TokenSet;
//import com.maddyhome.idea.copyright.language.parser.SimpleLexer;
import com.maddyhome.idea.copyright.language.parser.SimpleParser;
import com.maddyhome.idea.copyright.language.psi.SimpleFile;
import com.maddyhome.idea.copyright.language.psi.SimpleTypes;
import org.jetbrains.annotations.NotNull;

import java.io.Reader;

import static com.maddyhome.idea.copyright.language.psi.SimpleTypes.*;

/**
 * Created by mike on 9/7/15.
 */
public class SimpleParserDefinition implements ParserDefinition {
  public TokenSet WHITE_SPACES = TokenSet.create(TokenType.WHITE_SPACE);
  public TokenSet COMMENTS = TokenSet.create(SimpleTypes.COMMENT);
  public static TokenSet OPERATORS = TokenSet.create(
    LBRACE, RBRACE, LBRACK, RBRACK, COLON, EQ, ASSIGN, NOT_EQ, NOT, PLUS, MINUS, COND_OR, COND_AND
   ,BIT_AND, SEND_CHANNEL, LESS, BIT_XOR, MUL, QUOTIENT, REMAINDER, GREATER
   ,TILDE,PIPE_FORWARD, BACK_TICK, COMBINE, MULTIMAP, PIPE_BACK, GE, LE, COMBINE_BACK
   ,COLON_COLON, INT_DIVIDE, DOT_DOT, DOT, RETURN
  );

  public static final TokenSet KEYWORDS = TokenSet.create(
    ALIAS, AS, CASE, ELSE, EXPOSING, IF, IMPORT, IN, LET, MODULE, OF, OTHERWISE, PORT, THEN, TYPE, WHERE
    );

  public static final IFileElementType FILE
    = new IFileElementType(Language.<SimpleLanguage>findInstance(SimpleLanguage.class));

  @NotNull
  @Override
  public Lexer createLexer(Project project) {
    return new FlexAdapter(new SimpleLexer((Reader)null));
  }

  @Override
  public PsiParser createParser(Project project) {
    return new SimpleParser();
  }

  @Override
  public IFileElementType getFileNodeType() {
    return FILE;
  }

  @NotNull
  @Override
  public TokenSet getWhitespaceTokens() {
    return WHITE_SPACES;
  }

  @NotNull
  @Override
  public TokenSet getCommentTokens() {
    return COMMENTS;
  }

  @NotNull
  @Override
  public TokenSet getStringLiteralElements() {
    return TokenSet.EMPTY;
  }

  @NotNull
  @Override
  public PsiElement createElement(ASTNode node) {
    return SimpleTypes.Factory.createElement(node);
  }

  @Override
  public PsiFile createFile(FileViewProvider viewProvider) {
    return new SimpleFile(viewProvider);
  }

  @Override
  public SpaceRequirements spaceExistanceTypeBetweenTokens(ASTNode left, ASTNode right) {
    return SpaceRequirements.MAY;
  }
}
