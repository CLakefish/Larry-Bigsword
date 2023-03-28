/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*
 * Name: Carson Lakefish & Nico Sayed
 * Date: 3 / 24 / 2023
 * Desc: Menu Handler
 *~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/


using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Space(3), Header("Menus"), Space(3)]
    [SerializeField] private GameObject HeatlhDisplay;
    [SerializeField] public GameObject DeathScreen;
    [SerializeField] public GameObject PauseMenu;
    [SerializeField] public GameObject speedrunTime;


    private BetterMovement player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<BetterMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.gameObject.GetComponent<HealthPoints>().currentHP == 0) StartCoroutine(deathMenu());

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0f;
            PauseMenu.SetActive(true);
            HeatlhDisplay.SetActive(false);
        }
    }

    IEnumerator deathMenu()
    {
        yield return new WaitForSeconds(0.1f);
        DeathScreen.SetActive(true);
        PauseMenu.SetActive(false);

        speedrunTime.GetComponent<Timer>().isActive = false;

        HeatlhDisplay.SetActive(false);
    }

    public void LoadScene()
    {
        Time.timeScale = 1f;
        return;
    }

    public void ResumeScene()
    {
        Time.timeScale = 1f;

        if (FindObjectOfType<SwordInput>()) Destroy(FindObjectOfType<SwordInput>().sword);

        PauseMenu.SetActive(false);
        HeatlhDisplay.SetActive(true);
    }

    public void ReloadScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        return;
    }

    public void Exit()
    {
        Time.timeScale = 1f;
        Application.Quit();
        return;
    }
}
