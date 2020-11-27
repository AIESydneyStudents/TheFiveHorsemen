using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorEvents : MonoBehaviour
{
    private Animation anim;

    void Start()
    {
        TryGetComponent<Animation>(out anim);
    }

    public void PlayAnimation()
    {
        anim.Play();
    }
}
