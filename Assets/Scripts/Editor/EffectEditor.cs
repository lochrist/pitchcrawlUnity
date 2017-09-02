using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(EffectDescriptor), true)]
public class EffectEditor : Editor {
           
    public void OnEnable () {
        // EffectDescriptor effect = target as EffectDescriptor;
    }
    
    public override void OnInspectorGUI() {
        EffectDescriptor effect = target as EffectDescriptor;
        {
            EditorGUILayout.BeginHorizontal ();
            EditorGUILayout.LabelField ("Effect Type");
            EditorGUILayout.LabelField (effect.effectType.ToString ());
            EditorGUILayout.EndHorizontal ();
        }
        EditorGUILayout.Space ();
        EditorGUILayout.LabelField ("If AOE, Valid Target doesn't apply on initial Target.");
        
        GuiUtils.DrawHorizontalLine (1, GuiUtils.DividerColor, 3, 8);

        DrawDefaultInspector ();
    }

}
