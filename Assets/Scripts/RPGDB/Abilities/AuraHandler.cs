using UnityEngine;
using System.Collections;

public class AuraHandler : MonoBehaviour {
    private AuraAbility aura;
    private GameObject src;
    
    static public AuraHandler Attach (GameObject auraObject, GameObject src, AuraAbility aura) {
        AuraHandler handler = auraObject.AddComponent<AuraHandler>();
        handler.Init (src, aura);
        return handler;
    }
    
    public void Init (GameObject src, AuraAbility aura) {
        this.aura = aura;
        this.src = src;
    }
    
    void OnTriggerEnter2D (Collider2D collider) {
        aura.AddAuraEffects (src, collider.gameObject);
    }
    void OnTriggerStay2D (Collider2D collider) {
    }
    void OnTriggerExit2D (Collider2D collider) {
        aura.RemoveAuraEffects (src, collider.gameObject);
    }
}