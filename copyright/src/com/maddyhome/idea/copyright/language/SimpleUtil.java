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

import com.intellij.openapi.project.Project;
import com.intellij.openapi.vfs.VirtualFile;
import com.intellij.psi.PsiManager;
import com.intellij.psi.impl.source.tree.CompositeElement;
import com.intellij.psi.impl.source.tree.LeafElement;
import com.intellij.psi.impl.source.tree.LeafPsiElement;
import com.intellij.psi.search.FileTypeIndex;
import com.intellij.psi.search.GlobalSearchScope;
import com.intellij.psi.util.PsiTreeUtil;
import com.intellij.psi.util.PsiUtil;
import com.intellij.util.indexing.FileBasedIndex;
import com.maddyhome.idea.copyright.language.psi.SimpleFile;
import com.maddyhome.idea.copyright.language.psi.SimpleIdExpr;
import com.maddyhome.idea.copyright.language.psi.SimpleProperty;
import com.maddyhome.idea.copyright.language.psi.SimpleTopLevelFunDef;
import com.maddyhome.idea.copyright.language.psi.impl.SimplePsiUtil;

import java.util.ArrayList;
import java.util.Collection;
import java.util.Collections;
import java.util.List;

/**
 * Created by mike on 9/19/15.
 */
public class SimpleUtil {
  public static List<SimpleTopLevelFunDef> findProperties(Project project, String key) {
    List<SimpleTopLevelFunDef> result = null;
    Collection<VirtualFile> virtualFiles
      = FileBasedIndex.getInstance().getContainingFiles(FileTypeIndex.NAME
      ,SimpleFileType.INSTANCE, GlobalSearchScope.allScope(project) );
    for (VirtualFile virtualFile : virtualFiles ) {
      SimpleFile simpleFile = (SimpleFile) PsiManager.getInstance(project).findFile(virtualFile);
      Collection<SimpleTopLevelFunDef> simpleProperties = PsiTreeUtil.collectElementsOfType(simpleFile, SimpleTopLevelFunDef.class);
      if (simpleProperties != null) {
        for (SimpleTopLevelFunDef simpleIdExpr : simpleProperties ) {
          CompositeElement composite = (CompositeElement)simpleIdExpr.getNode();
          LeafPsiElement leaf = (LeafPsiElement)composite.getFirstChildNode();
          if (key.equals(leaf.getText())) {
            if (result == null) {
              result = new ArrayList<SimpleTopLevelFunDef>();
            }
            result.add(simpleIdExpr);
          }
        }
      }
    }
    return result != null ? result : Collections.<SimpleTopLevelFunDef>emptyList();
  }

  public static List<SimpleProperty> findProperties(Project project ) {
    List<SimpleProperty> result = new ArrayList<SimpleProperty>();
    Collection<VirtualFile> virtualFiles
      = FileBasedIndex.getInstance().getContainingFiles(FileTypeIndex.NAME
      ,SimpleFileType.INSTANCE, GlobalSearchScope.allScope(project) );
    for (VirtualFile virtualFile : virtualFiles ) {
      SimpleFile simpleFile = (SimpleFile)PsiManager.getInstance(project).findFile(virtualFile);
      if (simpleFile != null ) {
        SimpleProperty[] simpleProperties = PsiTreeUtil.getChildrenOfType(simpleFile, SimpleProperty.class);
        if (simpleProperties != null) {
          Collections.addAll(result, simpleProperties);
        }
      }
    }
    return result;
  }
}
