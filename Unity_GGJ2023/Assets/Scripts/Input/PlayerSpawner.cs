using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private int minAmount;
    [SerializeField] private int maxAmount;
    [SerializeField] private GameObject planetCenter;
    [SerializeField] private PlayerInputManager playerInputManager;
    [SerializeField] private int minNightAngle;
    [SerializeField] private int maxNightAngle;
    private List<Player> players = new();
    public List<Player> Players => players;
    public event Action<Player> OnPlayerSpawn;

    private int gamepadId = 0;


    void OnEnable() => playerInputManager.onPlayerJoined += Spawn;

    private void Spawn(PlayerInput input)
    {
    	var player = input.GetComponent<Player>();
        OnPlayerSpawn(player);
        players.Add(player);
        var pi = input.GetComponent<PlayerInputController>();
        if(input.currentControlScheme == "Gamepad")
            gamepadId++;        

        pi.InitPlayerInput(input.playerIndex, gamepadId-1);        
    }
}