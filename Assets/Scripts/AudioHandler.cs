/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*
 * Name: Carson Lakefish
 * Date: 3 / 28 / 2023
 * Desc: Audio Handler Script
 *~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    public static AudioClip enemyShoot, enemyHit, playerSwing, playerHit, playerParry, doorSound;
    static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        enemyShoot = Resources.Load<AudioClip>("");
        enemyHit = Resources.Load<AudioClip>("");

        playerHit = Resources.Load<AudioClip>("");
        playerSwing = Resources.Load<AudioClip>("");
        playerParry = Resources.Load<AudioClip>("");

        doorSound = Resources.Load<AudioClip>("");

        audioSrc = GetComponent<AudioSource>();
    }

    public static void PlaySound(string ID)
    {
        switch (ID)
        {
            case "eS":
                audioSrc.PlayOneShot(enemyShoot);
                break;
            case "eH":
                audioSrc.PlayOneShot(enemyHit);
                break;

            case "pS":
                audioSrc.PlayOneShot(playerSwing);
                break;
            case "pH":
                audioSrc.PlayOneShot(playerHit);
                break;
            case "pP":
                audioSrc.PlayOneShot(playerParry);
                break;

            case "d":
                audioSrc.PlayOneShot(doorSound);
                break;

        }
    }
}
