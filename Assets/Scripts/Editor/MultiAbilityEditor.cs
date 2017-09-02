using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(MultiAbility), true)]
public class MultiAbilityEditor : Editor {
    public List<ChainedAbilityItem> chainedAbilities;
    public AbilityType abilityToCreateType;
    public int indexToDelete = 0;

    public virtual void OnEnable () {
        MultiAbility obj = target as MultiAbility;
        
        chainedAbilities = obj.abilities;
        abilityToCreateType = AbilityType.Melee;
        indexToDelete = 0;
    }

    public override void OnInspectorGUI() {
        MultiAbility ability = target as MultiAbility;

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

        bool hasChanges = false;

        {
            EditorGUILayout.BeginHorizontal ();
            abilityToCreateType = (AbilityType)EditorGUILayout.EnumPopup (abilityToCreateType);

            if (GUILayout.Button ("Create")) {
                string className = abilityToCreateType.ToString () + "Ability";
                AbilityDescriptor created = ScriptableObject.CreateInstance (className) as AbilityDescriptor;
                if (created) {
                    created.name = abilityToCreateType.ToString ();
                    created.abilityType = abilityToCreateType;
                
                    AssetDatabase.AddObjectToAsset (created, ability);

                    var chainedItem = new ChainedAbilityItem ();
                    chainedItem.ability = created;

                    chainedAbilities.Add (chainedItem);
                    hasChanges = true;
                }
            }
            EditorGUILayout.EndHorizontal ();
        }

        {
            EditorGUILayout.BeginHorizontal ();
        
            indexToDelete = EditorGUILayout.IntField ("Index:", indexToDelete);
        
            if (GUILayout.Button ("Delete")) {
                if (indexToDelete >= 0 && indexToDelete < chainedAbilities.Count) {
                    var toDelete = chainedAbilities [indexToDelete].ability;
                    chainedAbilities.RemoveAt (indexToDelete);
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
