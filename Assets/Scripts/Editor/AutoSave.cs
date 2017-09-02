// Simple editor window that autosaves the working scene
// Make sure to have this window opened to be able to execute the auto save.

using UnityEditor;
using UnityEngine;

class AutoSave : EditorWindow
{

    double saveTime = 300;
    double nextSave = 0;

    [MenuItem("Tools/Auto Save (300s)")]
    static void Init()
    {
        AutoSave window = EditorWindow.GetWindowWithRect<AutoSave>(new Rect(0, 0, 165, 40));
        window.Show();
    }

    void OnGUI() 
    {
		EditorGUILayout.LabelField("Save Each:", saveTime + " Secs");
		double timeToSave = nextSave - EditorApplication.timeSinceStartup;
		EditorGUILayout.LabelField("Next Save:", timeToSave.ToString() + " Sec");
		this.Repaint();
			 
		if(EditorApplication.timeSinceStartup > nextSave) 
        {
			EditorApplication.SaveScene();
			Debug.Log("Saved Scene");
			nextSave = EditorApplication.timeSinceStartup + saveTime;
		}
	}
}