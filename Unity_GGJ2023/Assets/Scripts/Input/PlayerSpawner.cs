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
        playerInputManager.onPlayerLeft += Despawn;
    }
    void OnDisable(){
        Debug.Log("Disable PlayerSpawner");
    }

    private void Despawn(PlayerInput input)
    {
        Debug.Log($"Leaving Player {input.playerIndex}, {input.currentActionMap}, {input.currentControlScheme}, {input.currentControlScheme}");
        //Debug.Log("Leaving Player: "+ input.)
    }

    private void Spawn(PlayerInput input)
    {
        Debug.Log($"Spawning Player {input.playerIndex}, {input.currentActionMap}, {input.currentControlScheme}, {input.currentControlScheme}");
        var pi = input.GetComponent<PlayerInputController>();
        pi.InitPlayerInput(input.currentControlScheme);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

}
