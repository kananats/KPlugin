KPlugin
===
KPlugin is Unity Plugin by @kdev
Tailored for richer debugging purpose
Core features consists of Serialize Method, Console, Extension

See introduction slide here:
https://github.com/kananats/KPlugin/blob/master/Kplugin.pptx (EN)
https://github.com/kananats/KPlugin/blob/master/Kplugin%20JP.pptx (JP)

Status: active

Prerequisite
---
KPlugin is developed on Unity 2018.1.0f2

Getting Started
---
Import KPlugin folder into Assets/Script

Use following directive for corresponding features

Serialize Method
(namespace: KPlugin.Editor)
Any method (without non optianal parameter/ ref/ out/ params keyword) can be serialized as button to invoke as compile time and runtime

Console
(namespace: KPlugin.Debug)
Console allows interactive UI which can tweak any attribute/ property/ method at runtime.

Extension
(namespace: KPlugin.Extension)
There are plenty of useful extension methods which makes debug handy.
Object.ToSimpleString();
Object.ToDictionary();
String.Color(Color.red).Bold().Italic().Size(30);

Example Project
---
To be developed

Contact
---
Kananat Suwanviwatana (a.k.a @kdev)
Email: kananat.s@gmail.com
 
License
---
This library is under MIT License.
