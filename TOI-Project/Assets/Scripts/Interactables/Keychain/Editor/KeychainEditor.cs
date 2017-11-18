using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Keychain))]
public class KeychainEditor : Editor
{
    private bool[] showItemSlots = new bool[Keychain.NUM_KEY_SLOTS];
    private SerializedProperty keyInvProperty;
    private SerializedProperty haveKeyProperty;
    private SerializedProperty doorProperty;
    private SerializedProperty keyObjProperty;

    private const string propName_KEYINV = "keyInv";
    private const string propName_HAVEKEY = "haveKey";
    private const string propName_DOOR = "door";
    private const string propName_KEYOBJ = "keyObj";

    private void OnEnable()
    {
        keyInvProperty = serializedObject.FindProperty(propName_KEYINV);
        haveKeyProperty = serializedObject.FindProperty(propName_HAVEKEY);
        doorProperty = serializedObject.FindProperty(propName_DOOR);
        keyObjProperty = serializedObject.FindProperty(propName_KEYOBJ);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        for (int i = 0; i < Keychain.NUM_KEY_SLOTS; i++)
        {
            KeychainGUI(i);
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void KeychainGUI(int index)
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;

        showItemSlots[index] = EditorGUILayout.Foldout(showItemSlots[index], "Key slot " + index);

        if (showItemSlots[index])
        {
            EditorGUILayout.PropertyField(keyInvProperty.GetArrayElementAtIndex(index));
            EditorGUILayout.PropertyField(haveKeyProperty.GetArrayElementAtIndex(index));
            EditorGUILayout.PropertyField(doorProperty.GetArrayElementAtIndex(index));
            EditorGUILayout.PropertyField(keyObjProperty.GetArrayElementAtIndex(index));
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();

    }
}
