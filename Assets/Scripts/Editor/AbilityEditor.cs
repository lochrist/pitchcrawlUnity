using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(AbilityDescriptor), true)]
public class AbilityEditor : ObjectListEditor<EffectDescriptor, EffectType> {
    public override void OnEnable () {
        base.OnEnable ();
    }

    public override List<EffectDescriptor> GetObjectList () {
        AbilityDescriptor ability = target as AbilityDescriptor;
        return ability.effects;
    }

    public override void InitCreatedObject (EffectDescriptor objCreated, System.Enum enumType) {

    }

    public override void InitConfig (EditorConfig config) {
        config.supportsRename = false;
        foreach (string atypeName in System.Enum.GetNames (typeof(EffectType))) {
            config.objectTypeNames[atypeName] = atypeName + "Effect";
        }
    }
    
    public override void OnInspectorGUI() {
        AbilityDescriptor ability = target as AbilityDescriptor;

        {
            EditorGUILayout.BeginHorizontal ();
            EditorGUILayout.LabelField ("Ability Type");
            EditorGUILayout.LabelField (ability.abilityType.ToString ());
            EditorGUILayout.EndHorizontal ();
        }

        GuiUtils.DrawHorizontalLine (1, GuiUtils.DividerColor, 3, 8);

        DrawDefaultInspector ();

        GuiUtils.DrawHorizontalLine (1, GuiUtils.DividerColor, 3, 8);
        EditorGUILayout.Space ();
        
        DrawListToolbar ();
    }
	
}
