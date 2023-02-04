using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;
public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform planetCenter;
    [SerializeField] private Planet planetPrefab;
    [SerializeField] private Player playerPrefab;
    [SerializeField] private WeedRoot weedRootPrefab;
    [SerializeField] private Seedling seedlingPrefab;
    [SerializeField] private Water waterPrefab;
    [SerializeField] private int playerAmount = 2;
    [SerializeField] private PlayerSpawner playerSpawner;
    [SerializeField] private float rotationSpeed = 0.01f;
    [SerializeField] private int minNightAngle = 150;
    [SerializeField] private int maxNightAngle = 320;

    private int minNightAngleAdd = 0;
    private int maxNightAngleAdd = 0;
    

    


    public float SpawnInterval = 3.0f;

    private List<IRotatable> rotatables = new();
    private List<Player> players = new();
    private List<WeedRoot> weedRoots = new();
    private List<Seedling> seedlings = new();

    private float timer = 0.0f;

    private Action OnWeedGrow = delegate { };

    void Start()
    {
        playerSpawner.OnPlayerSpawn += HandlePlayerSpawn;
        InvokeRepeating("SpawnTick", 0f, SpawnInterval);
        rotatables.Add(Instantiate(planetPrefab, planetCenter));

        InitWater(0f);
        InitWater(180f);
    }

    private void InitWater(float rotation){
        Water water = Instantiate(waterPrefab, planetCenter);
        rotatables.Add(water);
        water.transform.Rotate(Vector3.back, rotation, Space.Self);
        //seedling.OnInteract += HandleHarvestSeedling;
    }
    private void HandlePlayerSpawn(Player obj)
    {
        obj.OnInteract += HandleRemoveInteractiveRotatable;
        rotatables.Add(obj);
    }
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
        weedRootInstance.Reset();
        weedRootInstance.transform.Rotate(Vector3.back, rand.Next(minNightAngle +minNightAngleAdd, maxNightAngle + maxNightAngleAdd), Space.Self);
        rotatables.Add(weedRootInstance);

        weedRootInstance.OnInteract += HandleRemoveInteractiveRotatable;
        return weedRootInstance;
    }
    private void HandleRemoveInteractiveRotatable(InteractiveRotatable obj)
    {
        IRotatable rotatable = obj;
        if (rotatables.Contains(rotatable))
            rotatables.Remove(rotatable);
    }

    private InteractiveRotatable HandleSpawnSeedling(InteractiveRotatable obj)
    {
        Seedling seedling = Instantiate(seedlingPrefab, planetCenter);
        rotatables.Add(seedling);
        seedling.transform.Rotate(Vector3.back, rand.Next(0, 360), Space.Self);
        seedling.OnInteract += HandleHarvestSeedling;
        return seedling;
    }
    private void HandleHarvestSeedling(InteractiveRotatable obj)
    {
        throw new NotImplementedException();
    }

    void Update()
    {
        foreach (IRotatable rotatable in rotatables)
            rotatable.AddRotation(rotationSpeed);
    }
}