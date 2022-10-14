// LeaderboardSystemEditor : Descrption : Custom Editor for LeaderboardSystem
#if (UNITY_EDITOR)
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(LeaderboardSystem))]
public class LeaderboardSystemEditor : Editor {
	public bool 			SeeInspector = false;						// use to draw default Inspector

	SerializedProperty 		MaxLetter;									// Max length name
	SerializedProperty 		MaxScoreDisplay;							// Max number scoredisplay on screen


	private Texture2D MakeTex(int width, int height, Color col) {		// use to change the GUIStyle
		Color[] pix = new Color[width * height];
		for (int i = 0; i < pix.Length; ++i) {
			pix[i] = col;
		}
		Texture2D result = new Texture2D(width, height);
		result.SetPixels(pix);
		result.Apply();
		return result;
	}

	void OnEnable () {
		// Setup the SerializedProperties.
		MaxLetter = serializedObject.FindProperty ("MaxLetter");
		MaxScoreDisplay = serializedObject.FindProperty ("MaxScoreDisplay");
	}

	public override void OnInspectorGUI()
	{
		SeeInspector = EditorGUILayout.Foldout(SeeInspector,"Inspector");

		if(SeeInspector)							// If true Default Inspector is drawn on screen
			DrawDefaultInspector();

		serializedObject.Update ();
		GUIStyle style = new GUIStyle(GUI.skin.box);

		GUILayout.Label("");

		style.normal.background = MakeTex(2, 2, new Color(1,0,1,.3f));
		EditorGUILayout.HelpBox("1 - Setup Leaderboard",MessageType.Info);

// --> Section : Max number for Name
		EditorGUILayout.BeginHorizontal(style);
			EditorGUILayout.LabelField("Name Max Characters : ", GUILayout.Width(220));
			EditorGUILayout.PropertyField(MaxLetter, new GUIContent (""));
		EditorGUILayout.EndHorizontal();


// --> Section : Max Score display on screen 
		EditorGUILayout.BeginHorizontal(style);
			EditorGUILayout.LabelField("Max Score display on Leaderboard : ", GUILayout.Width(220));
			EditorGUILayout.PropertyField(MaxScoreDisplay, new GUIContent (""));
		EditorGUILayout.EndHorizontal();

		serializedObject.ApplyModifiedProperties ();
	}
}
#endif