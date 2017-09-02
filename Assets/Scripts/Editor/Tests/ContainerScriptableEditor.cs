using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;


// boxes and such




[CustomEditor(typeof(ContainerScriptable))]
public class ContainerScriptableEditor : Editor {


    public enum ElementType {
        BaseClass,
        DeriveClass
    }

    int indexToDelete;
    ElementType currentElementTypeToCreate = ElementType.BaseClass;
    string newName;

    public void OnEnable () {
        ContainerScriptable obj = target as ContainerScriptable;
        // Setup the SerializedProperties
        // instances = serializedObject.FindProperty ("m_Instances");
        newName = obj.name;
        indexToDelete = 0;
    }

    public override void OnInspectorGUI() {
        ContainerScriptable obj = target as ContainerScriptable;
        /*
        foreach (var instance in obj.m_Instances) {
            EditorGUILayout.PropertyField(instance);
        }
        */

        // EditorGUILayout.PropertyField(instances, true);

        DrawDefaultInspector ();

        bool hasChanges = false;

        GuiUtils.DrawHorizontalLine (1, GuiUtils.DividerColor, 3, 8);
        EditorGUILayout.Space ();


        EditorGUILayout.BeginHorizontal ();
        newName = EditorGUILayout.TextField ("Name", newName);

        if (GUILayout.Button("Rename")) {
            if (newName != obj.name) {
                string assetPath = AssetDatabase.GetAssetPath(obj);
                string extension = Path.GetExtension(assetPath);
                string directory = Path.GetDirectoryName(assetPath);
                string newAssetPath = directory + "/" + newName + extension;
                string uniquePath = AssetDatabase.GenerateUniqueAssetPath(newAssetPath);
                newName = Path.GetFileNameWithoutExtension(uniquePath);
                string result = AssetDatabase.RenameAsset(assetPath, newName);

                if (result.Length == 0) {
                    hasChanges = true;
                    obj.name = newName;
                } else {
                    Debug.LogError(result);
                }
            }
        }
        EditorGUILayout.EndHorizontal ();
        


        EditorGUILayout.BeginHorizontal ();
        currentElementTypeToCreate = (ElementType)EditorGUILayout.EnumPopup (currentElementTypeToCreate);

        if (GUILayout.Button ("Create")) {
            BaseClassScriptable created = null;
            switch (currentElementTypeToCreate) {
            case ElementType.BaseClass:
                created = ScriptableObject.CreateInstance<BaseClassScriptable> ();
                break;
            case ElementType.DeriveClass:
                created = ScriptableObject.CreateInstance<DerivedClassScriptable> ();
                break;
            }
            if (created) {
                created.name = currentElementTypeToCreate.ToString();
                AssetDatabase.AddObjectToAsset(created, obj);
                obj.attrs.Add (created);
                hasChanges = true;
            }
        }
        EditorGUILayout.EndHorizontal ();

        EditorGUILayout.BeginHorizontal ();

        indexToDelete = EditorGUILayout.IntField ("Index:", indexToDelete);

        if (GUILayout.Button ("Delete")) {
            if (indexToDelete >= 0 && indexToDelete < obj.attrs.Count) {
                var oo = obj.attrs[indexToDelete];
                obj.attrs.RemoveAt(indexToDelete);
                Object.DestroyImmediate(oo, true);
                
                hasChanges = true;
            }
        }
        EditorGUILayout.EndHorizontal ();

        if (hasChanges) {
            //AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(animationClip));
            AssetDatabase.SaveAssets();
        }

    }
}

static public class ScriptableAttrUtils {
    
    [MenuItem("Assets/Create/ContainerScriptable")]
    public static void TestXmlSerialization () {
        ScriptableObjectUtils.CreateAsset<ContainerScriptable> ();
    }
}