using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownPlayer : MonoBehaviour
{
    public Transform moveTo;
    public UnityEngine.UI.Image fade;
    public float speed;

    private float timer = 0f;
    private bool finished = false;

    void FixedUpdate()
    {
        if (timer < Time.time)
        {
            fade.color = Color.Lerp(fade.color, new Color(fade.color.r, fade.color.g, fade.color.b, 1), Time.deltaTime);
            
            if (fade.color.a >= 0.9 && !finished)
            {
                finished = true;
                ControllerInput.available = 0;

                int level = 1;
                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(level);
            }
        }

        transform.position = Vector3.Lerp(transform.position, moveTo.position, Time.deltaTime * speed);
    }
}
