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

import com.intellij.lexer.FlexAdapter;
import com.intellij.lexer.Lexer;
import com.intellij.openapi.editor.DefaultLanguageHighlighterColors;
import com.intellij.openapi.editor.colors.TextAttributesKey;
import com.intellij.openapi.fileTypes.SyntaxHighlighterBase;
import com.intellij.psi.TokenType;
import com.intellij.psi.tree.IElementType;
import com.maddyhome.idea.copyright.language.psi.SimpleTypes;
import org.jetbrains.annotations.NotNull;

import java.io.Reader;
import java.util.HashMap;
import java.util.Map;

import static com.intellij.openapi.editor.colors.TextAttributesKey.createTextAttributesKey;
import static com.maddyhome.idea.copyright.language.SimpleSyntaxHighlightingColors.*;

/**
 * Created by mike on 9/8/15.
 */
public class SimpleSyntaxHighlighter extends SyntaxHighlighterBase {
  public static final TextAttributesKey SEPARATOR
    = createTextAttributesKey("SIMPLE_SEPARATOR", DefaultLanguageHighlighterColors.OPERATION_SIGN);

  private static final TextAttributesKey[] EMPTY_KEYS = new TextAttributesKey[0];

  private static final Map<IElementType, TextAttributesKey> ATTRIBUTES = new HashMap<IElementType, TextAttributesKey>();

  static {
    fillMap(ATTRIBUTES, PARENTHESIS, SimpleTypes.L_PAREN, SimpleTypes.R_PAREN);
    //fillMap(ATTRIBUTES, R_PAREN, SimpleTypes.R_PAREN);
    fillMap(ATTRIBUTES, VALUE, SimpleTypes.IDENTIFIER);
    fillMap(ATTRIBUTES, TYPE_IDENTIFIER, SimpleTypes.TYPE_IDENTIFIER);
    fillMap(ATTRIBUTES, COMMENT, SimpleTypes.COMMENT);
    fillMap(ATTRIBUTES, MULTILINE_COMMENT, SimpleTypes.MULTILINE_COMMENT);
    fillMap(ATTRIBUTES, STR, SimpleTypes.STR);
    fillMap(ATTRIBUTES, NUM, SimpleTypes.NUM);
    fillMap(ATTRIBUTES, SimpleParserDefinition.KEYWORDS, KEYWORD);
    fillMap(ATTRIBUTES, SimpleParserDefinition.OPERATORS, OPERATOR);
    fillMap(ATTRIBUTES, BAD_CHARACTER, TokenType.BAD_CHARACTER);
  }

  @NotNull
  @Override
  public Lexer getHighlightingLexer() {
    return new FlexAdapter(new SimpleLexer((Reader) null));
  }

  @NotNull
  @Override
  public TextAttributesKey[] getTokenHighlights(IElementType tokenType) {
    return pack(ATTRIBUTES.get(tokenType));
  }
}
