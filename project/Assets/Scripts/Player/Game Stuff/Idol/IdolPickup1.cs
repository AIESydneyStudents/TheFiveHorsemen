using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdolPickup1 : MonoBehaviour
{
    public ParticleSystem idolPickup;

    public float pickuptime = -1f;
    private void OnTriggerEnter(Collider other)
    {
        Player ya;

        if (other.transform.TryGetComponent<Player>(out ya))
        {
            if (pickuptime == -1f)
            {
                pickuptime = Time.time + 5f;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Player ya;

        if (other.transform.TryGetComponent<Player>(out ya))
        {
            if (pickuptime != -1f && pickuptime < Time.time)
            {
                pickuptime = -1f;
                ya.EndGame();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Player ya;

        if (other.transform.TryGetComponent<Player>(out ya))
        {
            pickuptime = -1f;
        }
    }
}
