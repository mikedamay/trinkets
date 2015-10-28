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

import com.intellij.openapi.editor.colors.TextAttributesKey;
import com.intellij.openapi.fileTypes.SyntaxHighlighter;
import com.intellij.openapi.options.colors.AttributesDescriptor;
import com.intellij.openapi.options.colors.ColorDescriptor;
import com.intellij.openapi.options.colors.ColorSettingsPage;
import org.jetbrains.annotations.NotNull;
import org.jetbrains.annotations.Nullable;

import javax.swing.*;
import java.util.Map;


/**
 * Created by mike on 9/8/15.
 */
public class SimpleColorSettingsPage implements ColorSettingsPage {
  public static final AttributesDescriptor[] DESCRIPTORS = new AttributesDescriptor[] {
    new AttributesDescriptor("Identifier", SimpleSyntaxHighlightingColors.VALUE)
    ,new AttributesDescriptor("Type Identifier", SimpleSyntaxHighlightingColors.TYPE_IDENTIFIER)
    ,new AttributesDescriptor("String", SimpleSyntaxHighlightingColors.STR)
    ,new AttributesDescriptor("Parenthesis", SimpleSyntaxHighlightingColors.PARENTHESIS)
    ,new AttributesDescriptor("Keyword", SimpleSyntaxHighlightingColors.KEYWORD)
    ,new AttributesDescriptor("Comment", SimpleSyntaxHighlightingColors.COMMENT)
    ,new AttributesDescriptor("Operator", SimpleSyntaxHighlightingColors.OPERATOR)
    ,new AttributesDescriptor("Number", SimpleSyntaxHighlightingColors.NUM)
  };


  @Nullable
  @Override
  public Icon getIcon() {
    return SimpleIcons.FILE;
  }

  @NotNull
  @Override
  public SyntaxHighlighter getHighlighter() {
    return new SimpleSyntaxHighlighter();
  }

  @NotNull
  @Override
  public String getDemoText() {
    return "# some comment \nkkk=vvv\n";
  }

  @Nullable
  @Override
  public Map<String, TextAttributesKey> getAdditionalHighlightingTagToDescriptorMap() {
    return null;
  }

  @NotNull
  @Override
  public AttributesDescriptor[] getAttributeDescriptors() {
    return DESCRIPTORS;
  }

  @NotNull
  @Override
  public ColorDescriptor[] getColorDescriptors() {
    return ColorDescriptor.EMPTY_ARRAY;
  }

  @NotNull
  @Override
  public String getDisplayName() {
    return "Simple";
  }
}
