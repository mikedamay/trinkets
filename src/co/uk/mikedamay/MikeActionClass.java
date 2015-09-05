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
package co.uk.mikedamay;

import com.intellij.openapi.actionSystem.AnAction;
import com.intellij.openapi.actionSystem.AnActionEvent;
import com.intellij.openapi.actionSystem.PlatformDataKeys;
import com.intellij.openapi.project.Project;
import com.intellij.openapi.ui.Messages;

/**
 * Created by mike on 8/26/15.
 */
public class MikeActionClass extends AnAction {

  // If you register the action from Java code, this constructor is used to set the menu item name
  // (optionally, you can specify the menu description and an icon to display next to the menu item).
  // You can omit this constructor when registering the action in the plugin.xml file.
  public MikeActionClass() {
    // Set the menu item name.
    super("Text _Boxes");
    // Set the menu item name, description and icon.
    // super("Text _Boxes","Item description",IconLoader.getIcon("/Mypackage/icon.png"));
  }

  public void actionPerformed(AnActionEvent event) {
    Project project = event.getData(PlatformDataKeys.PROJECT);
    String txt= Messages.showInputDialog(project, "What is your name?", "Input your name", Messages.getQuestionIcon());
    Messages.showMessageDialog(project, "Hello, " + txt + "!\n I am glad to see you.", "Information", Messages.getInformationIcon());
  }

}
