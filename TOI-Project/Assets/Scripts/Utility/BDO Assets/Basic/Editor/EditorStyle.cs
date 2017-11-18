using UnityEngine;
using System.Collections;

public class EditorStyle {

	private GUIStyle customStyle = new GUIStyle();

	public void SetFontStyle(FontStyle newFontStyle) { customStyle.fontStyle = newFontStyle; }
	public void SetTextAlignmt(TextAnchor newAlignment) { customStyle.alignment = newAlignment; }
	public void SetNormalTextColor(Color newColor) { customStyle.normal.textColor = newColor; }

	public GUIStyle CustomStyle() { return this.customStyle; }
	public GUIStyle TitleStyle()
	{
		customStyle.alignment = TextAnchor.MiddleCenter;
		customStyle.fontStyle = FontStyle.Bold;
		if(UnityEditorInternal.InternalEditorUtility.HasPro())
			customStyle.normal.textColor = Color.white;
		else
			customStyle.normal.textColor = Color.black;

		return this.customStyle;
	}
}
