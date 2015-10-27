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

import com.intellij.openapi.editor.DefaultLanguageHighlighterColors;
import com.intellij.openapi.editor.HighlighterColors;
import com.intellij.openapi.editor.colors.TextAttributesKey;

import static com.intellij.openapi.editor.colors.TextAttributesKey.createTextAttributesKey;

/**
 * Created by mike on 9/19/15.
 */
public class SimpleSyntaxHighlightingColors {
  public static final TextAttributesKey VALUE
    = createTextAttributesKey("SIMPLE_VALUE", DefaultLanguageHighlighterColors.STRING);
  public static final TextAttributesKey COMMENT
    = createTextAttributesKey("SIMPLE_COMMENT", DefaultLanguageHighlighterColors.LINE_COMMENT);
  public static final TextAttributesKey MULTILINE_COMMENT
    = createTextAttributesKey("SIMPLE_MULTILINE_COMMENT", DefaultLanguageHighlighterColors.LINE_COMMENT);
  public static final TextAttributesKey STR
    = createTextAttributesKey("SIMPLE_STR", DefaultLanguageHighlighterColors.STRING);
  public static final TextAttributesKey NUM
    = createTextAttributesKey("SIMPLE_NUM", DefaultLanguageHighlighterColors.NUMBER);
  public static final TextAttributesKey PARENTHESIS
    = createTextAttributesKey("SIMPLE_PARENTHESIS", DefaultLanguageHighlighterColors.STRING);
  public static final TextAttributesKey BAD_CHARACTER = HighlighterColors.BAD_CHARACTER;
  public static final TextAttributesKey KEYWORD
    = createTextAttributesKey("SIMPLE_KEYWORD", DefaultLanguageHighlighterColors.KEYWORD);
  public static final TextAttributesKey OPERATOR
    = createTextAttributesKey("SIMPLE_OPERATOR", DefaultLanguageHighlighterColors.OPERATION_SIGN);
  private SimpleSyntaxHighlightingColors() {
  }
}
