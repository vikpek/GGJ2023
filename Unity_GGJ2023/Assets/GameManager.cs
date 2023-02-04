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

    [SerializeField] private PlayerSpawner playerSpawner;

    private int minNightAngleAdd = 0;
    private int maxNightAngleAdd = 0;

    private List<IRotatable> rotatables = new();
    private List<Player> players = new();
    private List<WeedRoot> weedRoots = new();
    private List<Seedling> seedlings = new();
    private Action OnWeedGrow = delegate { };
    private float timer = 0.0f;
    void Start()
    {
        playerSpawner.OnPlayerSpawn += HandlePlayerSpawn;
        InvokeRepeating("SpawnTick", 0f, Configs.Instance.Get.spawnInterval);
        rotatables.Add(Instantiate(planetPrefab, planetCenter));

        InitWater(0f);
        InitWater(180f);
    }

    private void InitWater(float rotation)
    {
        Water water = Instantiate(waterPrefab, planetCenter);
        rotatables.Add(water);
        water.transform.Rotate(Vector3.back, rotation, Space.Self);
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
        weedRootInstance.transform.Rotate(Vector3.back, rand.Next(Configs.Instance.Get.minNightAngle + minNightAngleAdd, Configs.Instance.Get.maxNightAngle + maxNightAngleAdd), Space.Self);
        rotatables.Add(weedRootInstance);

        weedRootInstance.OnRemove += HandleOnRemove;
        return weedRootInstance;
    }
    private void HandleOnInteract(InteractiveRotatable obj, Player player)
    {
        if (obj == null)
        {
            if (player.HasWater())
            {
                SpawnSeedling(player);
                player.UseWater();
            }
        }
        else
        {
            switch (obj)
            {
                case Water water:
                    player.AddWater();
                    break;
                case Seedling seedling:
                    if (Seedling.IsReadyToHarvest)
                        player.AddFlower();
                    break;

            }
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
        seedling.rotatingObject.rotation = player.rotatingObject.rotation;
        seedling.OnInteract += HandleInteractWithSeedling;
        rotatables.Add(seedling);
    }
    private void HandleInteractWithSeedling(InteractiveRotatable obj)
    {
        throw new NotImplementedException();
    }

    void Update()
    {
        foreach (IRotatable rotatable in rotatables)
            rotatable.AddRotation(Configs.Instance.Get.rotationSpeed);

        OnWeedGrow();
    }
}