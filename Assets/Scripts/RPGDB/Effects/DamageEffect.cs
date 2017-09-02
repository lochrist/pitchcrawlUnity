using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Flags]
public enum DamageType {
    Physical = 1 << 0, // assume if no flag, it is normal damage.
    Magic = 1 << 1,

    Fire = 1 << 2,
    Cold = 1 << 3,
    
    Electricity = 1 << 4,
    Earth = 1 << 5,
    
    Shadow = 1 << 6,
    Light = 1 << 7,
    
    Holy = 1 << 8,
    Demonic = 1 << 9
}



public class DamageEffect : EffectDescriptor {
    public AttributeValue dmg;
    [EnumFlagsAttribute]
    public DamageType damageType;
    public bool ignoreArmor;
    public bool ignoreDmgModifier;

    List<DamageModifierEffect> dmgModifiers = new List<DamageModifierEffect>();
    List<DamageSensitivityEffect> dmgSensitivity = new List<DamageSensitivityEffect>();
    List<ArmorModifierEffect> armorModifiers = new List<ArmorModifierEffect>();

    protected override void DoEffectImp (GameObject src, GameObject target, BaseAction context) {
        var targetCharacter = target.GetComponent<PitchCharacter> ();
        if (dmg.modifierType == ModifierType.Add) {
            ApplyDamage(src, target, context);

        } else {
            // Cure Damage:
            Utils.Log ("Heal Damage on:", targetCharacter.name);
            int dmgValue = dmg.GetValue(src.GetComponent<PitchCharacter>());
            targetCharacter.hitPoint.current = System.Math.Min (targetCharacter.hitPoint.current + dmgValue, targetCharacter.hitPoint.baseValue);
        }
    }

    //////////////////////////////////////////////////

    void ApplyDamage (GameObject src, GameObject target, BaseAction context) {
        PitchCharacter srcCharacter = src.GetComponent<PitchCharacter>();
        if (!srcCharacter) {
            throw new UnityException("ApplyDamage: Src is not a Character " + src.ToString());
        }

        PitchCharacter targetCharacter = target.GetComponent<PitchCharacter>();
        if (!srcCharacter) {
            throw new UnityException("ApplyDamage: Target is not a Character " + src.ToString());
        }

        Utils.Log ("start applying Damage:");

        DamageType currentDmgType = damageType;
        int currentDmg = dmg.GetValue(srcCharacter);
        Utils.Log ("\t", "Initial Dmg:", currentDmg, "Type:", currentDmgType.ToString());

        if (!ignoreDmgModifier) {
            CheckDamageModifier (srcCharacter, targetCharacter, context, ref currentDmgType, ref currentDmg);
            CheckDamageSensitivity (targetCharacter, currentDmgType, ref currentDmg);
        }
                
        int armor = 0;
        if (!ignoreArmor) {
            CheckArmor (targetCharacter, ref armor);
            // Check for armor malus
            CheckArmorModifier (srcCharacter, targetCharacter, context, ref armor);
            // Check for defensive armor bonus
            CheckArmorModifier (targetCharacter, targetCharacter, context, ref armor);
            armor = System.Math.Max(armor, 0);
        }
                
        // At this point, dmg, dmgType and armor are final.
        int finalDamage = System.Math.Max(currentDmg - armor, 0);
        if (finalDamage > 0) {
            targetCharacter.TriggerAbilities (AbilityUsageType.OnSrcReceivesDamageFromTarget, target, src, context);
            srcCharacter.TriggerAbilities (AbilityUsageType.OnSrcInflictsDamageToTarget, src, target, context);

            Utils.Log ("\t", "Apply Damage:", "dmg:", currentDmg, "dmgType:", currentDmgType.ToString(), "armor:", armor, "target:", target.name);
            targetCharacter.hitPoint.current = System.Math.Max (targetCharacter.hitPoint.current - finalDamage, 0);

            UIRoot.Instance.SpawnDmgMsg(finalDamage, targetCharacter);

            if (targetCharacter.hitPoint.current == 0) {
                targetCharacter.TriggerAbilities (AbilityUsageType.OnSrcDiesByTarget, target, src, context);
                if (targetCharacter.hitPoint.current == 0) {
                    // Is the character still dead?
                    srcCharacter.TriggerAbilities (AbilityUsageType.OnSrcKillsEnemyTarget, src, target, context);
                    GameManager.Instance.KillCharacter (targetCharacter);
                }
            }
        } else {
            Utils.Log ("\t", "Final Damage not enough", "dmg:", currentDmg, "dmgType:", currentDmgType.ToString(), "armor:", armor, "target:", target.name);
        }
    }

    void CheckDamageModifier(PitchCharacter src, PitchCharacter target, BaseAction context, ref DamageType dmgType, ref int dmg) {
        // Check all Effect already on character providing DmgModifier. Do not check on AttackAction
        dmgModifiers.Clear ();
        src.GetEffects (dmgModifiers);

        for (int i = 0; i < dmgModifiers.Count; ++i) {
            var mod = dmgModifiers[i];
            if (mod.IsApplicable(src, target, context)) {
                Utils.Log ("\t", "Damage Modifier: type", mod.damageType.ToString(), "DmgMod", mod.damageModifier);
                dmgType |= mod.damageType;
                dmg += mod.damageModifier.GetValue (src);
            }
        }
    }
    
    void CheckDamageSensitivity(PitchCharacter c, DamageType dmgType, ref int dmg) {
        // Check all Effect already on character providing DmgSensitivity. Do not check on AttackAction
        dmgSensitivity.Clear ();
        c.GetEffects (dmgSensitivity);
        for (int i = 0; i < dmgSensitivity.Count; ++i) {
            // Only use the first that fits the bill:
            var mod = dmgSensitivity[i];
            if (dmgType.IsFlagSet(mod.damageType)) {
                Utils.Log ("\t", "Damage Sensitivity: type", mod.damageType.ToString(), "DmgRatio", mod.damageModifier);
                dmg = (int)(dmg * mod.damageModifier);
                break;
            }
        }
    }
    
    void CheckArmor(PitchCharacter c, ref int armor) {
        // Check character base Armor.
        armor = c.armor.current;
    }
    
    void CheckArmorModifier(PitchCharacter src, PitchCharacter target, BaseAction context, ref int armor) {
        // Check all Effect already on character providing DmgModifier. Do not check on AttackAction

        armorModifiers.Clear ();
        src.GetEffects (armorModifiers);
        for (int i = 0; i < armorModifiers.Count; ++i) {
            var mod = armorModifiers[i];
            if (mod.IsApplicable(src, target, context)) {
                Utils.Log ("\t", "Armor Modifier: ", mod.armorModifier);
                armor += mod.armorModifier.GetValue (src);
            }
        }

    }
}
