using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.AnimatedValues;

/*
 * AI Editor Script
 * 
 * This script is used to easily swap between common AI types by loading
 * different behavior values to the AI script. It uses the sctruct AI_Data
 * to store each values and to load them to the AI.
 */

public struct AI_Data
{
	private int hp_;
	private float sightDistance_;
	private float passiveSightAngle_;
	private float speed_;
	private float runSpeed_;
	private string[] seekTag_;
	private float attackDistance_;
	private float aggressionLimit_;
	private string panicPoints_;
	private bool aggression_;
	private List<GameObject> paths;
    private float suspicion_;
    private float chasingSightAngle_;

	public AI_Data(int hp,
	               float SightDistance,
	               float passiveSightAngle,
	               float speed,
	               float runSpeed,
	               string[] seekTag,
	               float attackDistance,
	               float aggressionLimit,
	               string panicPoints,
	               bool aggression,
                   float suspicionLimit,
                   float chasingSightAngle)
	{
		hp_ = hp;
		sightDistance_ = SightDistance;
		passiveSightAngle_ = passiveSightAngle;
		speed_ = speed;
		runSpeed_ = runSpeed;
		seekTag_ = seekTag;
		attackDistance_ = attackDistance;
		aggressionLimit_ = aggressionLimit;
		panicPoints_ = panicPoints;
		aggression_ = aggression;
		paths = new List<GameObject> ();
        suspicion_ = suspicionLimit;
        chasingSightAngle_ = chasingSightAngle;
	}

	public void loadData(AIMainTrimmed target)
	{
		target.hp = hp_;
		target.sightDistance = sightDistance_;
		target.passiveSightAngle = passiveSightAngle_;
		target.speed = speed_;
		target.runSpeed = runSpeed_;
		target.seekTag = seekTag_;
		target.aggressionLimit = aggressionLimit_;
		target.panicPoints = panicPoints_;
		target.enemy = aggression_;
        target.suspicionLimit = suspicion_;
        target.chasingSightAngle = chasingSightAngle_;
	}
}

[CustomEditor(typeof(AIMainTrimmed), true)]
public class AIEditor : Editor {

	AIMainTrimmed ai_target;
	AnimBool show_data;
	string[] ai_types = new string[]{"Villager", "Guard", "Commander"};
	string[] path_types = new string[]{"one way", "loop around", "back and forth", "On Guard"};

	AI_Data[] ai_data = new AI_Data[]{
		new AI_Data(100, 5, 35, 5, 8, new string[]{"Player"}, 3, 10, "PanicPoints", false, 10, 70),
		new AI_Data(200, 15, 35, 6, 12, new string[]{"Player"}, 5, 5, "PanicPoints", true, 5, 70),
		new AI_Data(300, 5, 35, 7, 16, new string[]{"Player"}, 7, 10, "PanicPoints", true, 10,70)};

	int ai_types_index = 0;
	int current_selection = 0;


	void OnEnable()
	{
		ai_target = (AIMainTrimmed)target;
		ai_types_index = ai_target.current_preset;
		show_data = new AnimBool(false);
		show_data.valueChanged.AddListener(Repaint);
	}

	override public void OnInspectorGUI()
	{
		EditorGUILayout.BeginVertical();

		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label("AI Type:", GUILayout.MaxWidth(60));
		ai_types_index = EditorGUILayout.Popup (ai_types_index, ai_types, GUILayout.MaxWidth(200));
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Add Path", GUILayout.MaxWidth(100))){
			ai_target.Pathways.Add(null);
			ai_target.PathType.Add(0);
			ai_target.infinite.Add(false);
			ai_target.nofLoops.Add(1);
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Separator ();

		for (int i=0; i < ai_target.Pathways.Count; i++) {
			EditorGUILayout.BeginHorizontal();
			if(i < ai_target.Pathways.Count){
				ai_target.Pathways[i] = EditorGUILayout.ObjectField (ai_target.Pathways[i], typeof(GameObject), true, GUILayout.Width(80)) as GameObject; 
			}

			if(i < ai_target.Pathways.Count){
				GUILayout.Label("Loop Type:", GUILayout.MaxWidth(80));
				ai_target.PathType[i] = EditorGUILayout.Popup (ai_target.PathType[i], path_types, GUILayout.MaxWidth(100));
				GUILayout.Label("infinite?", GUILayout.MaxWidth(50));
				ai_target.infinite[i] = EditorGUILayout.Toggle(ai_target.infinite[i], GUILayout.MaxWidth(20));
				GUILayout.Label("number of loops", GUILayout.MaxWidth(90));
				ai_target.nofLoops[i] = EditorGUILayout.IntField(ai_target.nofLoops[i], GUILayout.MaxWidth(30));
			}
			if(GUILayout.Button("Remove Path")){
				ai_target.Pathways.RemoveAt(i);
				ai_target.PathType.RemoveAt(i);
				ai_target.infinite.RemoveAt(i);
				ai_target.nofLoops.RemoveAt(i);
			}
			EditorGUILayout.EndHorizontal();
		}
		EditorGUILayout.EndVertical();

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Show Data:", GUILayout.MaxWidth(70));
		show_data.target = EditorGUILayout.Toggle (show_data.target);
		EditorGUILayout.EndHorizontal ();

		if (EditorGUILayout.BeginFadeGroup (show_data.faded))
		{
			ai_target.hp = EditorGUILayout.FloatField("Health:", ai_target.hp);
			ai_target.sightDistance = EditorGUILayout.FloatField("Sight Distance:", ai_target.sightDistance);
			ai_target.passiveSightAngle = EditorGUILayout.FloatField("Passive Sight Angle:", ai_target.passiveSightAngle);
			ai_target.chasingSightAngle = EditorGUILayout.FloatField("Chasing Sight Angle:", ai_target.chasingSightAngle);
			ai_target.speed = EditorGUILayout.FloatField("Speed:", ai_target.speed);
			ai_target.runSpeed = EditorGUILayout.FloatField("Running Speed:", ai_target.runSpeed);
			ai_target.attackDistance = EditorGUILayout.FloatField("Attack Distance:", ai_target.attackDistance);
			ai_target.aggressionLimit = EditorGUILayout.FloatField("Aggression Limit:", ai_target.aggressionLimit);
			ai_target.enemy = EditorGUILayout.Toggle("Aggressive:", ai_target.enemy);
		}
		EditorGUILayout.EndFadeGroup();

		if (current_selection != ai_types_index)
		{
			loadAI();
			current_selection = ai_types_index;
			ai_target.current_preset = ai_types_index;
		}
	}

	private void loadAI()
	{
		ai_data[ai_types_index].loadData(ai_target);
	}

	private string print_array(string[] arr){
		string result = "";
		foreach(string s in arr){
			result += " \"";
			result += s;
			result += "\"";
		}
		return result;
	}
}
