using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    static public ControllerInput.availableControllers winner;
    static public settingsInfo settings;
    static public characterInfo[] characters;

    [System.Serializable]
    public struct playerInfo
    {
        public ControllerInput.availableControllers controller;
        public characterInfo character;
        public GameObject spawnPosition;
        public Player player;
    }

    [System.Serializable]
    public struct characterInfo
    {
        public ControllerInput.availableControllers controller;
        public Material body;
    }

    [System.Serializable]
    public struct settingsInfo
    {
        public float globalMoveSpeed;
        public float globalTurnSpeed;
        public float globalGravity;
        public float globalJumpHeight;
        public float globalFriction;
        public float globalPushStrength;
        public float globalDashStrength;
        public float globalCanTurn;
        public bool debug;
    }

    [Header("Settings")]
    [SerializeField] private settingsInfo _playerSettings;

    [Header("Characters")]
    [SerializeField] private characterInfo[] _characters;

    public virtual void Start()
    {
        characters = _characters;
        settings = _playerSettings;
    }
}
