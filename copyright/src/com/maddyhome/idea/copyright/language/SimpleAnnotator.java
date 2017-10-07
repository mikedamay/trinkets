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

import com.intellij.lang.annotation.Annotation;
import com.intellij.lang.annotation.AnnotationHolder;
import com.intellij.lang.annotation.Annotator;
import com.intellij.openapi.util.TextRange;
import com.intellij.psi.PsiElement;
import com.maddyhome.idea.copyright.language.psi.SimpleImportStatement;
import org.jetbrains.annotations.NotNull;

/**
 * Created by mike on 9/20/15.
 */
public class SimpleAnnotator implements Annotator {

  @Override
  public void annotate(@NotNull PsiElement element, @NotNull AnnotationHolder holder) {
     if (element instanceof SimpleImportStatement) {
       TextRange range = element.getTextRange();
       Annotation annotation = holder.createErrorAnnotation(range, "interesting...");
       //annotation.setTextAttributes(SimpleSyntaxHighlightingColors.KEYWORD);
     }

  }
}
