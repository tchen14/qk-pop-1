public class EventTable {
    public EventEntry[] triggerEntries;
    public EventEntry[] actionEntries;

    public EventTable(string name) {
        if (name == "Left") {
            triggerEntries = new EventEntry[] { new EventEntry("Press One X Times", TestType.Int), 
                                                new EventEntry("Press Two", TestType.Void),
                                                new EventEntry("Every One Second", TestType.Void)};

            actionEntries = new EventEntry[] {  new EventEntry("FunctionOne", TestType.Void),
                                                new EventEntry("FunctionTwo", TestType.Void)};
        }
        if (name == "Right") {
            triggerEntries = new EventEntry[] { };

            actionEntries = new EventEntry[] {  new EventEntry("IntFunction", TestType.Int), 
                                                new EventEntry("NullFunction", TestType.Void),
                                                new EventEntry("VectorFunction", TestType.Vector3)};
        }
    }

    public string[] GetKeywords() {
        return GetArray(triggerEntries);
    }

    public string[] GetFunctions() {
        return GetArray(actionEntries);
    }

    private string[] GetArray(EventEntry[] eventEntries) {
        int length = eventEntries.Length;
        string[] entries = new string[length];
        for (int i = 0; i < length; i++) {
            entries[i] = eventEntries[i].keyword;
        }
        return entries;
    }
}

public enum TestType { Void, Int, Float, String, Vector3 }

public class EventEntry {
    public string keyword;
    public TestType testType;

    public EventEntry(string newKeyword, TestType newTestType) {
        keyword = newKeyword;
        testType = newTestType;
    }
}