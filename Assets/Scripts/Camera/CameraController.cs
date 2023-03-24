using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Variables

    [Header("Camera Variables")]
    public GameObject player;
    [Space()]
    public static CameraController instance;
    public CameraBounds bounds;
    Camera cam;

    internal float shakeMagnitude, shakeDuration;

    [Header("https://i.kym-cdn.com/entries/icons/original/000/023/977/cover3.jpg")]
    Vector2 targetPos, newPos, velocity;
    Vector3 initialPos;

    #endregion

    // Before start
    private void Awake()
    {
        targetPos = transform.position;
        instance = this;
        cam = this.GetComponent<Camera>();
        initialPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeDuration > 0)
        {
            transform.localPosition = initialPos + Random.insideUnitSphere * shakeMagnitude;

            shakeDuration -= Time.deltaTime;
        }
        else
        {
            shakeDuration = 0f;
            initialPos = transform.position;
        }

        // Make sure to follow player if in the bounds, else just follow them
        if (bounds)
        {
            targetPos = bounds.transform.position;

            if (bounds.followPlayer)
            {
                targetPos = player.transform.position;
            }

            // Camera size change 
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, bounds.desiredSize, bounds.transitionScaleTime * Time.deltaTime);
        }
        else
        {
            targetPos = player.transform.position;
        }

        // This is the part which applys the smoothing
        if (bounds != null)
        {
            newPos = Vector2.SmoothDamp(newPos, targetPos, ref velocity, bounds.transitionTime);
        }

        if (shakeDuration <= 0)
        {
            // Apply the changes
            Vector3 camPos = newPos;
            camPos.z = transform.position.z;
            transform.position = camPos;
        }
    }
}
