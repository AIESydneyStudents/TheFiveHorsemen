using UnityEngine;

public class Stick : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.transform.parent = transform;

        Player pl;

        if (other.transform.TryGetComponent<Player>(out pl))
        {
            pl.SetVelocity(Vector3.zero);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.parent = null;
    }
}
