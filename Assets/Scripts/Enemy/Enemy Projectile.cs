/*Name Nico Sayed
 * Date 3/15/2023
 * Enemys shooter
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerCannon : MonoBehaviour
{
    [Tooltip("Add a prefab from your assets into here. Make sure it has a RigidBody2D that can move")]
    public GameObject projectile;
    public float speed = 10;
    public float cooldown = 0.2f;
    private float timer;
    [Tooltip("How much to rotate the projectile in degrees")]
    public float rotationOffset = 0;
    //sound effect for firing stuff
    public AudioClip firingSound;
    AudioSource myAud;
    // Start is called before the first frame update
    void Start()
    {
        myAud = GetComponent<AudioSource>();
        timer = cooldown;
    }
    public void WalkNoise()
    {
        myAud.PlayOneShot(firingSound);
    }
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        //check for what is by default a mouseclick or joystick button
        if (Input.GetAxisRaw("Fire1") > 0 && timer >= cooldown)
        {
            myAud.PlayOneShot(firingSound);
            timer = 0;
            //set the spawn location
            Vector3 spawnPos = transform.position;
            //grab the mouse position
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            //setup a direction to fire
            Vector3 fireDir = (mousePos - spawnPos).normalized;
            //make the prefab real
            GameObject clone = Instantiate(projectile, spawnPos, Quaternion.Euler(0, 0, Mathf.Atan2(fireDir.y, fireDir.x) * Mathf.Rad2Deg + rotationOffset));
            //launch in desired direction
            clone.GetComponent<Rigidbody2D>().velocity = fireDir * speed;
        }
    }
}