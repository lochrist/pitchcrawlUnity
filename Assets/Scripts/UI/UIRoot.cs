using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIRoot : MonoBehaviour {
    public GameObject healthBarPrefab;
    public GameObject dmgMsgPrefab;
    public dfGUIManager guiManager;

    public List<dfLabel> dmgMsgs;

    public static UIRoot Instance;

    // Use this for initialization
    void Awake ()
    {
        Instance = this;
        guiManager = GetComponent<dfGUIManager>();
    }

	void Start () {

	}
	
	void Update () {
	
	}

    // =============================================
    Comp Spawn<Comp>(GameObject prefab, string name = "") where Comp : UnityEngine.Component {
        var obj = guiManager.AddPrefab(prefab);
        if (name.Length > 0)
        {
            obj.gameObject.name = name;
        }

        Comp comp = obj as Comp;
        if (!comp)
        {
            comp = obj.GetComponent<Comp>();
        }
        return comp;
    }
      

    public void CreateHealthBar(PitchCharacter c) {

        var follow = Spawn<dfFollowObject>(healthBarPrefab, c.name + "_HealthBar");
        follow.attach = c.gameObject;
        follow.enabled = true;

        var healthbar = follow.GetComponent<HealthBar>();
        healthbar.character = c;

        var label = healthbar.GetComponentInChildren<dfLabel>();
        label.Text = c.name;
    }

    public void DestroyHealthBar(PitchCharacter c) {
        dfFollowObject[] healthbars = GetComponentsInChildren<dfFollowObject>();
        for (int  i = 0; i < healthbars.Length; ++i)
        {
            if (healthbars[i].attach == c.gameObject) {
                GameObject.Destroy(healthbars[i]);
            }
        }
    }

    public void SpawnDmgMsg(int dmg, PitchCharacter c) {
        Vector2 guiPos = guiManager.WorldPointToGUI(c.transform.position);
        dfLabel msg = null;
        for (int i = 0; i < dmgMsgs.Count; ++i)
        {
            if (dmgMsgs[i].Opacity == 0.0f) {
                msg = dmgMsgs[i];
            }
        }

        if (msg == null)
        {
            msg = Spawn<dfLabel>(dmgMsgPrefab);
            dmgMsgs.Add(msg);
        }

        msg.Text = dmg.ToString();
        msg.RelativePosition = guiPos;
        if (c == GameManager.Instance.currentCharacter)
        {
            msg.Color = Color.red;
        } else
        {
            msg.Color = Color.white;
        }

        msg.GetComponent<dfTweenPlayableBase>().Play();
    }
}
