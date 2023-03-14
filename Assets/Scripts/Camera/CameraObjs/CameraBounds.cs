using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBounds : MonoBehaviour
{
    [Header("Camera Variables")]
    public float desiredSize;
    public float transitionTime;
    public float transitionScaleTime;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Movement>() != null && CameraController.instance)
        {
            CameraController.instance.bounds = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (CameraController.instance && CameraController.instance.bounds == this)
        {
            CameraController.instance.bounds = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new(0.35f, 0.1f, 1, 0.25f);
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}
