using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // remove
    public GameObject debug;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public bool ButtonHover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            debug.transform.position = hit.point;
            return true;
        }
        else return false;
    }

    void Update()
    {
        ButtonHover();
    }
}
