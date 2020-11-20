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
                idolPickup.Play();
                pickuptime = Time.time + 5f;
<<<<<<< Updated upstream
                
=======
                
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
<<<<<<< HEAD
=======
                
>>>>>>> master
=======
                
>>>>>>> Stashed changes
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
            idolPickup.Stop();
            
        }
    }
}
