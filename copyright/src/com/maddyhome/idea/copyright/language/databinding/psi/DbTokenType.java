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
package com.maddyhome.idea.copyright.language.databinding.psi;

import com.intellij.psi.tree.IElementType;
import com.maddyhome.idea.copyright.language.SimpleLanguage;
import org.jetbrains.annotations.NonNls;
import org.jetbrains.annotations.NotNull;


/**
 * Created by mike on 9/6/15.
 */
public class DbTokenType extends IElementType {
  public DbTokenType(@NotNull @NonNls String debugName) {
    super(debugName, SimpleLanguage.INSTANCE);
  }

  @Override
  public String toString() {
    return "DbTokenType." + super.toString();
  }
}
