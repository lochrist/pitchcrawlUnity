using UnityEngine;
using System.Collections;

public class MonsterSpawnPoint : SpawnPoint
{
    public GameObject[] possibleMonsters;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject SelectTemplate()
    {
        return possibleMonsters[Random.Range(0, possibleMonsters.Length)];
    }

#if UNITY_EDITOR

    public override Texture GetGizmoTexture()
    {
        if (this.possibleMonsters.Length == 0)
            return null;

        SpriteRenderer spriteRenderer = this.possibleMonsters[0].GetComponent<SpriteRenderer>();

        if (!spriteRenderer)
            return null;

        return spriteRenderer.sprite.texture;
    }

#endif
}
