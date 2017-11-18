/*
 * Created by Mauricio Cunha
 * http://www.mcunha98.com
 * mcunha98@gmail.com
 * 
 * To run the script import in your Unity and go to the Tools -> Screenshot
*/
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class Screenshot : EditorWindow
{
	private int resWidth = Screen.width;
    private int resHeight = Screen.height;
	private string path = "";
    private string prefix = "screenshot";
    private bool showPreview = true;
    private bool captureScreenshot = false;
    
    private RenderTexture renderTexture;
    private Camera myCamera;

	[MenuItem("Tools/Screenshot")]
	public static void ShowWindow()
	{
		EditorWindow editorWindow = EditorWindow.GetWindow(typeof(Screenshot));
		editorWindow.autoRepaintOnSceneChange = true;
		editorWindow.Show();
	}

	void OnGUI()
	{
		EditorGUILayout.LabelField ("Resolution", EditorStyles.boldLabel);
		resWidth = EditorGUILayout.IntField ("Width", resWidth);
		resHeight = EditorGUILayout.IntField ("Height", resHeight);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Options", EditorStyles.boldLabel);
        prefix = EditorGUILayout.TextField("Prefix for file", prefix);
        showPreview = EditorGUILayout.Toggle("Show screenshot", showPreview);

        GUILayout.Label("Save Path", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.TextField(path, GUILayout.ExpandWidth(false));
        if (GUILayout.Button("Browse", GUILayout.ExpandWidth(false)))
        {
            path = EditorUtility.SaveFolderPanel("Path to Save Images", path, Application.dataPath);
        }
        if (GUILayout.Button("Open Folder"))
        {

            Application.OpenURL("file://" + path);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
		GUILayout.Label ("Select Camera", EditorStyles.boldLabel);
		myCamera = EditorGUILayout.ObjectField(myCamera, typeof(Camera), true,null) as Camera;
		if(myCamera == null)
		{
			myCamera = Camera.main;
		}
		EditorGUILayout.Space();

		if(GUILayout.Button("Take Screenshot",GUILayout.MinHeight(60)))
		{
			if(path == "")
			{
				path = EditorUtility.SaveFolderPanel("Path to Save Images",path,Application.dataPath);
			}
            captureScreenshot = true;
        }

		EditorGUILayout.Space();


		if (captureScreenshot) 
		{
			int resWidthN = resWidth;
			int resHeightN = resHeight;
            TextureFormat tFormat = TextureFormat.RGB24;
            RenderTexture rt = new RenderTexture(resWidthN, resHeightN, 24);

            myCamera.targetTexture = rt;


			Texture2D screenShot = new Texture2D(resWidthN, resHeightN, tFormat,false);
			myCamera.Render();
			RenderTexture.active = rt;
			screenShot.ReadPixels(new Rect(0, 0, resWidthN, resHeightN), 0, 0);
			myCamera.targetTexture = null;
			RenderTexture.active = null; 
			byte[] bytes = screenShot.EncodeToPNG();
			string filename = ScreenShotName(resWidthN, resHeightN);
			System.IO.File.WriteAllBytes(filename, bytes);
			
            if (showPreview)
            {
                Application.OpenURL(filename);
            }
			captureScreenshot = false;
		}
	}

	public string ScreenShotName(int width, int height) 
    {
		string strPath="";
        if (string.IsNullOrEmpty(prefix)) prefix = "screenshot";
		strPath = string.Format("{0}/{1}_{2}.png", path, prefix, System.DateTime.Now.ToString("yyyyMMdd_HHmmss"));
		return strPath;
	}
}

