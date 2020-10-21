using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private float lerpSpeed = 1f;
    [SerializeField] private JoystickMouse joystick;
    [SerializeField] private Transform fall;
    [SerializeField] private Vector3 fallBack;
    [SerializeField] private bool shouldFall;
    [SerializeField] private int sceneTo;

    [System.Serializable]
    public struct dissapear
    {
        public string name;
        public GameObject goAway;
        public float timeLeft;
    }

    [SerializeField] private dissapear[] dissapears;
    private Vector3 startPos;
    private bool finished = false;

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

    void Update()
    {
        if (!joystick.Clicking() && !finished)
        {
            if (ButtonHover())
            {
                transform.position = Vector3.Lerp(transform.position, startPos - fallBack, Time.deltaTime * lerpSpeed);
            }
            else transform.position = Vector3.Lerp(transform.position, startPos, Time.deltaTime * lerpSpeed);
        }
        else if (!finished && shouldFall)
        {
            foreach (Transform child in fall)
            {
                Rigidbody go;
                child.gameObject.TryGetComponent(out go);

                if (!go)
                {
                    go = child.gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
                    go.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                }
            }

            finished = true;
        }
        else if (finished)
        {
            int ready = 0;

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
                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneTo);
            }
        }
    }
}
