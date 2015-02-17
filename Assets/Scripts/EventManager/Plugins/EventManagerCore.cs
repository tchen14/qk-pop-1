using UnityEngine;
using System.Diagnostics;

public class EventFieldAttribute : System.Attribute { }
public class EventMethodAttribute : System.Attribute {
    public EventMethodAttribute() {
        Trace.WriteLine("message: ");
    }
}

public class EventManagerCore {

    //      Keep these to streamline things later
    //public static MethodDelegate methodDelegate;
    //public static SetFieldDelegate setFieldDelegate;
    //public static GetFieldDelegate getFieldDelegate;

}


public delegate object MethodDelegate(object o, object[] p);
public delegate void SetFieldDelegate(object o, object p);
public delegate object GetFieldDelegate(object o);

