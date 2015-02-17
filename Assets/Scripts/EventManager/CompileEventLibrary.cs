#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Reflection;
using System.Linq;

public class CompileEventLibrary : EditorWindow {

    [MenuItem("Tools/Compile Event Library")]
    static void Compile() {

        string fileName = Application.dataPath + "\\EventManager\\Plugins\\EventLibrary.cs";

        string compilationString = "/*\n\n\n\n\n\n\n\n\n\n\n\n\t\t\t\t\t\tThis script has been automatically generated.\n\t\t\t\t\t\tDo not alter it, or your changes will be undone.\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n*/\n";
        compilationString += "using System.Collections.Generic;\npublic static class EventLibrary {\n\n\tpublic static Dictionary<string, string[]> library = new Dictionary<string, string[]> {\n";

        // Use reflection to get all classes in the project
         var q = from t in Assembly.GetExecutingAssembly().GetTypes() where t.IsClass select t;
        List<System.Type> classList = new List<System.Type>();
        q.ToList().ForEach(t => classList.Add(t));

        // Get every method with the MethodEventAttribute
        foreach (var t in classList) {
            bool validMethods = false;
            string compilationBuffer = "\t\t{ \"" + t.Name + "Methods\", new string[] {";

            foreach (var m in t.GetMethods()) {
                List<string> attList = new List<string>();
                foreach (var att in m.GetCustomAttributes(false)) {
                    attList.Add(att.ToString());
                }
                if (ListContains(attList, "EventMethodAttribute")) {
                    validMethods = true;
                    compilationBuffer += ("\"" + m.Name + "\", ");
                }
            }
            compilationBuffer += "} },\n";
            if (validMethods == true) {
                compilationString += compilationBuffer;
            }
        }

        // Not get every field with the MethodEventAttribute
        foreach (var t in classList) {
            bool validMethods = false;
            string compilationBuffer = "\t\t{ \"" + t.Name + "Fields\", new string[] {";

            foreach (var m in t.GetFields()) {
                List<string> attList = new List<string>();
                foreach (var att in m.GetCustomAttributes(false)) {
                    attList.Add(att.ToString());
                }
                if (ListContains(attList, "EventFieldAttribute")) {
                    validMethods = true;
                    compilationBuffer += ("\"" + m.Name + "\", ");
                }
            }
            compilationBuffer += "} },\n";
            if (validMethods == true) {
                compilationString += compilationBuffer;
            }
        }

        // Close up the string and write it to file
        compilationString += "\t};\n}";

        StreamWriter streamWriter;
        FileInfo fileInfo = new FileInfo(fileName);
        if (fileInfo.IsReadOnly == true) {
            fileInfo.IsReadOnly = false;
        }

        if (fileInfo.Exists) {
            fileInfo.Delete();
        }
        streamWriter = fileInfo.CreateText();
        streamWriter.Write(compilationString);
        streamWriter.Close();

        fileInfo.IsReadOnly = true;
    }

    public static bool ListContains(List<string> stringList, string name) {
        foreach (var s in stringList) {
            if (s == name) {
                return true;
            }
        }
        return false;
    }
}
#endif