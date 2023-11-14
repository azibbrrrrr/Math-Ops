using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 10f;
    public float destroyDelay = 2f; // Time until the bullet is destroyed.

    private Rigidbody2D rb;

    void Start()
    {
        // Get the Rigidbody2D component of the bullet.
        rb = GetComponent<Rigidbody2D>();

        // Set the initial velocity of the bullet.
        rb.velocity = transform.right * bulletSpeed;

        // print("bullet instantiated");
        // Destroy the bullet after a delay.
        Destroy(gameObject, destroyDelay);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Zombie"))
        {
            Debug.Log("Collision with Zombie");

            // Destroy the enemy (zombie) GameObject on collision.
            Destroy(collision.gameObject);

            // Destroy the bullet on collision.
            Destroy(gameObject);
        }
    }
}
