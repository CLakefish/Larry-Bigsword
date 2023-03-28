/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*
 * Name: Carson Lakefish
 * Date: 3 / 23 / 2023
 * Desc: Speedrun Timer
 *~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/


using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [Header("Misc Variables")]
    [Space(3)]
    public bool isActive;
    [Space(3)]
    TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
        {
            return;
        }

        int minutes = (int)(Time.timeSinceLevelLoad / 60);
        int seconds = (int)(Time.timeSinceLevelLoad % 60);
        int milliseconds = (int)(Time.timeSinceLevelLoad * 100f) % 100;

        string addedZero = seconds < 10 ? "0" : "";
        string addedZeroM = milliseconds < 10 ? "0" : "";

        text.text = minutes + " : " + addedZero + seconds + " : " + milliseconds + addedZeroM;
    }
}
