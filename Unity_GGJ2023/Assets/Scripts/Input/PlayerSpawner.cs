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

    private List<Player> players = new();
    public List<Player> Players => players;
    public event Action<Player> OnPlayerSpawn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable(){
        playerInputManager.onPlayerJoined += Spawn;
    }

    private void Spawn(UnityEngine.InputSystem.PlayerInput obj)
    {
        var player =obj.GetComponent<Player>();
        OnPlayerSpawn(player);
        players.Add(player);
        Debug.Log("Spawn!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}