using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPusher : MonoBehaviour
{
    private Vector3 startPos;
    private float delayTimer = 0f;
    private float extendTimer = 0f;
    private bool extended = false;

    [SerializeField] private Transform pusher;
    [SerializeField] private Transform extendTo;
    [SerializeField] private float extendSpeed = 1f;
    [SerializeField] private float extendWait = 1f;
    [SerializeField] private float delayDuration = 1f;

    // Start is called before the first frame update
    void Start()
    {
        startPos = pusher.localPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!extended && delayTimer <= Time.time)
        {
            if (pusher.localPosition == extendTo.localPosition)
            {
                extended = true;
                extendTimer = Time.time + extendWait;
            }

            pusher.localPosition = Vector3.MoveTowards(pusher.localPosition, extendTo.localPosition, extendSpeed * Time.deltaTime);
        }
        else if (extended && extendTimer <= Time.time)
        {
            if (pusher.localPosition == startPos)
            {
                delayTimer = Time.time + delayDuration;
                extended = false;
            }

            pusher.localPosition = Vector3.MoveTowards(pusher.localPosition, startPos, extendSpeed * Time.deltaTime);
        }
    }
}
