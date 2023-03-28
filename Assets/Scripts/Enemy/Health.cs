/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*
 * Name: Nico Sayed
 * Date: 3 / 10 / 2023
 * Desc: Basic Health Script 
 *~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int currentHealth = 3;
    public int maxHealth = 3;
    public bool destroyAtZero = true;
    // Start is called before the first frame update
    void Start()
    {

    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            //add extra death effects in here in future 

            currentHealth = (0);
            if (destroyAtZero)
            {
                DeathEffects deathEffects = GetComponent<DeathEffects>();
                if (deathEffects != null)
                {
                    deathEffects.deathEvent.Invoke();
                }
                Destroy(gameObject);
            }
        }
    }
    public void GainHealth(int health)
    {
        currentHealth += health;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}