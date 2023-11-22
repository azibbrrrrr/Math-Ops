using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    private Animator anim;
    public GameObject bulletPrefab;
    public Transform gunTransform;
    public GameObject muzzleFlash;

    private bool isShooting = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        // Ensure the muzzle flash is initially deactivated.
    }

    public void Attack()
    {
        // If shooting is already in progress, return and don't shoot again.
        if (isShooting)
            return;

        anim.SetTrigger("attack");
        // Activate the muzzle flash.
        if (muzzleFlash != null)
        {
            muzzleFlash.SetActive(true);
        }

        // Instantiate a new bullet at the gun's position.
        GameObject newBullet = Instantiate(bulletPrefab, gunTransform.position, Quaternion.identity, GetComponent<Transform>());

        // Set isShooting to true to prevent further shooting until the muzzle flash is gone.
        isShooting = true;

        // Deactivate the muzzle flash after a short delay (e.g., 0.1 seconds).
        StartCoroutine(DeactivateMuzzleFlash());
    }

    IEnumerator DeactivateMuzzleFlash()
    {
        yield return new WaitForSeconds(0.25f); // Adjust this delay as needed.

        // Deactivate the muzzle flash.
        if (muzzleFlash != null)
        {
            muzzleFlash.SetActive(false);
        }

        // Allow shooting again.
        isShooting = false;
    }
}
