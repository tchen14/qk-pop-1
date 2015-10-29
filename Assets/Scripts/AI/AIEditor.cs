using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEditor.AnimatedValues;

public struct AI_Data
{
	private int hp_;
	private float sightDistance_;
	private float sightAngle_;
	private float speed_;
	private float runSpeed_;
	private string[] seekTag_;
	private float attackDistance_;
	private float aggressionLimit_;
	private string panicPoints_;
	private bool aggressive_;

	public AI_Data(int hp,
	               float sightDistance,
	               float sightAngle,
	               float speed,
	               float runSpeed,
	               string[] seekTag,
	               float attackDistance,
	               float aggressionLimit,
	               string panicPoints,
	               bool aggressive)
	{
		hp_ = hp;
		sightDistance_ = sightDistance;
		sightAngle_ = sightAngle;
		speed_ = speed;
		runSpeed_ = runSpeed;
		seekTag_ = seekTag;
		attackDistance_ = attackDistance;
		aggressionLimit_ = aggressionLimit;
		panicPoints_ = panicPoints;
		aggressive_ = aggressive;
	}

	public void loadData(AIMain target)
	{
		target.hp = hp_;
		target.sightDistance = sightDistance_;
		target.sightAngle = sightAngle_;
		target.speed = speed_;
		target.runSpeed = runSpeed_;
		target.seekTag = seekTag_;
		target.aggressionLimit = aggressionLimit_;
		target.aggressive = aggressive_;
	}
}

[CustomEditor(typeof(AIMain), true)]
public class AIEditor : Editor {

	AIMain ai_target;
	AnimBool show_data;
	string[] ai_types = new string[]{"Villager", "Guard", "Commander"};

	AI_Data[] ai_data = new AI_Data[]{
		new AI_Data(100, 20, 35, 5, 8, new string[]{"Player"}, 3, 100, "PanicPoints", false),
		new AI_Data(200, 40, 35, 7, 12, new string[]{"Player"}, 5, 100, "PanicPoints", false),
		new AI_Data(300, 60, 35, 12, 16, new string[]{"Player"}, 7, 100, "PanicPoints", false)};

	int ai_types_index = 0;
	int current_selection = 0;
	int current_preset = 0;

	void OnEnable()
	{
		ai_target = (AIMain)target;
		ai_types_index = ai_target.current_preset;
		show_data = new AnimBool(false);
		show_data.valueChanged.AddListener(Repaint);
	}

	override public void OnInspectorGUI()
	{
		EditorGUI.BeginChangeCheck();

		EditorGUILayout.BeginVertical();
		ai_types_index = EditorGUILayout.Popup ("AI Type:", ai_types_index, ai_types);

		EditorGUILayout.EndVertical();

		show_data.target = EditorGUILayout.Toggle ("Show Data", show_data.target);

		if (EditorGUILayout.BeginFadeGroup (show_data.faded))
		{
			EditorGUILayout.LabelField("Health: ",ai_target.hp.ToString() );
			EditorGUILayout.LabelField("Sight Distance: ",ai_target.sightDistance.ToString() );
			EditorGUILayout.LabelField("Sight Angle: ",ai_target.sightAngle.ToString() );
			EditorGUILayout.LabelField("Speed: ",ai_target.speed.ToString() );
			EditorGUILayout.LabelField("Running Speed: ",ai_target.runSpeed.ToString() );
			EditorGUILayout.LabelField("Targets: ",print_array(ai_target.seekTag));
			EditorGUILayout.LabelField("Attack Distance: ",ai_target.attackDistance.ToString() );
			EditorGUILayout.LabelField("Aggression Limit: ",ai_target.aggressionLimit.ToString() );
			EditorGUILayout.LabelField("aggressive: ", ai_target.aggressive.ToString() );
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
