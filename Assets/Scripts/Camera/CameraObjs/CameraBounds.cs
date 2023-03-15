using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBounds : MonoBehaviour
{
    #region Variables

    [Header("Camera Variables")]
    [Space(3)]
    public bool followPlayer;
    [Space(3)]
    public float desiredSize;
    public float transitionTime;
    public float transitionScaleTime;

    #endregion

    #region Collisions

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Camera works based off of collision detections, you set the instance to this to allow for proper movement between the 2. Otherwise it doesn't work
        if (collision.gameObject.tag == "Player" && CameraController.instance)
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
    #endregion

    #region Gizmos Render
    private void OnDrawGizmos()
    {
        // why he ourple :skull:
        Gizmos.color = new(0.35f, 0.1f, 1, 0.25f);
        Gizmos.DrawCube(transform.position, transform.localScale);
    }

    #endregion
}
