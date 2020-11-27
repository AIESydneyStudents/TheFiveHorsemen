using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdolPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Player ya;

        if (other.transform.TryGetComponent<Player>(out ya))
        {
            ya.EndGame();
        }
    }
}
