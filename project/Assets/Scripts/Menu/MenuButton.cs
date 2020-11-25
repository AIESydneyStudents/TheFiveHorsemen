using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    private Vector3 startPos;
    private bool finished = false;
    public int brickValue;

    [System.Serializable]
    public struct dissapear
    {
        public string name;
        public GameObject goAway;
        public float timeLeft;
    }

    [SerializeField] private float lerpSpeed = 1f;
    [SerializeField] private JoystickMouse joystick;
    [SerializeField] private Transform fall;
    [SerializeField] private Vector3 fallBack;
    [SerializeField] private bool shouldFall;
    [SerializeField] private dissapear[] dissapears;
    [SerializeField] private AudioSource fallAudio;
    [SerializeField] private UnityEngine.Events.UnityEvent finishTask;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    public bool ButtonHover()
    {
        Ray ray = Camera.main.ScreenPointToRay(joystick.transform.position);
        RaycastHit hit;

        int layerMask = 1 << 0;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            return (hit.transform.gameObject == this.gameObject);
        }
        else return false;
    }

    void FixedUpdate()
    {
        if (!joystick.Clicking() && !finished)
        {
            if (ButtonHover())
            {
                transform.position = Vector3.Lerp(transform.position, startPos - fallBack, Time.deltaTime * lerpSpeed);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, startPos, Time.deltaTime * lerpSpeed);
            }
        }
        else if (joystick.Clicking() && !finished && ButtonHover())
        {
            if (shouldFall)
            {
                //Rigidbody bro = gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
                //bro.velocity = fallBack * -80f;

                foreach (Transform child in fall)
                {
                    Rigidbody go;
                    child.gameObject.TryGetComponent(out go);

                    if (!go)
                    {
                        go = child.gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
                        go.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                        go.velocity = fallBack * -30f;
                    }

                    MenuButton mb;
                    if (child != transform && child.gameObject.TryGetComponent(out mb))
                    {
                        Destroy(mb, 0);
                    }
                }

                fall.DetachChildren();
            }

            finished = true;
        }
        else if (finished)
        {
            int ready = 0;

            if (!fallAudio.isPlaying) fallAudio.Play();

            for (int i = 0; i < dissapears.Length; i++)
            {
                dissapears[i].timeLeft -= Time.deltaTime;

                if (dissapears[i].timeLeft <= 0)
                {
                    dissapears[i].goAway.SetActive(false);
                    ready++;
                }
            }

            if (ready == dissapears.Length)
            {
                finishTask.Invoke();

                //if (brickValue == 2 || brickValue == 3)
                //{
                //    //Camera.main.transform.position = cameraPos;
                //    //Camera.main.transform.position = cameraAng;
                //    gameObject.SetActive(false);
                //    ControllerInput.available = 0;

                //    int level = (brickValue == 3) ? sceneTo : Random.Range(1, sceneTo + 1);
                //    UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(level);
                //}

                //if (brickValue == 1)
                //{
                //    Application.Quit();
                //}
            }
            //if (Brickvalue == 3)
            //{

            //}
        }
    }
}
