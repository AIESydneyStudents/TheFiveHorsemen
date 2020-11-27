using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class MovingPlatform : MonoBehaviour
{
    [Header("Platform Path Movement"), SerializeField] private float followSpeed = 0f;
    [SerializeField] private bool debug = false;
    [SerializeField] private Transform[] points;

    [SerializeField] private Transform platform;

    private bool inverse = false;

    private int gotoPoint = 0;
    private Color debugColour = Color.red;

    void Start()
    {
        if (Application.isPlaying)
        {
            platform.position = points[0].position;

            if (points.Length > 0)
            {
                gotoPoint = 1;
            }
        }
    }

    void Update()
    {
        if (Application.isPlaying)
        {
            if (gotoPoint == points.Length)
            {
                inverse = true;
                gotoPoint--;
            }
            else if (gotoPoint == -1)
            {
                inverse = false;
                gotoPoint++;
            }

            if (gotoPoint < points.Length)
            {
                platform.position = Vector3.MoveTowards(platform.position, points[gotoPoint].position, Time.deltaTime * followSpeed);

                if (Vector3.Distance(platform.position, points[gotoPoint].position) < 0.001f)
                {
                    // Swap the position of the cylinder.

                    if (!inverse) gotoPoint++;
                    else gotoPoint--;
                }
            }

        }
    }

    void OnDrawGizmos()
    {
        if (debug)
        {
            Gizmos.color = debugColour;
            Gizmos.DrawWireSphere(points[0].position, 0.05f);

            for (int i = 0; i < points.Length; i++)
            {
                Vector3 start = points[i].position;
                Vector3 end = points[i].position;
                if (i + 1 < points.Length) end = points[i + 1].position;

                Gizmos.DrawLine(start, end);
                Gizmos.DrawWireSphere(end, 0.05f);
            }
        }
    }

    /// <summary>
    /// Auto loads all points from the child object named "path".
    /// </summary>
    [ContextMenu("Load Points From Child")]
    private void LoadPointsFromChild()
    {
        Transform child = transform.Find("Path");

        if (child != null)
        {
            points = new Transform[child.childCount];

            for (int i = 0; i < child.childCount; i++)
            {
                points[i] = child.GetChild(i);
            }
        }
    }
}
