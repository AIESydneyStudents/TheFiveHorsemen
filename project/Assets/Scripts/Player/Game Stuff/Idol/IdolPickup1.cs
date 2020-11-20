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
                idolPickup.Play(true);
                idolPickup.Stop(false);
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
                idolPickup.Play(true);
                idolPickup.Stop(false);
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
