using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Variables

    [Header("Camera Variables")]
    public GameObject player;
    public static CameraController instance;
    public CameraBounds bounds;

    [Header("https://i.kym-cdn.com/entries/icons/original/000/023/977/cover3.jpg")]
    Vector2 targetPos;
    Vector2 newPos;
    Vector2 velocity;

    #endregion

    // Before start
    private void Awake()
    {
        targetPos = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (bounds)
        {
            targetPos = bounds.transform.position;

            this.GetComponent<Camera>().orthographicSize = Mathf.Lerp(this.GetComponent<Camera>().orthographicSize, bounds.desiredSize, bounds.transitionScaleTime * Time.deltaTime);
        }

        newPos = Vector2.SmoothDamp(newPos, targetPos, ref velocity, bounds.transitionTime);

        Vector3 camPos = newPos;
        camPos.z = transform.position.z;
        transform.position = camPos;
    }
}
