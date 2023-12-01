using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 10f;
    public float destroyDelay = 2f; // Time until the bullet is destroyed.
    public GameObject Blood;
    private Rigidbody2D rb;

    void Start()
    {
        // Get the Rigidbody2D component of the bullet.
        rb = GetComponent<Rigidbody2D>();

        // Set the initial velocity of the bullet.
        rb.velocity = transform.right * bulletSpeed;

        // Destroy the bullet after a delay.
        Destroy(gameObject, destroyDelay);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject GameElement = GameObject.FindWithTag("Game");
        if (collision.gameObject.CompareTag("Zombie"))
        {
            Debug.Log("Collision with Zombie");

            //Instantiate blood
            GameObject blood = Instantiate(Blood, transform.position, Quaternion.identity, GameElement.transform) as GameObject;
            // Play the particle system
            var bloodParticleSystem = blood.GetComponent<ParticleSystem>();
            if (bloodParticleSystem != null)
            {
                bloodParticleSystem.Play();
            }

            // Stop zombie movement
            ZombieSpawner zombieSpawner = GameElement.GetComponent<ZombieSpawner>();

            // Pass the specific zombie GameObject to HitByBullet
            zombieSpawner.HitByBullet(collision.gameObject);


            // Zombie dead on collision.
            Animator ZombAnimator = collision.gameObject.GetComponent<Animator>();
            ZombAnimator.SetTrigger("ZombieDead");

            // Destroy the bullet on collision.
            Destroy(gameObject);
        }
    }
}
