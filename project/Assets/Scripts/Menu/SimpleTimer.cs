using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTimer : MonoBehaviour
{
    private float timer = 0f;
    private bool finished = false;

    public int sceneFrom = 1;
    public int sceneTo = 2;
    public float timerDelay = 1f;

    private void Start()
    {
        timer = Time.time + timerDelay;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timer < Time.time && !finished)
        {
            finished = true;
            ControllerInput.available = 0;

            int level = Random.Range(sceneFrom, sceneTo + 1);
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(level);
        }
    }
}
