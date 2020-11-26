using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTimer : MonoBehaviour
{
    private float timer = 0f;
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
        if (timer < Time.time)
        {
            ControllerInput.available = 0;

            int level = Random.Range(sceneFrom, sceneTo + 1);
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(level);
        }
    }
}
