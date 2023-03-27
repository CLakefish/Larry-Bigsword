using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Yeah")]
    private GameObject player;
    public static CameraManager instance;
    internal BoundsObj bounds;
    internal SpriteRenderer boundsRender;
    Camera cam;

    [Header("Camera Size")]
    float camVertSize, camHorzSize;

    [Header("Bounds")]
    float leftBound, rightBound, bottomBound, topBound;

    [Header("Misc")]
    float camX, camY;

    Vector3 initialPos;
    public float shakeMagnitude, shakeDuration;

    private void Awake()
    {
        cam = this.GetComponent<Camera>();
        instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        initialPos = transform.position;

        camVertSize = cam.orthographicSize;
        camHorzSize = cam.aspect * camVertSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (bounds == null) return;

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


        Vector3 dir = new Vector3(0f, 0f, 0f);
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if (GameObject.FindGameObjectWithTag("enemy") != null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("enemy");

            dir = ((obj.transform.position + player.transform.position) / 2).normalized;
        }
        else
        {
            dir = mousePos + player.transform.position / 2;
            dir.Normalize();
        }

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, bounds.size, bounds.transitionScaleTime * Time.deltaTime);

        Bounds();

        camX = Mathf.Clamp(player.transform.position.x - dir.x, leftBound, rightBound);
        camY = Mathf.Clamp(player.transform.position.y - dir.y, bottomBound, topBound);

        cam.transform.position = Vector3.Lerp(cam.transform.position, new Vector3(camX, camY, cam.transform.position.z), bounds.transitionTime * Time.deltaTime);
    }

    void Bounds()
    {
        if (boundsRender == null) return;

        camVertSize = cam.orthographicSize;
        camHorzSize = cam.aspect * camVertSize;

        leftBound = boundsRender.bounds.min.x + camHorzSize;
        rightBound = boundsRender.bounds.max.x - camHorzSize;
        bottomBound = boundsRender.bounds.min.y + camVertSize;
        topBound = boundsRender.bounds.max.y - camVertSize;
    }
}
