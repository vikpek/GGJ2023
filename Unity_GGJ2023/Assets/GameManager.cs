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
        obj.OnInteract += delegate(InteractiveRotatable rotatable)
        {
            HandleOnInteract(rotatable, obj);
        };
        rotatables.Add(obj);
    }
    void SpawnTick()
    {
        var weedRoot = SpawnWeedRoot();
        OnWeedGrow += weedRoot.Grow;
        weedRoots.Add(weedRoot);
    }

    Random rand = new();
    private WeedRoot SpawnWeedRoot()
    {
        WeedRoot weedRootInstance = Instantiate(weedRootPrefab, planetCenter);
        weedRootInstance.Reset();
        weedRootInstance.transform.Rotate(Vector3.back, rand.Next(minNightAngle + minNightAngleAdd, maxNightAngle + maxNightAngleAdd), Space.Self);
        rotatables.Add(weedRootInstance);

        weedRootInstance.OnRemove += HandleOnRemove;
        return weedRootInstance;
    }
    private void HandleOnInteract(InteractiveRotatable obj, Player player)
    {
        if (obj == null)
            SpawnSeedling(player);
        else
        {
            // does player have water? -> water
            // is seedling ready? -> harvest
            // otherwise do nothing...
        }
    }
    private void HandleOnRemove(InteractiveRotatable obj)
    {
        if (rotatables.Contains(obj))
        {
            rotatables.Remove(obj);
            obj.gameObject.SetActive(false);
            Destroy(obj);
        }
    }

    private void SpawnSeedling(Player player)
    {
        Seedling seedling = Instantiate(seedlingPrefab, planetCenter);
        seedling.transform.rotation = player.transform.rotation;
        seedling.OnRemove += HandleHarvestSeedling;
        rotatables.Add(seedling);
    }
    private void HandleHarvestSeedling(InteractiveRotatable obj)
    {
        throw new NotImplementedException();
    }

    void Update()
    {
        foreach (IRotatable rotatable in rotatables)
            rotatable.AddRotation(rotationSpeed);

        OnWeedGrow();
    }
}