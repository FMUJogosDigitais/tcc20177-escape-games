using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

public class EzBookFilesRaname : EditorWindow {

	string fileNewName;
	string separatorStrg = "";

	int fixedInitialNumber, initialNumber;
	
	bool makeSequential = false;

	enum SequenceOptions {
		AfterName = 0, BeforeName
	}
	SequenceOptions sequenceOptions = SequenceOptions.AfterName;

	enum SeparatedBy {
		Hyphen, Point, Space_, Underline, Nothing 
	}
	SeparatedBy separatedBy = SeparatedBy.Space_;

	#region [ Menu Items ]
	[MenuItem("Window/File Rename/Open")]
	static void OpenFileRenameWindow()
	{
		EditorWindow.GetWindow(typeof(EzBookFilesRaname));
	}
	[MenuItem("Window/File Rename/Sort Selected")]
	static void SortSelectedObjs()
	{
		SortSelected();
	}
	#endregion

	void OnGUI()
	{
		GUILayout.Label("File Rename", EditorStyles.boldLabel);
		GUILayout.Space(10);
		fileNewName = EditorGUILayout.TextField("File Name:", fileNewName);
		
		makeSequential = EditorGUILayout.Toggle("Make Sequential", makeSequential);
		if(makeSequential)
		{
			sequenceOptions = (SequenceOptions)EditorGUILayout.EnumPopup("Sequence goes:", sequenceOptions);
			fixedInitialNumber = EditorGUILayout.IntField("Initial Number", fixedInitialNumber);
			separatedBy = (SeparatedBy)EditorGUILayout.EnumPopup("Separated By:", separatedBy);
		}

		// Display the buttons to rename side by side
		GUILayout.BeginHorizontal();
		if(GUILayout.Button("Remane Hierarchy Files"))
			HirarchyFiles();
		if(GUILayout.Button("Rename Project Files"))
			ProjectFiles();
		GUILayout.EndHorizontal();

		// Display the button to sort the selected items
		if(GUILayout.Button("Sort Selected"))
			SortSelected();
	}

	#region [ Rename ]
	void HirarchyFiles()
	{
		GameObject[] tempSelection = Selection.gameObjects;

		if(tempSelection.Length <= 0)
			Debug.LogWarning("There isn't no game object selected. Please, select the game objects that you want to rename and try again.");
		else
		{
			// This part will sort the array that is keeping the selected game objects. It is a human like sort, x11 comes after xx2.
			System.Array.Sort(tempSelection, delegate(GameObject tempSelection0, GameObject tempSelection1) {
				return EditorUtility.NaturalCompare(tempSelection0.name, tempSelection1.name);
			});
			
			initialNumber = fixedInitialNumber;
			foreach (GameObject gameObj in tempSelection) {

				if(makeSequential)
				{
					switch(sequenceOptions)
					{
					case SequenceOptions.AfterName:
						gameObj.name = fileNewName + GetSeparationType() + initialNumber;
						break;
					case SequenceOptions.BeforeName:
						gameObj.name = initialNumber + GetSeparationType() + fileNewName;
						break;
					}
				}
				else
					gameObj.name = fileNewName;

				initialNumber++;
			}
		}
	}
	
	void ProjectFiles()
	{
		Object[] selectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

		if(selectedAsset.Length <= 0)
			Debug.LogWarning("There isn't no asset selected. Please, select the assets that you want to rename and try again.");
		else
		{
			initialNumber = fixedInitialNumber;
			string path; // Keep the path of the current selected asset

			// Reset the obj name to prevent conflict, because in the project folder it is not permited 2 files with the same name
			foreach(Object obj in selectedAsset)
			{
				path = AssetDatabase.GetAssetPath(obj);
				AssetDatabase.RenameAsset(path, initialNumber.ToString());
				initialNumber++;
			}
			
			initialNumber = fixedInitialNumber;

			// Rename the assets
			foreach(Object obj in selectedAsset)
			{
				path = AssetDatabase.GetAssetPath(obj);
				if(makeSequential)
				{
					switch(sequenceOptions)
					{
					case SequenceOptions.AfterName:
						AssetDatabase.RenameAsset(path, fileNewName + GetSeparationType() + initialNumber);
						break;
						
					case SequenceOptions.BeforeName:
						AssetDatabase.RenameAsset(path, initialNumber + GetSeparationType() + fileNewName);
						break;
					}
				}
				else
					AssetDatabase.RenameAsset(path, fileNewName + initialNumber.ToString());
				
				initialNumber++;
			}
		}
	}
	
	string GetSeparationType()
	{
		switch(separatedBy)
		{
		case SeparatedBy.Hyphen:
			separatorStrg =  "-";
			break;
			
		case SeparatedBy.Nothing:
			separatorStrg = "";
			break;
			
		case SeparatedBy.Point:
			separatorStrg = ".";
			break;
			
		case SeparatedBy.Space_:
			separatorStrg = " ";
			break;
			
		case SeparatedBy.Underline:
			separatorStrg = "_";
			break;
		}
		
		return separatorStrg;
	}
	#endregion

	#region [ Sort ]
	static void SortSelected ()
	{
		// Get the selected transforms
		Transform[] Objtransforms = Selection.transforms;

		if(Objtransforms.Length <= 0)
			Debug.LogWarning("There isn't no game object selected. Please, select the game objects that you want to sort and try again.");
		else
		{
			// Get the lowest index from the selected transforms
			int newIndex = GetLowestIndex(Objtransforms);

			// This part will sort the array that is keeping the selected game objects. It is a human like sort, x11 comes after xx2.
			System.Array.Sort(Objtransforms, delegate(Transform tempObjTrans0, Transform tempObjTrans1) {
				return EditorUtility.NaturalCompare(tempObjTrans0.name, tempObjTrans1.name);
			});
			
			// Set the index of the selected game objects
			foreach (Transform currentTransform in Objtransforms.Cast<Transform>())
			{
				currentTransform.SetSiblingIndex (newIndex);
				newIndex++;
			}
		}
	}

	static int GetLowestIndex (Transform[] objTransform) 
	{
		int lowestIndex = 9999;
		int index;
		
		for (int i = 0; i < objTransform.Length; i++) 
		{
			index = objTransform[i].GetSiblingIndex();
			
			if (index < lowestIndex) 
				lowestIndex = index;
		}
		
		return lowestIndex;
	}
	#endregion
}