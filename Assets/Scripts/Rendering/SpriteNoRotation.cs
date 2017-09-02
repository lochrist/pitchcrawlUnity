using UnityEngine;
using System.Collections;

/// <summary>
/// Make sure a sprite has no rotation applied
/// has no rotation.
/// </summary>
public class SpriteNoRotation : MonoBehaviour
{
    /// <summary>
    /// Reference to the target sprite to change position
    /// </summary>
    public SpriteRenderer m_Target;
    public bool isAwake;

    /// <summary>
    /// Link self to other objects.
    /// </summary>
    void Awake()
    {
        // Get the target sprite.
        m_Target = m_Target ? m_Target : GetComponent<SpriteRenderer>();
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
        // m_Target.transform.rotation = Quaternion.identity;
        isAwake = gameObject.rigidbody2D.IsAwake();
    }
}
