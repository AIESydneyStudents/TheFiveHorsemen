using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    private bool colliding = false;
    private Vector3 startPos;
    public bool finished = false;

    [SerializeField] private Transform plate;
    [SerializeField] private Vector3 fallback;
    [SerializeField] private float lerpSpeed = 1f;
    [SerializeField] private UnityEngine.Events.UnityEvent finishTask;

    void Start()
    {
        startPos = plate.localPosition;
    }

    /// <summary>
    /// When standing on, it pushes it down.
    /// </summary>
    void Pressure()
    {
        //if (finished) return;

        if (colliding)
            plate.localPosition = Vector3.Lerp(plate.localPosition, startPos - fallback, Time.deltaTime * lerpSpeed);
        else plate.localPosition = Vector3.Lerp(plate.localPosition, startPos, Time.deltaTime * lerpSpeed);

        Vector3 getTo = (startPos - fallback);
        getTo *= 1.01f;

        if (plate.localPosition == getTo)
        {
            finished = true;
            finishTask.Invoke();
        }
    }

    private void FixedUpdate()
    {
        Pressure();
    }

    private void OnTriggerEnter(Collider other)
    {
        colliding = true;
    }

    private void OnTriggerExit(Collider other)
    {
        colliding = false;
    }
}
