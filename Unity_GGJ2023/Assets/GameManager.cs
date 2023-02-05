using System;
using System.Collections;
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
        AudioManager.Instance.PlayMusic(MusicPurpose.Chill);
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
            switch (player.CurrentlyHolding)
            {
                case Cargo.Nothing:
                    SpawnSeedling(player);
                    break;
                case Cargo.Water:
                    player.UseWater();
                    break;
                case Cargo.Flower:
                    player.UseFlower();
                    break;
            }
        }
        else
        {
            switch (obj)
            {
                case Water water:
                    HandleWaterPickup(player);
                    break;
                case Seedling seedling:
                    if (seedling.IsReadyToHarvest)
                    {
                        HandleHarvest(seedling, player);
                        break;
                    }
                    if (player.HasWater())
                    {
                        HandleWater(seedling, player);
                    }
                    break;
                case WeedRoot weedRoot:
                    if (player.HasFlower())
                    {
                        player.AoeBomb();
                        player.UseFlower();
                    }
                    else
                    {
                        weedRoot.RipOut();
                    }

                    break;
            }
        }
    }


    private void HandleWaterPickup(Player player)
    {
        StartCoroutine(DelayedWaterPickup(player));
    }
    private IEnumerator DelayedWaterPickup(Player player)
    {
        player.PerformInteraction(Configs.Instance.Get.wateringDuration, InteractionType.Water);
        yield return new WaitForSeconds(Configs.Instance.Get.wateringDuration);
        player.AddWater();
    }

    private void HandleWater(Seedling seedling, Player player)
    {
        StartCoroutine(DelayedWater(seedling, player));
    }
    private IEnumerator DelayedWater(Seedling seedling, Player player)
    {
        player.PerformInteraction(Configs.Instance.Get.wateringDuration, InteractionType.Water);
        yield return new WaitForSeconds(Configs.Instance.Get.wateringDuration);
        player.UseWater();
        seedling.Water();
        player.HideCargoUI();
    }
    private void HandleHarvest(Seedling seedling, Player player)
    {
        StartCoroutine(DelayedHarvest(seedling, player));
    }
    private IEnumerator DelayedHarvest(Seedling seedling, Player player)
    {
        player.PerformInteraction(Configs.Instance.Get.harvestDuration, InteractionType.Harvest);
        yield return new WaitForSeconds(Configs.Instance.Get.harvestDuration);
        player.AddFlower();
        if (seedling != null)
            seedling.DelayedDestroy();
    }

    private void HandleOnRemove(InteractiveRotatable obj)
    {
        if (rotatables.Contains(obj))
        {
            rotatables.Remove(obj);
            if (obj != null)
                Destroy(obj);
        }
    }

    private void SpawnSeedling(Player player)
    {
        StartCoroutine(SpawnSeedlingDelayed(player));


    }
    private IEnumerator SpawnSeedlingDelayed(Player player)
    {
        player.PerformInteraction(Configs.Instance.Get.spawnSeedDuration, InteractionType.Seed);
        yield return new WaitForSeconds(Configs.Instance.Get.spawnSeedDuration);
        Seedling seedling = Instantiate(seedlingPrefab, planetCenter);
        seedling.rotatingObject.rotation = player.rotatingObject.rotation;
        seedling.OnInteract += HandleInteractWithSeedling;
        seedling.OnRemove += HandleOnRemove;
        rotatables.Add(seedling);
        player.HideCargoUI();
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