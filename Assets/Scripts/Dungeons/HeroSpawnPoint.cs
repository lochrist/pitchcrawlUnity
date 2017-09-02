using UnityEngine;
using System.Collections;

public class HeroSpawnPoint : SpawnPoint
{
    public GameObject template;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }


#if UNITY_EDITOR

    public override Texture GetGizmoTexture()
    {
        if (!this.template)
            return null;

        SpriteRenderer spriteRenderer = this.template.GetComponent<SpriteRenderer>();

        if (!spriteRenderer)
            return null;

        return spriteRenderer.sprite.texture;
    }

#endif
}
