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
import com.intellij.openapi.editor.SyntaxHighlighterColors;
import com.intellij.openapi.editor.colors.TextAttributesKey;
import com.intellij.openapi.editor.markup.TextAttributes;
import com.intellij.openapi.fileTypes.SyntaxHighlighterBase;
import com.intellij.psi.TokenType;
import com.intellij.psi.tree.IElementType;
import com.maddyhome.idea.copyright.language.psi.SimpleTypes;
import org.jetbrains.annotations.NotNull;

/*
mike
 */

import java.awt.*;
import java.io.Reader;

import static com.intellij.openapi.editor.colors.TextAttributesKey.createTextAttributesKey;

/**
 * Created by mike on 9/8/15.
 */
public class SimpleSyntaxHighlighter extends SyntaxHighlighterBase {
  public static final TextAttributesKey SEPARATOR
    = createTextAttributesKey("SIMPLE_SEPARATOR", SyntaxHighlighterColors.OPERATION_SIGN);
  public static final TextAttributesKey KEY
    = createTextAttributesKey("SIMPLE_KEY", SyntaxHighlighterColors.KEYWORD);
  public static final TextAttributesKey VALUE
    = createTextAttributesKey("SIMPLE_VALUE", SyntaxHighlighterColors.STRING);
  public static final TextAttributesKey COMMENT
    = createTextAttributesKey("SIMPLE_COMMENT", SyntaxHighlighterColors.LINE_COMMENT);
  public static final TextAttributesKey MULTILINE_COMMENT
    = createTextAttributesKey("SIMPLE_MULTILINE_COMMENT", SyntaxHighlighterColors.LINE_COMMENT);
  public static final TextAttributesKey STR
    = createTextAttributesKey("SIMPLE_STR", SyntaxHighlighterColors.STRING);
  public static final TextAttributesKey BAD_CHARACTER = createTextAttributesKey("SIMPLE_BAD_CHARACTER"
    ,new TextAttributes(Color.RED, null, null, null, Font.BOLD));
  public static final TextAttributesKey KEYWORD
    = createTextAttributesKey("KEYWORD", SyntaxHighlighterColors.COMMA);

  private static final TextAttributesKey[] BAD_CHAR_KEYS = new TextAttributesKey[]{BAD_CHARACTER};
  private static final TextAttributesKey[] SEPARATOR_KEYS = new TextAttributesKey[]{SEPARATOR};
  private static final TextAttributesKey[] KEY_KEYS = new TextAttributesKey[]{KEY};
  private static final TextAttributesKey[] VALUE_KEYS = new TextAttributesKey[]{VALUE};
  private static final TextAttributesKey[] COMMENT_KEYS = new TextAttributesKey[]{COMMENT};
  private static final TextAttributesKey[] ML_COMMENT_KEYS = new TextAttributesKey[]{MULTILINE_COMMENT};
  private static final TextAttributesKey[] STR_KEYS = new TextAttributesKey[]{STR};
  private static final TextAttributesKey[] KEYWORDS = new TextAttributesKey[]{KEYWORD};
  private static final TextAttributesKey[] EMPTY_KEYS = new TextAttributesKey[0];

  @NotNull
  @Override
  public Lexer getHighlightingLexer() {
    return new FlexAdapter(new SimpleLexer((Reader) null));
  }

  @NotNull
  @Override
  public TextAttributesKey[] getTokenHighlights(IElementType tokenType) {
    if (tokenType.equals(SimpleTypes.SEPARATOR)) {
      return SEPARATOR_KEYS;
    } else if (tokenType.equals(SimpleTypes.KEY)) {
      return KEY_KEYS;
    } else if (tokenType.equals(SimpleTypes.VALUE)) {
      return VALUE_KEYS;
    } else if (tokenType.equals(SimpleTypes.COMMENT)) {
      return COMMENT_KEYS;
    } else if (tokenType.equals(SimpleTypes.MULTILINE_COMMENT)) {
      return ML_COMMENT_KEYS;
    } else if (tokenType.equals(SimpleTypes.STR)) {
      return STR_KEYS;
    } else if (tokenType.equals(SimpleTypes.IMPORT)) {
      return KEYWORDS;
    } else if (tokenType.equals(TokenType.BAD_CHARACTER)){
      return BAD_CHAR_KEYS;
    } else{
      return EMPTY_KEYS;
    }
  }
}
