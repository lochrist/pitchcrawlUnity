using UnityEngine;
using System.Collections;

public class ActionSlot : MonoBehaviour {
    public AbilityDescriptor ability;
    public dfTextureSprite icon;
    public dfSprite border;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (GameManager.Instance.currentCharacter && GameManager.Instance.currentCharacter.currentAbility == ability)
        {
            border.Color = Color.green;
            icon.Color = Color.green;
        } else
        {
            border.Color = Color.white;
            icon.Color = Color.white;
        }
	}

    void OnClick() {
        if (GameManager.Instance.currentCharacter)
        {
            GameManager.Instance.currentCharacter.SetCurrentAbility(ability);
        }
    }

    // =====================================

    public void BindAction(AbilityDescriptor ability) {
        this.ability = ability;
        gameObject.name = ability.Name;
        icon = GetComponentInChildren<dfTextureSprite>();
        icon.Texture = ability.Icon;
        border = GetComponent<dfSprite>();
    }
}
