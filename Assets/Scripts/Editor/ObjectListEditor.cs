using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

public abstract class ObjectListEditor<ObjectType, ObjectEnumType> : Editor where ObjectType : ScriptableObject  {
    public class EditorConfig {
        public bool supportsRename;
        public Dictionary<string, string> objectTypeNames = new Dictionary<string, string>();
        public bool inited;
    }

    int indexToDelete;
    System.Enum objectTypeEnum;
    string newName;
    public List<ObjectType> objectList;

    EditorConfig config = new EditorConfig();

    //abstract public int EnumTypeToInt(System.Enum genericEnum);
    abstract public List<ObjectType> GetObjectList ();
    abstract public void InitCreatedObject (ObjectType objCreated, System.Enum enumType);
    abstract public void InitConfig (EditorConfig config);

    public virtual void OnEnable () {
        if (!config.inited) {
            InitConfig (config);
            config.inited = true;
        }

        ScriptableObject obj = target as ScriptableObject;

        objectList = GetObjectList ();
        objectTypeEnum = (System.Enum)System.Enum.GetValues (typeof(ObjectEnumType)).GetValue (0);
        indexToDelete = 0;
        newName = obj.name;
    }

    public void DrawListToolbar () {
        ScriptableObject targetObj = target as ScriptableObject;

        bool hasChanges = false;
        if (config.supportsRename) {
            EditorGUILayout.BeginHorizontal ();
            newName = EditorGUILayout.TextField ("Name", newName);

            if (GUILayout.Button ("Rename")) {
                if (newName != targetObj.name) {
                    string assetPath = AssetDatabase.GetAssetPath (targetObj);
                    string extension = Path.GetExtension (assetPath);
                    string directory = Path.GetDirectoryName (assetPath);
                    string newAssetPath = directory + "/" + newName + extension;
                    string uniquePath = AssetDatabase.GenerateUniqueAssetPath (newAssetPath);
                    newName = Path.GetFileNameWithoutExtension (uniquePath);
                    string result = AssetDatabase.RenameAsset (assetPath, newName);
                    
                    if (result.Length == 0) {
                        hasChanges = true;
                        targetObj.name = newName;
                    } else {
                        Debug.LogError (result);
                    }
                }
            }
            EditorGUILayout.EndHorizontal ();
        }
        
        
        {
            EditorGUILayout.BeginHorizontal ();
            objectTypeEnum = EditorGUILayout.EnumPopup (objectTypeEnum);
        
            if (GUILayout.Button ("Create")) {
                string className = config.objectTypeNames [objectTypeEnum.ToString ()];
                ObjectType created = ScriptableObject.CreateInstance (className) as ObjectType;
                if (created) {
                    created.name = objectTypeEnum.ToString ();
                    InitCreatedObject (created, objectTypeEnum);

                    AssetDatabase.AddObjectToAsset (created, targetObj);
                    objectList.Add (created);
                    hasChanges = true;
                }
            }
            EditorGUILayout.EndHorizontal ();
        }

        {
            EditorGUILayout.BeginHorizontal ();
        
            indexToDelete = EditorGUILayout.IntField ("Index:", indexToDelete);
        
            if (GUILayout.Button ("Delete")) {
                if (indexToDelete >= 0 && indexToDelete < objectList.Count) {
                    var toDelete = objectList [indexToDelete];
                    objectList.RemoveAt (indexToDelete);
                    Object.DestroyImmediate (toDelete, true);
                    hasChanges = true;
                }
            }
            EditorGUILayout.EndHorizontal ();
        }
        
        if (hasChanges) {
            //AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(animationClip));
            AssetDatabase.SaveAssets();
        }
    }

}
