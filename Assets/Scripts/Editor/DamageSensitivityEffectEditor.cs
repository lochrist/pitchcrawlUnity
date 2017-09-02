using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(DamageSensitivityEffect), true)]
public class DamageSensitivityEffectEditor : Editor {
    public void OnEnable () {
        // EffectDescriptor effect = target as EffectDescriptor;

    }
    
    public override void OnInspectorGUI() {
        DamageSensitivityEffect effect = target as DamageSensitivityEffect;
        
        EditorGUILayout.BeginHorizontal ();
        EditorGUILayout.LabelField ("Effect Type");
        EditorGUILayout.LabelField (effect.effectType.ToString());
        EditorGUILayout.EndHorizontal ();
        
        GuiUtils.DrawHorizontalLine (1, GuiUtils.DividerColor, 3, 8);
        
        DrawDefaultInspector ();

        EditorGUILayout.LabelField ("Damage Modifier", effect.damageModifier.ToString());
    }
    
}
