using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(AbilityContainer), true)]
public class AbilityContainerEditor : ObjectListEditor<AbilityDescriptor, AbilityType> {
    public override List<AbilityDescriptor> GetObjectList () {
        var obj = target as AbilityContainer;
        return obj.abilities;
    }
    public override void InitCreatedObject (AbilityDescriptor objCreated, System.Enum enumType) {
        objCreated.abilityType = (AbilityType)enumType;
    }
    public override void InitConfig (EditorConfig config) {
        config.supportsRename = true;
        foreach (string atypeName in System.Enum.GetNames (typeof(AbilityType))) {
            config.objectTypeNames[atypeName] = atypeName + "Ability";
        }
    }

    public override void OnEnable () {
        base.OnEnable ();
    }
    
    public override void OnInspectorGUI() {
        // AbilityContainer container = target as AbilityContainer;

        DrawDefaultInspector ();

        GuiUtils.DrawHorizontalLine (1, GuiUtils.DividerColor, 3, 8);
        EditorGUILayout.Space ();

        DrawListToolbar ();
    }
}
