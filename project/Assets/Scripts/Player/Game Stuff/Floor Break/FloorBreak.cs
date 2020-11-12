using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorBreak : MonoBehaviour
{
    private bool breaking = false;
    private bool broken = false;
    private float breakDelay = 0f;

    [SerializeField] private float breakTime = 0f;
    [SerializeField] private AudioSource audio = null;

    private void FixedUpdate()
    {
        if (breaking && !broken)
        {
            Quaternion target = Quaternion.Euler(Random.Range(-5,5), 0, Random.Range(-5, 5));

            // Dampen towards the target rotation
            transform.rotation = target;

            if (breakDelay < Time.time)
            {
                broken = true;
                breaking = false;
                gameObject.AddComponent<Rigidbody>();

                Destroy(gameObject, 5f);
            }
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
