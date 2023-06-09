/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*
 * Name: Carson Lakefish
 * Date: 3 / 20 / 2023
 * Desc: Impact Hit
 *~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HitManager : MonoBehaviour
{
    private static HitManager instance;
    static GameObject player;
    BetterMovement playerScript;

    private void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<BetterMovement>();
    }

    public static void ImpactHit()
    {
        SceneManager.activeSceneChanged += (scene1, scene2) => Time.timeScale = 1f;

        float time = instance.playerScript.impactTimeFreeze;

        instance.StartCoroutine(Impact());

        IEnumerator Impact()
        {
            Time.timeScale = 0.00001f;
            yield return new WaitForSeconds(time);
            Time.timeScale = 1f;
        }
    }
}
