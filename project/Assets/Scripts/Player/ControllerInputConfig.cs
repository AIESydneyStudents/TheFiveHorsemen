using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInputConfig : MonoBehaviour
{
    static public splitScreenList[] splitScreenController;

    [System.Serializable]
    public struct splitScreenInfo
    {
        public ControllerInput.availableControllers controller;
        public Rect viewPort;
    }

    [System.Serializable]
    public struct splitScreenList
    {
        public string name;
        public splitScreenInfo[] splits;
    }

    [Header("Split Screen Configuration")]
    [SerializeField] private splitScreenList[] splitScreenConfig;
    //

    public virtual void Start()
    {
        splitScreenController = splitScreenConfig;
    }
}
