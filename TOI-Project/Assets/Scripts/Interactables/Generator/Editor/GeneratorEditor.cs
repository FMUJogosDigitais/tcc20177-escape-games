using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Generator))]
public class GeneratorEditor : Editor
{
	private bool[] showItemSlots = new bool[Generator.NUM_SWITCHES];
	private SerializedProperty keySwitchProperty;
	private SerializedProperty keyLeverProperty;
	private SerializedProperty keySwitchStateProperty;

	private SerializedProperty keyBinaryAnswerProperty;
	private SerializedProperty keyNumericAnswerProperty;

	private SerializedProperty keyBinaryExpectedProperty;
	private SerializedProperty keyNumericExpectedProperty;

	private const string propName_SWITCHES = "switches";
	private const string propName_LEVERS = "levers";
	private const string propName_SWITCH_STATE = "switchState";

	private const string propName_BINARY_ANSWER = "binaryAnswer";
	private const string propName_NUMERIC_ANSWER = "numericAnswer";

	private const string propName_BINARY_EXPECTED = "binaryExpected";
	private const string propName_NUMERIC_EXPECTED = "numericExpected";

	private void OnEnable()
	{
		keySwitchProperty = serializedObject.FindProperty(propName_SWITCHES);
		keyLeverProperty = serializedObject.FindProperty(propName_LEVERS);
		keySwitchStateProperty = serializedObject.FindProperty(propName_SWITCH_STATE);

		keyBinaryAnswerProperty = serializedObject.FindProperty(propName_BINARY_ANSWER);
		keyNumericAnswerProperty = serializedObject.FindProperty(propName_NUMERIC_ANSWER);

		keyBinaryExpectedProperty = serializedObject.FindProperty(propName_BINARY_EXPECTED);
		keyNumericExpectedProperty = serializedObject.FindProperty(propName_NUMERIC_EXPECTED);
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		for (int i = 0; i < Generator.NUM_SWITCHES; i++)
		{
			GeneratorLogicGUI(i);
		}
		EditorGUILayout.PropertyField(keyBinaryAnswerProperty);
		EditorGUILayout.PropertyField(keyNumericAnswerProperty);

		EditorGUILayout.PropertyField(keyBinaryExpectedProperty);
		EditorGUILayout.PropertyField(keyNumericExpectedProperty);

		serializedObject.ApplyModifiedProperties();
	}

	private void GeneratorLogicGUI(int index)
	{
		EditorGUILayout.BeginVertical(GUI.skin.box);
		EditorGUI.indentLevel++;

		//showItemSlots[index] = EditorGUILayout.Foldout(showItemSlots[index], "Switch Slot " + index);
		showItemSlots[index] = EditorGUILayout.Foldout(true, "Switch Slot " + index);

		if (showItemSlots[index])
		{
			EditorGUILayout.PropertyField(keySwitchProperty.GetArrayElementAtIndex(index));
			EditorGUILayout.PropertyField(keyLeverProperty.GetArrayElementAtIndex(index));
			EditorGUILayout.PropertyField(keySwitchStateProperty.GetArrayElementAtIndex(index));
		}

		EditorGUI.indentLevel--;
		EditorGUILayout.EndVertical();

	}
}
