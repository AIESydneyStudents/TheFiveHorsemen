using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class TeleportZone : MonoBehaviour
{
    private Color debugColour = Color.red;
    private Color debugColourLine = Color.white;
    private Color debugColourEnter = Color.green;

    [SerializeField] private Transform exit;
    [SerializeField] private bool debug = false;

    private void OnTriggerEnter(Collider other)
    {
        if (Application.isPlaying)
        {
            CharacterController ya;

            if (other.transform.TryGetComponent<CharacterController>(out ya))
            {
                ya.enabled = false;
                ya.transform.position = exit.position;
                ya.enabled = true;
            }
        }
    }

    void OnDrawGizmos()
    {
        if (debug)
        {
            Gizmos.color = debugColourEnter;
            Gizmos.DrawWireSphere(transform.position, 0.1f);

            Gizmos.color = debugColourLine;
            Gizmos.DrawLine(transform.position, exit.position);

            Gizmos.color = debugColour;
            Gizmos.DrawWireSphere(exit.position, 0.1f);
        }
    }
}
