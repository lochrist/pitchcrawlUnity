using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnumFlagsAttribute : PropertyAttribute
{
    public EnumFlagsAttribute() { }
}


[System.Serializable]
public class Trait {
    public string name;
    public int value;
}

static public class TraitUtils {
    static public bool HasTrait(Trait t, List<Trait> traits) {
        return traits.Find (trait => trait.name == t.name) != null;
    }

    static public bool HasAnyTrait(List<Trait> t, List<Trait> traits) {
        for (int i = 0; i < t.Count; ++i) {
            bool found = HasTrait (t[i], traits);
            if (found) {
                return true;
            }
        }
        return false;
    }
}

public enum RequirementType {
    Class, // lister toutes les classes ici???
    Race, // lister toutes les races ici?
    Level
}

[System.Serializable]
public class Requirement {
    public RequirementType requirementType;
    public Trait value;
}

[System.Serializable]
public class AttributeValue {
    public AttributeType attrType = AttributeType.None;
    public int customValue = 1;
    public bool addRandomizer = false;
    public ModifierType modifierType;

    public int GetValue(PitchCharacter c) {
        int value = customValue;
        if (attrType != AttributeType.None) {
            Attribute a = c.Attribute(attrType);
            value = a.current;
        }

        if (modifierType == ModifierType.Remove) {
            value = -value;
        }

        return value;
    }
}


public class AbilityContainer : ScriptableObject, System.ICloneable {
    public Texture2D icon;

    public List<Trait> traits;
    public List<Requirement> requirements;
    public List<AbilityDescriptor> abilities;

    public DurationType usageType = DurationType.Permanent;
    public int maxNbOfUses;
    public int nbOfUses;
    public bool destroyOnLastUse;

    public object Clone () {
        AbilityContainer clone = Object.Instantiate (this) as AbilityContainer;

        clone.name = this.name;
        for (int i = 0; i < clone.abilities.Count; ++i) {
            var clonedAbility = clone.abilities[i].Clone() as AbilityDescriptor;
            clone.abilities[i] = clonedAbility;
        }
        return clone;
    }
}
