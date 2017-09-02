using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum AttributeType
{
    // Attr
    Might,
    Speed,
    Intelligence,
    Charisma,

    // 
    Armor,
    Focus,
    HitPoint,

    NbAttributes,
    None
}

[System.Serializable]
public class Attribute
{
    public Attribute(AttributeType type, int baseValue)
    {
        this.attributeType = type;
        this.baseValue = baseValue;
        this.current = baseValue;
    }
    public int current;
    public int baseValue;
    public int modifier;
    [HideInInspector]
    public AttributeType attributeType;

    public void Modify(ModifierType modType, int modValue)
    {
        switch (modType)
        {
            case ModifierType.Add:
                current += modValue;
                break;
            case ModifierType.Remove:
                current -= modValue;
                break;
            case ModifierType.SetTo:
                current = modValue;
                break;
        }
        current = Utils.Clamp(current, 0, baseValue);
    }

    public string Name()
    {
        return attributeType.ToString();
    }
}

public class PitchCharacter : MonoBehaviour
{
    public Attribute armor = new Attribute(AttributeType.Armor, 0);
    public Attribute hitPoint = new Attribute(AttributeType.HitPoint, 5);
    public Attribute focus = new Attribute(AttributeType.Focus, 0);
    public Attribute might = new Attribute(AttributeType.Might, 1);
    public Attribute speed = new Attribute(AttributeType.Speed, 1);
    public Attribute intelligence = new Attribute(AttributeType.Intelligence, 1);
    public Attribute charisma = new Attribute(AttributeType.Charisma, 1);

    public bool isSelected;
    public bool isSelectable;
    public bool hasActed;
    public bool isHero;

    public List<AbilityDescriptor> actionAbilities = new List<AbilityDescriptor>();
    public List<AbilityDescriptor> actionSequence = new List<AbilityDescriptor>();
    public int actionSequenceIndex;
    public AbilityDescriptor currentAbility;
    public AbilityDescriptor currentSequenceAbility;
    public BaseAction currentAction;
    public bool inActionExecution;


    public List<AbilityContainer> startingGear;
    public Dictionary<AbilityUsageType, List<AbilityDescriptor>> triggerToAbilitiesMap = new Dictionary<AbilityUsageType, List<AbilityDescriptor>>();
    public List<Skill> skills = new List<Skill>();
    public List<Item> items = new List<Item>();
    public List<Trait> traits = new List<Trait>();

    public List<EffectDescriptor> currentEffects = new List<EffectDescriptor>();
    public List<EffectDescriptor> toRemoveEffects = new List<EffectDescriptor>();

    public AbilityDescriptor meleeAttack;
    public AbilityDescriptor rangedAttack;
    public AbilityDescriptor move;

    GameObject selectableMarker;
    Color defaultSelectionMakerColor;
    SpriteRenderer healthBar;
    Vector3 healthScale;

    // Use this for initialization
    public virtual void Awake()
    {
        selectableMarker = transform.Find("Selectable").gameObject;
        defaultSelectionMakerColor = selectableMarker.GetComponent<SpriteRenderer>().color;

        isHero = GetComponent<Hero>() != null;
    }

    public void Start()
    {
        // Add Default Actions :
        EquipSkill("Melee Attack");
        EquipSkill("Ranged Attack");
        EquipSkill("Move");

        meleeAttack = FindActionAbility("Melee Attack");
        rangedAttack = FindActionAbility("Ranged Attack");
        move = FindActionAbility("Move");

        for (int i = 0; i < startingGear.Count; ++i)
        {
            EquipStartingGear(startingGear[i]);
        }

        UIRoot.Instance.CreateHealthBar(this);
    }

    public void Destroy () {
        UIRoot.Instance.DestroyHealthBar(this);
    }
    //////////////////////////////////////////////////////////////////////////////

    public virtual bool OnClickDown(Vector3 mouseWorldPos)
    {
        if (currentAction != null)
        {
            return currentAction.OnClickDown(mouseWorldPos);
        }
        return false;
    }
    public virtual void OnClickDrag(Vector3 mouseWorldPos)
    {
        if (currentAction != null)
        {
            currentAction.OnClickDrag(mouseWorldPos);
        }
    }
    public virtual void OnClickUp(Vector3 mouseWorldPos)
    {
        if (currentAction != null)
        {
            currentAction.OnClickUp(mouseWorldPos);
        }
    }

    //////////////////////////////////////////////////////////////////////////////

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        // Check for triggers abilities
        ICollisionHandler handler = currentAction as ICollisionHandler;
        if (handler != null)
        {
            handler.HandleCollisionEnter2D(collision);
        }
    }
    public virtual void OnCollisionStay2D(Collision2D collision)
    {

    }
    public virtual void OnCollisionExit2D(Collision2D collision)
    {

    }
    public virtual void OnWorldStabilize()
    {
        if (currentAction != null)
        {
            currentAction.OnWorldStabilize();
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    /// 
    public Attribute Attribute(AttributeType attr)
    {
        Attribute a = null;
        switch (attr)
        {
            case AttributeType.Armor:
                a = armor;
                break;
            case AttributeType.Charisma:
                a = charisma;
                break;
            case AttributeType.Focus:
                a = focus;
                break;
            case AttributeType.HitPoint:
                a = hitPoint;
                break;
            case AttributeType.Intelligence:
                a = intelligence;
                break;
            case AttributeType.Might:
                a = might;
                break;
            case AttributeType.Speed:
                a = speed;
                break;
        }
        return a;
    }

    public void GetEffects(EffectType effectType, List<EffectDescriptor> effects)
    {
        for (int i = 0; i < currentEffects.Count; ++i)
        {
            if (currentEffects[i].effectType == effectType)
            {
                effects.Add(effects[i]);
            }
        }
    }

    public void GetEffects<T>(List<T> effects) where T : EffectDescriptor
    {
        for (int i = 0; i < currentEffects.Count; ++i)
        {
            T e = currentEffects[i] as T;
            if (e)
            {
                effects.Add(e);
            }
        }
    }

    public void GetEffects<T>(List<EffectDescriptor> effects) where T : EffectDescriptor
    {
        for (int i = 0; i < currentEffects.Count; ++i)
        {
            T e = currentEffects[i] as T;
            if (e)
            {
                effects.Add(e);
            }
        }
    }

    public void TriggerAbilities(AbilityUsageType triggerType, GameObject src, GameObject target, BaseAction context)
    {
        List<AbilityDescriptor> abilities = null;
        if (triggerToAbilitiesMap.TryGetValue(triggerType, out abilities))
        {
            for (int i = 0; i < abilities.Count; ++i)
            {
                abilities[i].ActivateAbility(src, target, context);
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    ///  Action managing stuff
    public AbilityDescriptor FindActionAbility(string name)
    {
        var ability = this.actionAbilities.Find(a => a.Name == name);
        return ability;
    }

    public void SetCurrentAbility(AbilityDescriptor ability)
    {
        if (ability == currentAbility)
        {
            return;
        }

        if (currentAbility != null)
        {
            actionSequence.Clear();
            if (currentAction)
            {
                currentAction.CancelAction();
            }
        }

        currentAbility = ability;

        if (currentAbility != null)
        {
            InitActionSequence(currentAbility);
        }
    }

    public void InitActionSequence(AbilityDescriptor desc)
    {

        // define action sequence.
        actionSequenceIndex = 0;
        actionSequence.Clear();

        if (desc.abilityType == AbilityType.Multi)
        {
            MultiAbility ability = desc as MultiAbility;
            for (int i = 0; i < ability.abilities.Count; ++i)
            {
                AbilityDescriptor subAbility = ability.abilities[i].ability;
                AddAbilityToSequence(subAbility);
            }
        }
        else
        {
            AddAbilityToSequence(desc);
        }
        actionSequenceIndex = 0;
        BindAbilityToAction(actionSequenceIndex);
    }

    public void BindAbilityToAction(int sequenceIndex)
    {
        if (currentAction)
        {
            currentAction.enabled = false;
        }

        currentSequenceAbility = actionSequence[sequenceIndex];
        string actionHandlerName = currentSequenceAbility.abilityType.ToString() + "Action";
        currentAction = gameObject.GetComponent(actionHandlerName) as BaseAction;
        if (!currentAction)
        {
            currentAction = gameObject.AddComponent(actionHandlerName) as BaseAction;
            if (!currentAction)
            {
                throw new UnityException("No action with name: " + actionHandlerName);
            }
        }
        currentAction.Init(currentSequenceAbility);
        currentAction.enabled = true;
        currentAction.Reset();
    }

    public void OnStartAction()
    {
        // Cancel of action is not available : in action execution
        inActionExecution = true;
    }

    public void OnEndAction(bool cancelled)
    {
        // checked next action in sequence
        ++actionSequenceIndex;
        if (!cancelled && actionSequenceIndex < actionSequence.Count)
        {
            BindAbilityToAction(actionSequenceIndex);
        }
        else
        {
            hasActed = !cancelled;
            currentAction.enabled = false;
            currentAction = null;

            if (!cancelled)
            {
                if (currentAbility.container.usageType == DurationType.UntilXUsage)
                {
                    --currentAbility.container.nbOfUses;
                    if (currentAbility.container.nbOfUses == 0 && currentAbility.container.destroyOnLastUse)
                    {
                        Item item = currentAbility.container as Item;
                        if (item)
                        {
                            UnequipItem(item);
                        }
                        else
                        {
                            Skill skill = currentAbility.container as Skill;
                            if (skill)
                            {
                                UnequipSkill(skill);
                            }
                        }
                    }
                }
            }

            UpdateSelectableMarker();
            GameManager.Instance.OnEndAction(cancelled);
        }
    }

    void AddAbilityToSequence(AbilityDescriptor ability)
    {
        int nbActivation = ability.nbActivation;
        if (ability.abilityType == AbilityType.Reference)
        {
            ReferenceAbility reference = ability as ReferenceAbility;
            ability = reference.ResolveReference(this);

            if (ability == null)
            {
                throw new UnityException("Cannot resolved reference ability: " + ability);
            }
        }

        for (int i = 0; i < nbActivation; ++i)
        {
            actionSequence.Add(ability);
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    public void Select()
    {
        isSelected = true;
        SetCurrentAbility(actionAbilities[0]);
        UpdateSelectableMarker();
    }

    public void Unselect()
    {
        isSelected = false;
        SetCurrentAbility(null);
        UpdateSelectableMarker();
    }

    public void SetSelectable(bool isSelectable)
    {
        this.isSelectable = isSelectable;
        UpdateSelectableMarker();
    }



    //////////////////////////////////////////////////////////////////////////////
    public void EquipSkill(string name)
    {
        Skill skill = ResourceMgr.Load<Skill>("Skills/" + name, true);
        if (!skill)
        {
            throw new UnityException("Skill not found: " + name);
        }

        EquipSkill(skill);
    }

    public void EquipSkill(Skill skill)
    {
        if (!skill)
        {
            throw new UnityException("Skill cannot be null");
        }

        skills.Add(skill);
        Equip(skill);
    }

    public void UnequipSkill(Skill skill)
    {
        if (!skill)
        {
            throw new UnityException("Skill cannot be null");
        }

        skills.Remove(skill);
        Unequip(skill);
    }

    public void EquipItem(string itemName)
    {
        Item item = ResourceMgr.Load<Item>("Items/" + itemName, true);
        if (!item)
        {
            throw new UnityException("Item not found: " + itemName);
        }

        EquipItem(item);
    }

    public void EquipItem(Item item)
    {
        if (!item)
        {
            throw new UnityException("Item cannot be null");
        }

        items.Add(item);
        Equip(item);
    }

    public void UnequipItem(Item item)
    {
        if (!item)
        {
            throw new UnityException("Item cannot be null");
        }

        items.Remove(item);
        Unequip(item);
    }

    protected void EquipStartingGear(AbilityContainer container)
    {
        Item item = container as Item;
        if (item)
        {
            EquipItem(item.Clone() as Item);
        }
        else
        {
            Skill skill = container as Skill;
            EquipSkill(skill.Clone() as Skill);
        }
    }

    protected void Equip(AbilityContainer container)
    {
        for (int i = 0; i < container.abilities.Count; ++i)
        {
            AbilityDescriptor adesc = container.abilities[i];
            adesc.BindToContainer(container);

            if (adesc.usageType == AbilityUsageType.Action)
            {
                actionAbilities.Add(adesc);
            }
            else if (adesc.usageType == AbilityUsageType.OnTargetEquips)
            {
                BaseAction dummyAction = null;
                adesc.ActivateAbility(gameObject, gameObject, dummyAction);
            }
            else
            {
                List<AbilityDescriptor> abilities;
                if (!triggerToAbilitiesMap.TryGetValue(adesc.usageType, out abilities))
                {
                    abilities = new List<AbilityDescriptor>();
                    triggerToAbilitiesMap[adesc.usageType] = abilities;
                }
                abilities.Add(adesc);
            }
        }
    }

    protected void Unequip(AbilityContainer container)
    {
        for (int i = 0; i < container.abilities.Count; ++i)
        {
            AbilityDescriptor adesc = container.abilities[i];
            if (adesc.usageType == AbilityUsageType.Action)
            {
                actionAbilities.Remove(adesc);
            }
            else
            {
                triggerToAbilitiesMap[adesc.usageType].Remove(adesc);
            }
        }
    }

    public void AddEffect(EffectDescriptor desc)
    {
        currentEffects.Add(desc);
    }

    public void RemoveEffect(EffectDescriptor effect)
    {
        currentEffects.Remove(effect);
    }

    //////////////////////////////////////////////////////////////////////////////
    /// Game phase handler

    public void OnStartTurn()
    {
        hasActed = false;
        UpdateSelectableMarker();
    }

    public void OnEndTurn()
    {
        Utils.Log("Character: ", name, "turn ended");

        toRemoveEffects.Clear();

        foreach (var e in currentEffects)
        {
            if (e.durationType == DurationType.UntilXTurn)
            {
                e.duration -= 1;
                if (e.duration == 0)
                {
                    toRemoveEffects.Add(e);
                }
            }
        }

        foreach (var e in toRemoveEffects)
        {
            e.UndoEffect(gameObject);
        }
    }

    //////////////////////////////////////////////////////////////////////////////


    void UpdateSelectableMarker()
    {
        selectableMarker.SetActive(true);
        if (isSelected)
        {
            selectableMarker.GetComponent<SpriteRenderer>().color = new Color(0.0f, 1.0f, 0.0f);
        }
        else
        {
            selectableMarker.GetComponent<SpriteRenderer>().color = defaultSelectionMakerColor;
        }
    }
}