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

import com.intellij.codeInsight.daemon.RelatedItemLineMarkerInfo;
import com.intellij.codeInsight.daemon.RelatedItemLineMarkerProvider;
import com.intellij.codeInsight.navigation.NavigationGutterIconBuilder;
import com.intellij.openapi.project.Project;
import com.intellij.psi.PsiElement;
import com.intellij.psi.PsiLiteralExpression;
import com.intellij.psi.impl.source.tree.CompositeElement;
import com.intellij.psi.impl.source.tree.LeafPsiElement;
import com.maddyhome.idea.copyright.language.psi.SimpleIdExpr;
import com.maddyhome.idea.copyright.language.psi.SimpleStringExpr;
import com.maddyhome.idea.copyright.language.psi.SimpleTopLevelFunDef;
import org.jetbrains.annotations.NotNull;

import java.util.Collection;
import java.util.List;

/**
 Can't think of any sensible gutter markers for elm so for any string
 containing "fun:xxx" hitch up to a function named xxx
 ****** NOT WORKING ******
 * despite return non-empty result.
 */
public class SimpleLineMarkerProvider extends RelatedItemLineMarkerProvider{
  @Override
  protected void collectNavigationMarkers(@NotNull PsiElement element
    ,Collection<? super RelatedItemLineMarkerInfo> result) {
    if (element instanceof SimpleStringExpr) {
      SimpleStringExpr strExpr = (SimpleStringExpr) element;
      CompositeElement composite = (CompositeElement)strExpr.getNode();
      LeafPsiElement leaf = (LeafPsiElement)composite.getFirstChildNode();
      String value = leaf.getText().replace("\"", "");
      if ( value.startsWith("fun:")) {
        String funName = value.substring("fun:".length());
        Project project = element.getProject();
        final List<SimpleTopLevelFunDef> ids = SimpleUtil.findProperties(project, funName);
        if (ids.size() > 0 ) {
          NavigationGutterIconBuilder<PsiElement> builder
            = NavigationGutterIconBuilder.create(SimpleIcons.FILE)
            .setTarget(ids.get(0)).setTooltipText("Simply navigate...");
          result.add(builder.createLineMarkerInfo(element));
        }
      }
    }
  }

}
