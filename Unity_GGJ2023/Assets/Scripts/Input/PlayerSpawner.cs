using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private int minAmount;
    [SerializeField] private int maxAmount;
    [SerializeField] private GameObject planetCenter;
    [SerializeField] private PlayerInputManager playerInputManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable(){
        playerInputManager.onPlayerJoined += Spawn;
    }

    private void Spawn(UnityEngine.InputSystem.PlayerInput obj)
    {
        Debug.Log("Spawn!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
