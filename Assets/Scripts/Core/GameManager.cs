using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public bool freeformTurnOrder = false;
    public int currentTurn = 0;
    public bool isHeroTurn = true;
    public bool isTurnActive = false;

    public bool characterEvent = false;
    public bool uiEvent = false;

    public static GameManager Instance;

    List<Rigidbody2D> bodies = new List<Rigidbody2D>();
    List<PitchCharacter> characters = new List<PitchCharacter>();
    List<Hero> heroes = new List<Hero>();
    List<Monster> monsters = new List<Monster>();

    public GameObject uiRoot;
    public dfGUIManager guiManager;
    public CharacterBar characterBar;

    public PitchCharacter currentCharacter;
    public bool allCharactersAsleep = false;
    public bool trackMovingBody = false;

    public CameraControl camControl;


    // Use this for initialization
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        camControl = GetComponentInChildren<CameraControl>();

        uiRoot = GameObject.Find("UIRoot");
        guiManager = uiRoot.GetComponent<dfGUIManager>();
        characterBar = uiRoot.GetComponentInChildren<CharacterBar>();


        PitchCharacter[] characterArray = FindObjectsOfType(typeof(PitchCharacter)) as PitchCharacter[];
        characters.AddRange(characterArray);

        foreach (PitchCharacter c in characterArray)
        {
            Hero hero = c as Hero;
            if (hero)
            {
                heroes.Add(hero);
            }
            else
            {
                Monster monster = c as Monster;
                if (monster)
                {
                    monsters.Add(monster);
                }
            }
        }

        StartTurn();
    }
    // Update is called once per frame
    void Update()
    {
        if (trackMovingBody)
        {
            if (CheckAllBodiesAsleep() != allCharactersAsleep)
            {
                allCharactersAsleep = !allCharactersAsleep;

                if (allCharactersAsleep)
                {
                    trackMovingBody = false;
                    camControl.target = null;

                    if (currentCharacter)
                    {
                        currentCharacter.OnWorldStabilize();
                    }

                    SetHeroTurn(isHeroTurn);
                }
                else
                {
                    SetCharactersSelectable(false);
                }
            }
        }

        ForwardInputToCharacter();

        characterBar.Show(allCharactersAsleep && currentCharacter);

        if (CheckEndGame())
        {
            EndGame();

        }
        else if (!isTurnActive)
        {
            StartTurn();
        }
        else if (CheckEndTurn())
        {
            EndTurn();
        }
    }

    /////////////////////////////////////
    bool CheckAllBodiesAsleep()
    {
        bool areAllAsleep = true;
        for (int i = 0; i < characters.Count; ++i)
        {
            var hero = characters[i].gameObject;
            if (hero.rigidbody2D.IsAwake())
            {
                areAllAsleep = false;
                break;
            }
        }

        if (areAllAsleep)
        {
            for (int i = 0; i < bodies.Count; ++i)
            {
                if (bodies[i].IsAwake())
                {
                    areAllAsleep = false;
                    break;
                }
            }
        }

        return areAllAsleep;
    }

    /// //////////////////////////////////
    public void SelectCharacter(PitchCharacter c)
    {
        if (c == currentCharacter)
            return;

        if (currentCharacter)
        {
            currentCharacter.Unselect();
        }
        currentCharacter = c;
        if (currentCharacter)
        {
            currentCharacter.Select();
        }
    }

    public void OnEndAction(bool cancelled)
    {
        if (!cancelled)
        {
            SelectCharacter(null);
            SetHeroTurn(!isHeroTurn);
        }
    }

    public void KillCharacter(PitchCharacter c)
    {
        if (currentCharacter == c)
        {
            SelectCharacter(null);
        }

        characters.Remove(c);
        if (c.isHero)
        {
            heroes.Remove(c as Hero);
        }
        else
        {
            monsters.Remove(c as Monster);
        }

        Object.Destroy(c.gameObject);
    }

    // Body won't be counter in all bodies asleep
    public void RemoveBody(GameObject objWithBody)
    {
        if (objWithBody.rigidbody2D)
        {
            bodies.Remove(objWithBody.rigidbody2D);
        }
    }

    public void FollowBody(GameObject obj)
    {
        camControl.target = obj.transform;
    }

    public void AddBody(GameObject objWithBody, bool forceSleeping = false)
    {
        if (objWithBody.rigidbody2D && objWithBody.GetComponent<PitchCharacter>() == null && !bodies.Contains(objWithBody.rigidbody2D))
        {
            if (forceSleeping)
            {
                objWithBody.rigidbody2D.Sleep();
            }
            bodies.Add(objWithBody.rigidbody2D);
        }
    }

    public GameObject InstantiateBody(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        var objWithBody = GameObject.Instantiate(prefab, position, rotation) as GameObject;
        AddBody(objWithBody, true);
        return objWithBody;
    }

    public void DestroyBody(GameObject objWithBody)
    {
        RemoveBody(objWithBody);
        GameObject.Destroy(objWithBody);
    }

    /////////////////////////////////////

    bool IsMouseOnGUI(Vector3 mousePos)
    {
        return guiManager.HitTest(mousePos);
    }

    void ForwardInputToCharacter()
    {
        if (allCharactersAsleep)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (IsMouseOnGUI(Input.mousePosition))
                {
                    // Click in action bar
                    Utils.Log("On ui!");
                }
                else
                {
                    var worldMouse = Utils.MouseToWorldCoord(Input.mousePosition);
                    if (currentCharacter && currentCharacter.OnClickDown(worldMouse))
                    {
                        // Character has accepted the event.
                        characterEvent = true;
                    }
                    else
                    {
                        PitchCharacter character = Utils.FindObjectUnderPoint<PitchCharacter>(worldMouse);
                        if (character && character.isSelectable)
                        {
                            SelectCharacter(character);
                        }
                        else
                        {
                            SelectCharacter(null);
                        }
                    }
                }
            }
            else if (characterEvent)
            {
                if (Input.GetMouseButton(0))
                {
                    if (currentCharacter)
                    {
                        currentCharacter.OnClickDrag(Utils.MouseToWorldCoord(Input.mousePosition));
                    }

                }
                else if (Input.GetMouseButtonUp(0))
                {
                    if (currentCharacter)
                    {
                        currentCharacter.OnClickUp(Utils.MouseToWorldCoord(Input.mousePosition));
                    }
                    characterEvent = false;
                }
            }
        }
        else if (characterEvent && Input.GetMouseButtonUp(0))
        {
            characterEvent = false;
        }
    }

    void SetCharactersSelectable(bool selectable, List<PitchCharacter> characterList = null)
    {
        if (characterList == null)
        {
            foreach (PitchCharacter c in characters)
            {
                c.SetSelectable(selectable);
            }
        }
        else
        {
            foreach (var c in characterList)
            {
                c.SetSelectable(selectable);
            }
        }
    }

    void SetHeroTurn(bool isHeroTurn)
    {

        if (isHeroTurn)
        {
            isHeroTurn = heroes.Find(c => !c.hasActed) != null;
        }
        else
        {
            isHeroTurn = monsters.Find(c => !c.hasActed) == null;
        }


        this.isHeroTurn = isHeroTurn;

        if (freeformTurnOrder)
        {
            SetCharactersSelectable(true);
        }
        else
        {
            foreach (PitchCharacter c in heroes)
            {
                c.SetSelectable(isHeroTurn && !c.hasActed);
            }

            foreach (PitchCharacter c in monsters)
            {
                c.SetSelectable(!isHeroTurn && !c.hasActed);
            }
        }
    }

    bool CheckEndGame()
    {
        return (heroes.Count == 0 || monsters.Count == 0);
    }

    bool CheckEndTurn()
    {
        return characters.Find(character => !character.hasActed) == null;
    }

    void EndTurn()
    {
        //Reset all characters:
        foreach (var c in characters)
        {
            c.OnEndTurn();
        }
        isTurnActive = false;
    }

    void EndGame()
    {

    }

    void StartTurn()
    {
        isTurnActive = true;
        currentTurn++;
        foreach (var c in characters)
        {
            c.OnStartTurn();
        }

        SetHeroTurn(true);
    }
}
