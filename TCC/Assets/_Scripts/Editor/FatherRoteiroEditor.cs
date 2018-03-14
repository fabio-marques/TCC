﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[System.Serializable]
public class AddStateInfo{
	public FatherStates newState;
}

[CustomEditor(typeof(RoteiroPai))]
public class FatherRoteiroEditor : Editor {

	private ReorderableList list;
	private RoteiroPai roteiro;

	float lineHeight;
	float lineHeightSpace;
	float elementHeightSpace;

	GUIStyle labelStyle = new GUIStyle ();

	private void OnEnable() {
		if(target == null){
			return;
		}

		roteiro = (RoteiroPai)target;

		lineHeight = EditorGUIUtility.singleLineHeight;
		lineHeightSpace = lineHeight + 5;
		elementHeightSpace = lineHeight + 10;

		labelStyle.fontStyle = FontStyle.Bold;
		labelStyle.clipping = TextClipping.Clip;

		list = new ReorderableList(serializedObject, 
			serializedObject.FindProperty("roteiro"), 
			true, false, true, true);

		list.drawHeaderCallback = (Rect rect) =>
		{
			rect.y += 2;
			EditorGUI.LabelField(rect, new GUIContent("Roteiro do Pai na Fase"));
		};

		//list.drawElementBackgroundCallback

		list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
		{
			var element = list.serializedProperty.GetArrayElementAtIndex(index);

			rect.y += 2;
			EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width - 80, lineHeight), element.FindPropertyRelative("name").stringValue, labelStyle);

			EditorGUIUtility.labelWidth = 40;
			EditorGUI.PropertyField(new Rect(rect.width - 40, rect.y, 80, lineHeight), element.FindPropertyRelative("show"));
			EditorGUIUtility.labelWidth = 0;

			if(element.FindPropertyRelative("show").boolValue){
				int i = 1;

				EditorGUI.PropertyField(new Rect(rect.x, rect.y + (lineHeightSpace * i), rect.width, lineHeight), element.FindPropertyRelative("name"));
				i++;
				EditorGUI.PropertyField(new Rect(rect.x, rect.y + (lineHeightSpace * i), rect.width, lineHeight), element.FindPropertyRelative("state"));
				i++;

				if(element.FindPropertyRelative("state").enumValueIndex == 5 || element.FindPropertyRelative("state").enumValueIndex == 7 || element.FindPropertyRelative("state").enumValueIndex == 8){
					EditorGUI.indentLevel++;
					EditorGUI.PropertyField(new Rect(rect.x, rect.y + (lineHeightSpace * i), rect.width, lineHeight), element.FindPropertyRelative("destination"));
					i++;
					EditorGUI.indentLevel--;
				}
				else if(element.FindPropertyRelative("state").enumValueIndex == 2){
					EditorGUI.indentLevel++;
					EditorGUI.PropertyField(new Rect(rect.x, rect.y + (lineHeightSpace * i), rect.width, lineHeight), element.FindPropertyRelative("areaCenter"));
					i++;
					EditorGUI.PropertyField(new Rect(rect.x, rect.y + (lineHeightSpace * i), rect.width, lineHeight), element.FindPropertyRelative("areaRadius"));
					i++;
					EditorGUI.indentLevel--;
				}

				// ---------------------------- OPTIONAL VARIABLES ------------------------- \\
				if(element.FindPropertyRelative("state").enumValueIndex == 3 || element.FindPropertyRelative("state").enumValueIndex == 8){
					EditorGUI.indentLevel++;
					element.FindPropertyRelative("showAdvanced").boolValue = EditorGUI.Foldout(new Rect(rect.x, rect.y + (lineHeightSpace * i), rect.width, lineHeight), element.FindPropertyRelative("showAdvanced").boolValue, new GUIContent("Advanced"), new GUIStyle(EditorStyles.foldout));
					i++;
					if(element.FindPropertyRelative("showAdvanced").boolValue){
						EditorGUI.indentLevel++;
						EditorGUI.PropertyField(new Rect(rect.x, rect.y + (lineHeightSpace * i), rect.width, lineHeight), element.FindPropertyRelative("secondsFlying"));
						i++;
						EditorGUI.PropertyField(new Rect(rect.x, rect.y + (lineHeightSpace * i), rect.width, lineHeight), element.FindPropertyRelative("allowSlowFalling"));
						i++;
						EditorGUI.PropertyField(new Rect(rect.x, rect.y + (lineHeightSpace * i), rect.width, lineHeight), element.FindPropertyRelative("jumpHeight"));
						i++;
						EditorGUI.PropertyField(new Rect(rect.x, rect.y + (lineHeightSpace * i), rect.width, lineHeight), element.FindPropertyRelative("timeToJumpApex"));
						i++;
						EditorGUI.indentLevel--;
					}
					EditorGUI.indentLevel--;
				}
				else if(element.FindPropertyRelative("state").enumValueIndex == 4){
					EditorGUI.indentLevel++;
					element.FindPropertyRelative("showAdvanced").boolValue = EditorGUI.Foldout(new Rect(rect.x, rect.y + (lineHeightSpace * i), rect.width, lineHeight), element.FindPropertyRelative("showAdvanced").boolValue, new GUIContent("Advanced"));
					i++;
					if(element.FindPropertyRelative("showAdvanced").boolValue){
						EditorGUI.indentLevel++;
						EditorGUI.PropertyField(new Rect(rect.x, rect.y + (lineHeightSpace * i), rect.width, lineHeight), element.FindPropertyRelative("jumpHeight"));
						i++;
						EditorGUI.PropertyField(new Rect(rect.x, rect.y + (lineHeightSpace * i), rect.width, lineHeight), element.FindPropertyRelative("timeToJumpApex"));
						i++;
						EditorGUI.indentLevel--;
					}
					EditorGUI.indentLevel--;
				}
				// ------------------------------------------------------------------------- \\

				EditorGUI.PropertyField(new Rect(rect.x, rect.y + (lineHeightSpace * i), rect.width, lineHeight), element.FindPropertyRelative("stateChanger"));
				i++;
				EditorGUI.PropertyField(new Rect(rect.x, rect.y + (lineHeightSpace * i), rect.width, lineHeight), element.FindPropertyRelative("triggerDetail"));
				//i++;

				serializedObject.ApplyModifiedProperties();
			}
		};


		list.elementHeightCallback = (int index) =>
		{
			float height = 0;

			var element = list.serializedProperty.GetArrayElementAtIndex(index);

			float i = 4;

			if(element.FindPropertyRelative("show").boolValue){
				switch (element.FindPropertyRelative("state").enumValueIndex) {
				case 2:
					i = 5;
					break;
				case 3:
					i = (element.FindPropertyRelative("showAdvanced").boolValue)? 9 : 5;
					break;
				case 4:
					i = (element.FindPropertyRelative("showAdvanced").boolValue)? 7 : 5;
					break;
				case 5:
					i = (element.FindPropertyRelative("showAdvanced").boolValue)? 6 : 5;
					break;
				case 7:
					i = (element.FindPropertyRelative("showAdvanced").boolValue)? 6 : 5;
					break;
				case 8:
					i = (element.FindPropertyRelative("showAdvanced").boolValue)? 10 : 5;
					break;
				default:
				break;
				}
			}
			else {
				i = 1.5f;
			}
				
			height = elementHeightSpace * i;

			serializedObject.ApplyModifiedProperties();
			return height;
		};

		list.onAddDropdownCallback = (Rect rect, ReorderableList rList) =>
		{
			GenericMenu dropdownMenu = new GenericMenu();

			dropdownMenu.AddItem(new GUIContent("Add State"), false, AddState, new AddStateInfo { newState = FatherStates.Inactive });

			dropdownMenu.AddItem(new GUIContent("Idle/Inactive"), false, AddState, new AddStateInfo { newState = FatherStates.Inactive });
			dropdownMenu.AddItem(new GUIContent("Idle/LookingAtPlayer"), false, AddState, new AddStateInfo { newState = FatherStates.LookingAtPlayer });
			dropdownMenu.AddItem(new GUIContent("Idle/RandomWalk"), false, AddState, new AddStateInfo { newState = FatherStates.RandomWalk });
			dropdownMenu.AddItem(new GUIContent("Idle/Gliding"), false, AddState, new AddStateInfo { newState = FatherStates.Gliding });
			dropdownMenu.AddItem(new GUIContent("Idle/Jumping"), false, AddState, new AddStateInfo { newState = FatherStates.Jumping });

			dropdownMenu.AddItem(new GUIContent("Walk/SimpleWalk"), false, AddState, new AddStateInfo { newState = FatherStates.SimpleWalk });
			dropdownMenu.AddItem(new GUIContent("Walk/FollowPlayer"), false, AddState, new AddStateInfo { newState = FatherStates.FollowingPlayer });
			dropdownMenu.AddItem(new GUIContent("Walk/GuidePlayer"), false, AddState, new AddStateInfo { newState = FatherStates.GuidingPlayer });
			dropdownMenu.AddItem(new GUIContent("Walk/Flying"), false, AddState, new AddStateInfo { newState = FatherStates.Flying });

			dropdownMenu.ShowAsContext ();
		};

	}

	public void AddState (object obj){
		AddStateInfo stateInfo = (AddStateInfo)obj;

		RoteiroPai.Roteiro newAction = new RoteiroPai.Roteiro ();
		newAction.state = stateInfo.newState;
		newAction.name = "Estado " + roteiro.roteiro.Count.ToString ();
		//newAction.show = true;

		roteiro.roteiro.Add (newAction);
	}

	public override void OnInspectorGUI() {
		//TODO:Comment later
		//DrawDefaultInspector ();

		serializedObject.Update();
		list.DoLayoutList();
		serializedObject.ApplyModifiedProperties();
	}

}