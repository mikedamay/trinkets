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
package com.maddyhome.idea.copyright.language.psi;

import com.intellij.openapi.project.Project;
import com.intellij.psi.PsiFileFactory;
import com.maddyhome.idea.copyright.language.SimpleFileType;

import java.io.File;

/**
 * Created by mike on 31/10/15.
 */
public class SimpleElementFactory {
  public static SimpleProperty createProperty(Project project, String name ) {
    final SimpleFile file = createFile(project, name);
    return (SimpleProperty)file.getFirstChild();
  }
  public static SimpleFile createFile(Project project, String text) {
    String name = "dummy.simpole";
    return (SimpleFile)PsiFileFactory.getInstance(project)
      .createFileFromText(name, SimpleFileType.INSTANCE, text);
  }
}
