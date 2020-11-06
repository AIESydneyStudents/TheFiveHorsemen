using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushOnCollide : MonoBehaviour
{
    [SerializeField] private float pushDistance = 5f;

    private void OnTriggerStay(Collider other)
    {
        Player ya;

        if (other.transform.TryGetComponent<Player>(out ya))
        {
            Vector3 dir = transform.forward;

            ya.AddVelocity(dir * -pushDistance);
        }
    }
}
