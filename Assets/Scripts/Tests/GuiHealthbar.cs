using UnityEngine;
using System.Collections;

public class GuiHealthbar : MonoBehaviour
{
    public float timeCoef = 3.5f;
    public float timeDisplay; //current progress
    public float dragDisplay; //current progress
    public Vector2 pos = new Vector2(20,40);
    public Vector2 size = new Vector2(100,20);
    public Texture2D emptyTex;
    public Texture2D greenTex;
    public Texture2D redTex;

    float startTime;
    bool isDragging = false;
    Vector3 startDrag;

    public float timeSizeX;
    public float dragX;
    
    void OnGUI() {

        GUI.BeginGroup(new Rect(pos.x, pos.y, size.x, size.y));
            GUI.DrawTexture(new Rect(0,0, size.x, size.y), emptyTex);

            float dragSizeX = size.x * dragDisplay;
            GUI.BeginGroup(new Rect(0, 0, dragSizeX, size.y));
                GUI.DrawTexture(new Rect(0,0, size.x, size.y), greenTex);
            GUI.EndGroup();

        //draw the filled-in part:
            float timeX = size.x - (size.x * timeDisplay);
            GUI.BeginGroup(new Rect(timeX,0, size.x, size.y));
                GUI.DrawTexture(new Rect(0,0, size.x, size.y), redTex);
            GUI.EndGroup();

        GUI.EndGroup();
    }
    
    void Update() {
        //for this example, the bar display is linked to the current time,
        //however you would set this value based on your desired display
        //eg, the loading progress, the player's health, or whatever.

        if (Input.GetMouseButtonDown (0)) {
            startTime = Time.time;
            startDrag = Input.mousePosition;
            isDragging = true;
        } else if (isDragging) {
            timeDisplay = System.Math.Min ((Time.time - startTime) * timeCoef, 1.0f);

            if (Input.GetMouseButton(0) || Input.GetMouseButtonUp(0)) {
                Vector2 endpos = Input.mousePosition - startDrag;
                var length = System.Math.Min (endpos.magnitude, size.x);
                dragDisplay = System.Math.Min (length / size.x, 1-timeDisplay);
                isDragging = !Input.GetMouseButtonUp(0);
            }

            if (dragDisplay + timeDisplay > 0.999f) {
                isDragging = false;
            }
        }
    }
          
}

