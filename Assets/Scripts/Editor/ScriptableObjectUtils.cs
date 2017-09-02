using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public static class ScriptableObjectUtils
{
    /// <summary>
    //  This makes it easy to create, name and place unique new ScriptableObject asset files.
    /// </summary>
    public static void CreateAsset<T> () where T : ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance<T> ();
        
        string path = AssetDatabase.GetAssetPath (Selection.activeObject);
        if (path == "") 
        {
            path = "Assets/Resources";
        } 
        else if (Path.GetExtension (path) != "") 
        {
            path = path.Replace (Path.GetFileName (AssetDatabase.GetAssetPath (Selection.activeObject)), "");
        }
        
        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath (path + "/New " + typeof(T).ToString() + ".asset");
        
        AssetDatabase.CreateAsset (asset, assetPathAndName);

        AssetDatabase.SaveAssets ();
        EditorUtility.FocusProjectWindow ();
        Selection.activeObject = asset;
    }


    [MenuItem("Assets/Create/Skill")]
    public static void CreateAbility ()
    {
        ScriptableObjectUtils.CreateAsset<Skill> ();
    }

    [MenuItem("Assets/Create/Item")]
    public static void CreateItem ()
    {
        ScriptableObjectUtils.CreateAsset<Item> ();
    }


}