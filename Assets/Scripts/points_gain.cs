using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class points_gain : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        // Get the animator component from the GameObject.
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void Triggeranimator()
    {
        // Play the animator.
        animator.SetTrigger("show");
    }
}
