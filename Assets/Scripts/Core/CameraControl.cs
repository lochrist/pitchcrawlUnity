using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{

    public Vector3 originalPos;
    public float originalSize;
    public Camera cam;
    public float minZoom = 1.0f;
    public float maxZoom = 3.0f;
    public float zoomFactor = 1.0f;
    public float panSpeed = 8;
    public float zoomSpeed = 2.5f;
    public float deltaT;
    public float UnitsPerPixel = 1f / 100f;
    public float zoomTransitionDuration = 0.3f;

    public float xMargin = 1f;      // Distance in the x axis the player can move before the camera follows.
    public float yMargin = 1f;      // Distance in the y axis the player can move before the camera follows.
    public float xSmooth = 4f;      // How smoothly the camera catches up with it's target movement in the x axis.
    public float ySmooth = 4f;      // How smoothly the camera catches up with it's target movement in the y axis.
    public Vector2 maxXAndY;        // The maximum x and y coordinates the camera can have.
    public Vector2 minXAndY;        // The minimum x and y coordinates the camera can have.

    public Transform target;       // Reference to the player's transform.

    public float smoothTime = 0.3f;
    private float yVelocity = 0.0f;
    private float xVelocity = 0.0f;
    private float targetOrthographicSize;
    private float zoomTransitionStartTime;
    private float startOrthographicSize;

    // Use this for initialization
    void Start()
    {
        cam = gameObject.GetComponent<Camera>();
        originalPos = transform.position;
        originalSize = cam.orthographicSize;

        cam.orthographicSize = startOrthographicSize = targetOrthographicSize = Screen.height / 2f * UnitsPerPixel * zoomFactor;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 1)
        {
            deltaT = Time.deltaTime;
        }
        else if (Time.timeScale > 1)
        {
            deltaT = Time.deltaTime / Time.timeScale;
        }
        else
        {
            deltaT = 0.015f;
        }

        if (Input.GetButton("Horizontal"))
        {
            transform.Translate(Vector3.right * panSpeed * deltaT * Input.GetAxisRaw("Horizontal"));
        }

        if (Input.GetButton("Vertical"))
        {
            transform.Translate(Vector3.up * panSpeed * deltaT * Input.GetAxisRaw("Vertical"));
        }

        float mouseScroll = Input.GetAxis("Mouse ScrollWheel");
        if (mouseScroll != 0.0f)
        {
            Debug.Log("Zooming...");
            zoomFactor -= zoomSpeed * mouseScroll;
            zoomFactor = Mathf.Clamp(zoomFactor, minZoom, maxZoom);

            if (Mathf.Approximately(targetOrthographicSize, cam.orthographicSize))
            {
                zoomTransitionStartTime = Time.time;
                startOrthographicSize = cam.orthographicSize;
            }
        }

        // Transition to next ortho size
        // Set pixel perfect size.
        targetOrthographicSize = Screen.height / 2f * UnitsPerPixel * zoomFactor;        
    }

    bool CheckXMargin()
    {
        // Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
        return Mathf.Abs(transform.position.x - target.position.x) > xMargin;
    }


    bool CheckYMargin()
    {
        // Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
        return Mathf.Abs(transform.position.y - target.position.y) > yMargin;
    }

    void FixedUpdate()
    {
        if (target)
        {
            TrackTarget();
        }

        if (!Mathf.Approximately(targetOrthographicSize, cam.orthographicSize))
        {
            // Apply smooth transition
            float t = (Time.time - zoomTransitionStartTime) / zoomTransitionDuration;
            cam.orthographicSize = Mathf.SmoothStep(startOrthographicSize, targetOrthographicSize, t);
        }
    }

    void TrackTarget()
    {
        float targetX = target.position.x;
        float targetY = target.position.y;

        targetX = Mathf.SmoothDamp(transform.position.x, targetX, ref xVelocity, smoothTime);
        targetY = Mathf.SmoothDamp(transform.position.y, targetY, ref yVelocity, smoothTime);

        // The target x and y coordinates should not be larger than the maximum or smaller than the minimum.
        // targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
        // targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);

        transform.position = new Vector3(targetX, targetY, transform.position.z);
    }

    private float Sigmoid(float x)
    {
        float expValue;
        float result;

        // Exponential calculation
        expValue = Mathf.Exp(-x);

        // Final sigmoid value
        result = 1.0f / (1.0f + expValue);

        return result;
    }
}
