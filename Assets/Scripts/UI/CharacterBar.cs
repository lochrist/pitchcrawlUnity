using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterBar : MonoBehaviour {

    public PitchCharacter currentCharacter;
    public GameObject actionSlotPrefab;

    public dfControl characterBar;
    public dfControl actionBar;
    public dfSlider focusBar;
    public dfSlider hpBar;
    public dfLabel message;
    public dfButton okButton;
    public dfTweenPlayableBase slideIn;
    public dfTweenPlayableBase slideOut;
    bool isIn = false;
    // public 
    
    // Use this for initialization
    void Start () {
        characterBar = GetComponent<dfControl>();
        actionBar = GameObject.Find("ActionBar").GetComponent<dfControl>();
        focusBar = GameObject.Find("Status/EnergyBar").GetComponent<dfSlider>();
        hpBar = GameObject.Find("Status/LifeBar").GetComponent<dfSlider>();
        message = GameObject.Find("Message").GetComponent<dfLabel>();
        okButton = GameObject.Find("OkButton").GetComponent<dfButton>();

        dfTweenPlayableBase[] tweens = GetComponents<dfTweenPlayableBase>();
        for (int i = 0; i < tweens.Length; ++i)
        {
            string name = tweens[i].TweenName;
            if ("SlideIn" == name) {
                slideIn = tweens[i];
            } else if ("SlideOut" == name) {
                slideOut = tweens[i];
            }
        }

        // TODO: Fix the top of the character bar based on the screen resolution when sliding in
    }
    
    // Update is called once per frame
    void Update () {
        if (currentCharacter)
        {
            focusBar.Value = currentCharacter.focus.current;
            hpBar.Value = currentCharacter.hitPoint.current;
        }
    }

    // ======================================
    public void Show(bool show) {
        if (isIn == show)
        {
            return;
        }

        if (!show)
        {
            slideOut.Play();
            // characterBar.IsVisible = false;
            isIn = false;
            SetCharacter(null);
        } else
        {

            if (currentCharacter != GameManager.Instance.currentCharacter) {
                SetCharacter(GameManager.Instance.currentCharacter);
            }
            //characterBar.IsVisible = true;
            isIn = true;
            slideIn.Play();
        }
    }

    public void SetMessage(string msg) {
        message.Text = msg;
    }

    public void ShowOkButton(MouseEventHandler handler) {
        okButton.IsVisible = true;
        okButton.Click += handler;
    }

    public void SetEnableOkButton(bool enable) {
        okButton.IsEnabled = enable;
    }

    public void HideOkButton(MouseEventHandler handler) {
        okButton.IsVisible = false;
        okButton.Click -= handler;
    }

    // ======================================
    void SetCharacter(PitchCharacter character) {
        List<GameObject> toRemove = new List<GameObject>();
        for (int i = 0; i < actionBar.Controls.Count; ++i) {
            toRemove.Add(actionBar.Controls[i].gameObject);
        }
        for (int i = 0; i < toRemove.Count; ++i) {
            GameObject.DestroyImmediate(toRemove[i]);
        }

        currentCharacter = character;

        if (character)
        {
            hpBar.MaxValue = currentCharacter.hitPoint.baseValue;
            focusBar.MaxValue = System.Math.Max(1, currentCharacter.focus.baseValue);
            focusBar.IsVisible = currentCharacter.focus.baseValue > 0;

            for (int i = 0; i < currentCharacter.actionAbilities.Count; ++i) {
                var ability = currentCharacter.actionAbilities[i];
                dfControl actionSlotControl = actionBar.AddPrefab(actionSlotPrefab);
                var actionSlot = actionSlotControl.gameObject.GetComponent<ActionSlot>();
                actionSlot.BindAction(ability);
            }
        }
    }
}
