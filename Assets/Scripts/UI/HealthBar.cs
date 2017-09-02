using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {
    public dfProgressBar bar;
    int currentValue;
    public PitchCharacter character;

    void Start () {
        bar = GetComponent<dfProgressBar>();
    }
    void Update () {
        if (currentValue != character.hitPoint.current)
        {
            currentValue = character.hitPoint.current;
            float percent = 100.0f * ((currentValue * 1.0f) / character.hitPoint.baseValue);
            bar.Value = percent;

            if (currentValue == 0) {
                GameObject.Destroy(gameObject);
            }
        }
    }
}
