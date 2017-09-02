using UnityEngine;
using System.Collections;

public class DungeonSetup : MonoBehaviour
{
    private HeroSpawnPoint[] m_heroSpawnPoints;
    private MonsterSpawnPoint[] m_monsterSpawnPoints;

    void Awake()
    {
        // Get all slots
        m_heroSpawnPoints = GameObject.FindObjectsOfType<HeroSpawnPoint>();
        m_monsterSpawnPoints = GameObject.FindObjectsOfType<MonsterSpawnPoint>();
    }

    // Use this for initialization
    void Start()
    {
        // For each hero slot instanciate an hero
        // TODO: Grab party formation and spawn here based on that.
        // TODO: Let the user chooses where to spawn heroes.
        foreach (HeroSpawnPoint heroSpawnPoint in m_heroSpawnPoints)
        {
            // Spawn hero
            Instantiate(
                heroSpawnPoint.template, 
                heroSpawnPoint.transform.position, 
                heroSpawnPoint.transform.rotation);

            Object.Destroy(heroSpawnPoint);
        }

        // Spawn monsters based on difficulty.
        foreach (MonsterSpawnPoint monsterSpawnPoint in m_monsterSpawnPoints)
        {
            // Spawn hero
            Instantiate(
                monsterSpawnPoint.SelectTemplate(),
                monsterSpawnPoint.transform.position,
                monsterSpawnPoint.transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
