using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatParticles : MonoBehaviour
{
    public ParticleSystem splatParticles;
    public GameObject splatprefab;
    // public Transform splatHolder;
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    private void OnParticleCollision(GameObject other)
    {
        ParticlePhysicsExtensions.GetCollisionEvents(splatParticles, other, collisionEvents);

        int count = collisionEvents.Count;
        for (int i = 0; i < count; i++)
        {
            GameObject splat = Instantiate(splatprefab, collisionEvents[i].intersection, Quaternion.identity) as GameObject;
            // splat.transform.SetParent(splatHolder, true);
            Splat splatScript = splat.GetComponent<Splat>();
            splatScript.Initialize(Splat.SplatLocation.Foreground);
        }
    }
}
