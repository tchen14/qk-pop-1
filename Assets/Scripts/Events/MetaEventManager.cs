using UnityEngine;
using System;
using System.Collections;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif

[AttributeUsage(AttributeTargets.Method)]
public class MethodEventAttribute : System.Attribute {
	public MethodEventAttribute(){}
}

public class MetaEventManager : MonoBehaviour {

	void Start () {
		Type[] types = Assembly.GetExecutingAssembly().GetTypes();
		
		for(int j = 0; j< types.Length; j++){
			Type type = typeof(MonoBehaviour);
			MethodInfo[] info = types[j].GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy |
												BindingFlags.Static | BindingFlags.Instance);
			
			foreach(MethodInfo i in info) {
				object[] attributes = i.GetCustomAttributes(typeof(MethodEventAttribute),true);
				if(attributes.Length == 1){
					Debug.Log(i.Name);
				}else if(attributes.Length > 1){
					Debug.Log("Potential error on " + i.Name);
				}
			}
		}
		#if UNITY_EDITOR
			EditorApplication.isPlaying = false;
		#endif
	}
	
	public void Function1(){}
	
	[MethodEventAttribute]
	public void Function2(){}
	
	[MethodEventAttribute]
	public void Function3(){}
	
	public void Function4(){}
}
