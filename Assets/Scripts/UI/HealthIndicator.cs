using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthIndicator : MonoBehaviour
{
    [Header("Misc Variables")]
    [Space(3)]
    public bool isActive;
    [Space(3)]
    public GameObject player;
    TMP_Text text;

    // Start is called before the first frame update
    void Start()
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

        text.text = player.GetComponent<HealthPoints>().currentHP + "  /  " + player.GetComponent<HealthPoints>().maxHP;
    }
}
