using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
 
namespace NexoUtilities
{
 
    public class ColorToCSharpCode : EditorWindow
    {
        private static GUIStyle previewLabelStyle;
       
        List<Color> m_colors = new List<Color> ();
        private Color m_color = Color.white;
        private bool m_color32 = false;
        private bool m_alpha = true;
        private bool m_textColor = true;
       
        [MenuItem ("Tools/Color Code Generator")]
        private static void ShowWindow ()
        {
            GetWindow<ColorToCSharpCode> (true, "Color to C# Code", true);
        }
       
        private void OnGUI ()
        {
            if (previewLabelStyle == null)
            {
                previewLabelStyle = EditorStyles.label;
                previewLabelStyle.richText = true;
            }
           
            this.m_textColor = EditorGUILayout.Toggle ("Text Color", this.m_textColor);
            this.m_color32 = EditorGUILayout.Toggle ("Color32", this.m_color32);
            this.m_alpha = EditorGUILayout.Toggle ("Alpha", this.m_alpha);
            this.m_color = EditorGUILayout.ColorField ("Color", this.m_color);
           
            string csharpCode = ColorToCode (this.m_color);
           
            GUILayout.TextField (csharpCode);
           
            GUILayout.BeginHorizontal ();
           
            if (GUILayout.Button ("Copy Code"))
            {
                CopyText (csharpCode);
            }
           
            if (GUILayout.Button ("Push"))
            {
                this.m_colors.Add (this.m_color);
            }
           
            GUILayout.EndHorizontal ();
           
            string label = "new " + (this.m_color32 ? "Color32" : "Color") + "["
                           + this.m_colors.Count + "] \n{\n";
                           
            for (int i = 0; i < this.m_colors.Count; i++)
            {
                bool flag = Event.current.type == EventType.Repaint && this.m_textColor;
               
                if (flag)
                    label += "<color=#" + ColorUtility.ToHtmlStringRGB (this.m_colors[i]) + ">";
                   
                label += "\t" + ColorToCode (this.m_colors[i]) + (flag ? "</color>" : "") + ",\n";
               
            }
           
            label += "};\n";
           
            GUILayout.Label (label, previewLabelStyle,
                             GUILayout.Height ((this.m_colors.Count + 3) * 16));
                             
            GUILayout.BeginHorizontal ();
           
            if (GUILayout.Button ("Copy Code"))
            {
                CopyText (label);
            }
           
            if (GUILayout.Button ("Clear"))
            {
                this.m_colors.Clear ();
            }
           
            GUILayout.EndHorizontal ();
           
            GUILayout.Space (16);
           
            float height = GUILayoutUtility.GetLastRect ().yMax;
           
            if (position.height != height && Event.current.type == EventType.Repaint)
            {
                position = new Rect (position.x, position.y, position.width, height);
            }
        }
       
        private string ColorToCode (Color c)
        {
            if (this.m_color32)
            {
                return string.Format ("new Color32 ({0:0}, {1:0}, {2:0}, {3:0})",
                                      c.r * 255,
                                      c.g * 255,
                                      c.b * 255,
                                      this.m_alpha ? (c.a * 255) : 255);
            }
           
           
            return string.Format ("new Color ({0:0.##}f, {1:0.###}f, {2:0.###}f{3:0.###})",
                                  c.r,
                                  c.g,
                                  c.b,
                                  GetAlphaString (c.a));
        }
       
        private string GetAlphaString (float alpha)
        {
            if (this.m_alpha)
            {
                return string.Format (", {0:0.###}f", alpha);
            }
           
            return "";
        }
       
        void CopyText (string txt)
        {
            TextEditor editor = new TextEditor ();
           
            //editor.content = new GUIContent(pText); // Unity 4.x code
            editor.text = txt; // Unity 5.x code
           
            editor.SelectAll ();
            editor.Copy ();
        }
    }
}