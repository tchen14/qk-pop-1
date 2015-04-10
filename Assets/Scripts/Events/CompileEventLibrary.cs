#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Reflection;
using System.Linq;

public class CompileEventLibrary : EditorWindow {

    [MenuItem("Custom Tools/Compile Event Library")]
    static void Compile() {
        int count = 0;

        string fileName = Application.dataPath + "\\Scripts\\Events\\Plugins\\EventLibrary.cs";

        string compilationString = "/*\n\n\n\n\n\n\n\n\n\n\n\n\t\t\t\t\t\tThis script has been automatically generated.\n\t\t\t\t\t\tDo not alter it, or your changes will be undone.\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n*/\n";
        compilationString += "using System.Collections.Generic;\npublic static class EventLibrary {\n\n\tpublic static Dictionary<string, string[]> library = new Dictionary<string, string[]> {\n";

        // Use reflection to get all classes in the project
         var q = from t in Assembly.GetExecutingAssembly().GetTypes() where t.IsClass select t;
        List<System.Type> classList = new List<System.Type>();
        q.ToList().ForEach(t => classList.Add(t));

        // Get every method with the EventVisibleAttribute
        foreach (var t in classList) {
            bool validMethods = false;
            string compilationBuffer = "\t\t{ \"" + t.Name + "Methods\", new string[] {";

            foreach (var m in t.GetMethods()) {
                count++;
                List<string> attList = new List<string>();
                foreach (var att in m.GetCustomAttributes(false)) {
                    attList.Add(att.ToString());
                }
                if (ListContains(attList, "EventVisibleAttribute")) {
                    validMethods = true;
                    compilationBuffer += ("\"" + m.Name + "\", ");
                }
            }
            compilationBuffer += "} },\n";
            if (validMethods == true) {
                compilationString += compilationBuffer;
            }
        }

        // Not get every field with the EventVisibleAttribute
        foreach (var t in classList) {
            bool validMethods = false;
            string compilationBuffer = "\t\t{ \"" + t.Name + "Fields\", new string[] {";

            foreach (var m in t.GetFields()) {
                count++;
                List<string> attList = new List<string>();
                foreach (var att in m.GetCustomAttributes(false)) {
                    attList.Add(att.ToString());
                }
                if (ListContains(attList, "EventVisibleAttribute")) {
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

        if (count == 0) {
            compilationString = "/*\n\n\n\n\n\n\n\n\n\n\n\n\t\t\t\t\t\tThere are no valid fields or methods in the project.\n\n\t\t\t\t\t\t\t\tUse [EventField] and [EventMethod]\n\n\n\n\n\n\n\n\n\n\n\n\n*/\n";
            compilationString += "using System.Collections.Generic;\npublic static class EventLibrary {\n\tpublic static Dictionary<string, string[]> library = new Dictionary<string, string[]>();\n}";
        }

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

        AssetDatabase.ImportAsset("Assets/Scripts/Events/Plugins/EventLibrary.cs");
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