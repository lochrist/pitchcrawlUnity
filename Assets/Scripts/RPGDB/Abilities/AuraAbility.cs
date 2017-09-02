using UnityEngine;
using System.Collections;


public class AuraAbility : AbilityDescriptor {
    public float auraRadius = 1.0f;
    public GameObject auraPrefab;
    public AuraHandler handler;
    private GameObject aura;

    public override void ActivateAbility(GameObject src, GameObject target, BaseAction context) {
        if (aura) {
            throw new UnityException("Cannot activate an aura twice!");
        }
        if (!auraPrefab) {
            throw new UnityException("Aura ability needs an auraPrefab");
        }
        if (auraRadius <= 0.0f) {
            throw new UnityException("Aura radius cannot be 0.0f");
        }

        aura = GameObject.Instantiate(auraPrefab) as GameObject;
        AuraHandler.Attach (aura, src, this);

        aura.transform.parent = src.transform;
        aura.transform.localPosition = Vector3.zero;
        float spriteRadius = Utils.GetSpriteWorldSize (aura).x / 2;
        float newSpriteScale = auraRadius / spriteRadius;
        aura.transform.localScale = new Vector3 (newSpriteScale, newSpriteScale, 1.0f);
    }

    public override void DeactivateAbility (GameObject src) {
        if (aura) {
            Object.Destroy(aura);
        }
    }

    public void AddAuraEffects (GameObject src, GameObject target) {
        base.ActivateAbility (src, target, null);
    }

    public void RemoveAuraEffects (GameObject src, GameObject target) {
        base.DeactivateAbility (target);
    }
}
