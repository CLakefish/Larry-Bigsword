/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*
 * Name: Carson Lakefish
 * Date: 3 / 15 / 2023
 * Desc: Unused camera object
 *~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBounds : MonoBehaviour
{
    #region Variables

    [System.Serializable]
    public class EnemyData
    {
        public GameObject enemy;
        public int chanceIncrease = 1;
    }

    [Header("Camera Variables")]
    [Space()]
    public bool followPlayer;
    [Space()]
    public float desiredSize;
    public float transitionTime;
    public float transitionScaleTime;

    [Header("Door System")]
    public DoorHandler doorHandler;

    [Header("Spawn Enemies")]
    [Space()]
    public bool spawnEnemies;
    [Space()]
    public GameObject enemyIndicator;
    public List<EnemyData> enemies;
    [HideInInspector]
    public List<GameObject> enemyPrefabs;
    [Space()]
    public float enemyCount, distance;
    float enemyCountTemp;
    List<GameObject> enemyList = new();

    #endregion

    private void Start()
    {
        doorHandler = FindObjectOfType<DoorHandler>().GetComponent<DoorHandler>();

        if (spawnEnemies)
        {
            enemyCountTemp = enemyCount;

            foreach (EnemyData currentData in enemies)
            {
                for (int i = 0; i < currentData.chanceIncrease; i++)
                {
                    enemyPrefabs.Add(currentData.enemy);
                }
            }
        }
    }

    #region Collisions

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (spawnEnemies) doorHandler.closeDoors();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Camera works based off of collision detections, you set the instance to this to allow for proper movement between the 2. Otherwise it doesn't work
        if (collision.gameObject.tag == "Player" && CameraController.instance)
        {
            CameraController.instance.bounds = this;
        }

        if (spawnEnemies)
        {
            StartCoroutine(SpawnEnemies());
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

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(1f);

        List<GameObject> indicators = new List<GameObject>();

        if (enemyCountTemp > 0)
        {
            GameObject clone = Instantiate(enemyIndicator, (Vector2)transform.position + Random.insideUnitCircle * distance, Quaternion.identity);
            indicators.Add(clone);
            enemyCountTemp--;
        }

        yield return new WaitForSeconds(0.5f);

        foreach (GameObject obj in indicators)
        {
            GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
            GameObject clone = Instantiate(prefab, obj.transform.position, Quaternion.identity);

            enemyList.Add(clone);
        }

        GameObject[] enemyListCopy = enemyList.ToArray();

        foreach (GameObject currentEnemy in enemyListCopy)
        {
            if (currentEnemy == null || (currentEnemy.TryGetComponent(out HealthPoints h) && h.currentHP <= 0))
            {
                enemyList.Remove(currentEnemy);
            }
        }
    }

    #region Gizmos Render
    private void OnDrawGizmos()
    {
        // why he ourple :skull:
        Gizmos.color = new(0.35f, 0.1f, 1, 0.25f);
        Gizmos.DrawCube(transform.position, transform.localScale);
    }

    #endregion
}
