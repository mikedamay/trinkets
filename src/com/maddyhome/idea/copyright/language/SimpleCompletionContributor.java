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

import com.intellij.codeInsight.completion.*;
import com.intellij.codeInsight.lookup.LookupElementBuilder;
import com.intellij.patterns.PlatformPatterns;
import com.intellij.util.ProcessingContext;
import com.maddyhome.idea.copyright.language.psi.SimpleTypes;
import org.jetbrains.annotations.NotNull;

/**
 ****** NOT WORKING ******
 * maybe do something with IDENTIFIER
 */
public class SimpleCompletionContributor extends CompletionContributor{
  public SimpleCompletionContributor() {
    extend(CompletionType.BASIC, PlatformPatterns.psiElement(SimpleTypes.IDENTIFIER)
      .withLanguage(SimpleLanguage.INSTANCE), new CompletionProvider<CompletionParameters>() {
        public void addCompletions(@NotNull CompletionParameters parameters
          ,ProcessingContext context, @NotNull CompletionResultSet resultSet) {
            resultSet.addElement(LookupElementBuilder.create("hello"));
        }

      });
  }
}
