/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*
 * Name: Carson Lakefish
 * Date: 3 / 27 / 2023
 * Desc: Parry Visual Bar script
 *~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParryBar : MonoBehaviour
{
    public Image parryBar;

    public void updateParryBar(float maxC, float currentC)
    {
        if (currentC > 100f) currentC = 100f;

        parryBar.fillAmount = currentC / maxC;
    }
}
