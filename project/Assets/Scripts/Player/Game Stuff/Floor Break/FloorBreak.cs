using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorBreak : MonoBehaviour
{
    private bool breaking = false;
    private bool broken = false;
    private float breakDelay = -1f;
    private float respawnDelay = -1f;
    
    [SerializeField] private float breakTime = 0f;
    [SerializeField] private GameObject destroy;
    [SerializeField] private GameObject instant;
    [SerializeField] private float respawnTime = 5f;
    [SerializeField] private AudioSource audio = null;

    private void FixedUpdate()
    {
        if (breaking && !broken)
        {
            Quaternion target = Quaternion.Euler(Random.Range(-5,5), 0, Random.Range(-5, 5));

            // Dampen towards the target rotation
            transform.rotation = target;

            if (breakDelay != -1f && breakDelay < Time.time)
            {
                broken = true;
                breaking = false;
                destroy.AddComponent<Rigidbody>();

                Destroy(destroy, 5f);

                respawnDelay = respawnTime + Time.time;
            }
        }
        else if (broken && respawnDelay != -1f && respawnDelay < Time.time)
        {
            broken = false;
            breaking = false;
            breakDelay = -1f;
            respawnDelay = -1f;

            transform.rotation = Quaternion.Euler(0,0,0);
            destroy = Instantiate(instant, transform);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!breaking && !broken)
        {
            breaking = true;
            breakDelay = Time.time + breakTime;

            if (audio)
                audio.Play();
        }
    }
}
