using UnityEngine;
using System.Collections;

/// <summary>
/// Behavior used to dynamically adjust the health bar sprite above character.
/// It makes sure that the health bar is always above the character and that it
/// has no rotation.
/// </summary>
public class StickyPlacement : MonoBehaviour
{
    SpriteRenderer m_ParentSprite;

    /// <summary>
    /// Reference to the target sprite to change position
    /// </summary>
    public SpriteRenderer m_Target;

    /// <summary>
    /// Adjust the vertical offset for the health bar placement.
    /// TODO: This could become bigger based on the scale of the objects
    /// </summary>
    public float kVerticalOffset = 0.2f;

    /// <summary>
    /// Link self to other objects.
    /// </summary>
    void Awake()
    {
        // Get the target sprite.
        m_Target = m_Target ? m_Target : GetComponent<SpriteRenderer>();

        // Get the parent sprite on which we will be placed
        m_ParentSprite = m_Target.transform.parent.gameObject.GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Initialize behavior
    /// </summary>
    void Start()
    {
    }

    /// <summary>
    /// Keep the target sprite atop of the target parent's sprite.
    /// </summary>
    void Update()
    {
        Bounds parentBounds = m_ParentSprite.bounds;
        m_Target.transform.rotation = Quaternion.identity;
        m_Target.transform.position = parentBounds.center + new Vector3(0, parentBounds.extents.y + kVerticalOffset, 0);
    }
}
