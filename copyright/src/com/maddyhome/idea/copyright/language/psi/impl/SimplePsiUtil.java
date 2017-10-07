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
package com.maddyhome.idea.copyright.language.psi.impl;

import com.intellij.lang.ASTNode;
import com.intellij.psi.PsiElement;
import com.maddyhome.idea.copyright.language.psi.SimpleElementFactory;
import com.maddyhome.idea.copyright.language.psi.SimpleProperty;
import com.maddyhome.idea.copyright.language.psi.SimpleTypes;

/**
 * Created by mike on 9/19/15.
 */
public class SimplePsiUtil {

  public static String getName(SimpleProperty element) {
    return getKey(element);
  }

  public static PsiElement setName(SimpleProperty element, String newName ) {
    ASTNode keyNode = element.getNode().findChildByType(SimpleTypes.CASE);
    if (keyNode != null ) {
      SimpleProperty property = SimpleElementFactory.createProperty(element.getProject(), newName);
      ASTNode newKeyNode = property.getFirstChild().getNode();
      element.getNode().replaceChild(keyNode, newKeyNode);
    }
    return element;
  }

  public static PsiElement getNameIdentifier(SimpleProperty element) {
    ASTNode keyNode = element.getNode().findChildByType(SimpleTypes.CASE);
    if (keyNode != null) {
      return keyNode.getPsi();
    }
    else {
      return null;
    }
  }

  public static String getKey(SimpleProperty element) {
    ASTNode keyNode = element.getNode().findChildByType(SimpleTypes.CASE);
    if (keyNode != null ) {
      return keyNode.getText();
    } else {
      return null;
    }
  }
  public static String getValue(SimpleProperty element) {
    ASTNode keyNode = element.getNode().findChildByType(SimpleTypes.OF);
    if (keyNode != null ) {
      return keyNode.getText();
    } else {
      return null;
    }
  }
}
