using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using Random = System.Random;
public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform planetCenter;
    [SerializeField] private Planet planetPrefab;
    [SerializeField] private Player playerPrefab;
    [SerializeField] private WeedRoot weedRootPrefab;
    [SerializeField] private int playerAmount = 2;
    [SerializeField] private PlayerSpawner playerSpawner;
    [SerializeField] private float rotationSpeed = 0.01f;


    public float SpawnInterval = 3.0f;

    private List<IRotatable> rotatables = new();
    private List<Player> players = new();
    private List<WeedRoot> weedRoots = new();

    private float timer = 0.0f;

    private Action OnWeedGrow = delegate { };

    void Start()
    {
        playerSpawner.OnPlayerSpawn += HandlePlayerSpawn;
        
        InvokeRepeating("SpawnTick", 0f, 3f);
        rotatables.Add(Instantiate(planetPrefab, planetCenter));
        
        
    }
    private void HandlePlayerSpawn(Player obj) => rotatables.Add(obj);
    void SpawnTick()
    {
        OnWeedGrow();
        var weedRoot = SpawnWeedRoot();
        OnWeedGrow += weedRoot.Grow;
        weedRoots.Add(weedRoot);
    }

    Random rand = new();
    private WeedRoot SpawnWeedRoot()
    {
        WeedRoot weedRootInstance = Instantiate(weedRootPrefab, planetCenter);
        rotatables.Add(weedRootInstance);
        weedRootInstance.transform.Rotate(Vector3.back, rand.Next(0, 360), Space.Self);
        return weedRootInstance;
    }

    void Update()
    {
        foreach (IRotatable rotatable in rotatables)
            rotatable.AddRotation(rotationSpeed);
    }
}