using UnityEngine;
using System.Collections;
using UnityEditor;

//[CustomPropertyDrawer(typeof(MyBaseClass), true)]
public class MyBaseClassEditor : PropertyDrawer {

    public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
        return property.isExpanded ? 64f : 16f;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        Debug.Log (property.propertyType + " " + property.propertyPath + " " + property.name + " " + property.isArray);



        // base.OnGUI (position, property, label);
        EditorGUI.PropertyField (position, property, true);

        foreach (var p in property) {
            Debug.Log (p.GetType().FullName);
        }
    }
}
