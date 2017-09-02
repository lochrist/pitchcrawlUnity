using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollisionAbility : AbilityDescriptor {
    public CollisionTriggerType collisionType;

    public void HandleCollision (GameObject src, Collision2D collision, List<Collision2D> collisionList, BaseAction context) {
        switch (collisionType) {
        case CollisionTriggerType.FirstContactTarget:
            if (collisionList.Count == 0) {
                ActivateAbility (src, collision, collisionList, context);
            }
            break;
        case CollisionTriggerType.EachContactTarget:
            ActivateAbility (src, collision, collisionList, context);
            break;
        }
    }

    public void ActivateAbility (GameObject src, Collision2D collision, List<Collision2D> collisionList, BaseAction context) {
        collisionList.Add (collision);

        CheckOnContact (src, collision.gameObject, context);
        CheckOnContact (collision.gameObject, src, context);

        ActivateAbility(src, collision.gameObject, context);
    }


    void CheckOnContact(GameObject src, GameObject target, BaseAction context) {
        PitchCharacter targetCharacter = target.GetComponent<PitchCharacter> ();
        if (targetCharacter) {
            targetCharacter.TriggerAbilities(AbilityUsageType.OnSrcContactsTarget, src, target, context);
        }
    }

}
