using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorBreak : MonoBehaviour
{
    private bool breaking = false;

    [SerializeField] private float breakTime = 0f;

    private void FixedUpdate()
    {
        if (breaking)
        {
            Quaternion target = Quaternion.Euler(Random.Range(-5,5), 0, Random.Range(-5, 5));

            // Dampen towards the target rotation
            transform.rotation = target;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!breaking)
        {
            breaking = true;
            Destroy(gameObject, breakTime);
        }
    }
}
