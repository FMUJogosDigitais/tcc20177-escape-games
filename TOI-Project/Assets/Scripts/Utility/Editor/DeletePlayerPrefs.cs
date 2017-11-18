using UnityEditor;
using UnityEngine;

public class DeletePlayerPrefs : Editor
{
	[MenuItem("Jogo/Apagar Save do Jogo", false, 151)]
	public static void DeleteSave()
	{
		if (EditorUtility.DisplayDialog("Tem certeza?",
				"Você quer apagar o save do jogo?", "Sim", "Não"))
			PlayerPrefs.DeleteAll();
	}
}
