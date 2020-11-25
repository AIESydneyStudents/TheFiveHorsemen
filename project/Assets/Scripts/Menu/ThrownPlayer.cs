using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownPlayer : MonoBehaviour
{
    public Transform moveTo;
    public float speed;

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, moveTo.position, Time.deltaTime * speed);
    }
}
