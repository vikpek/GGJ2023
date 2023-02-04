using System;
using System.Collections;
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
    [SerializeField] private int playerAmount = 2;
    public float SpawnInterval = 3.0f;

    private IRotatable planet;
    private List<Player> players = new();
    private List<WeedRoot> weedRoots = new();

    private float timer = 0.0f;

    private Action OnWeedGrow = delegate { };

    void Start()
    {
        InvokeRepeating("SpawnTick", 0f, 3f);
        planet = Instantiate(planetPrefab, planetCenter);
        // for (int i = 0; i < playerAmount; i++)
        // {
        //     players.Add(Instantiate(playerPrefab, planetCenter));
        // }
    }
    void SpawnTick()
    {
        // timer += Time.deltaTime;
        // if (timer >= SpawnInterval)
        // {
        OnWeedGrow();
        var weedRoot = SpawnWeedRoot();
        OnWeedGrow += weedRoot.Grow;
        weedRoots.Add(weedRoot);
        //     timer = 0.0f;
        // }
    }

    Random rand = new();
    private WeedRoot SpawnWeedRoot()
    {
        var weedRootInstance = Instantiate(weedRootPrefab, planetCenter);
        weedRootInstance.transform.Rotate(Vector3.back, rand.Next(0, 360), Space.Self);
        return weedRootInstance;
    }



    void Update()
    {
        planet.AddRotation(1);

        // if (Input.GetKey(KeyCode.RightArrow))
        //     players[0].MoveClockwise();

        // if (Input.GetKey(KeyCode.LeftArrow))
        //     players[0].MoveCounterClockwise();

        // if (Input.GetKey(KeyCode.A))
        //     players[1].MoveClockwise();

        // if (Input.GetKey(KeyCode.D))
        //     players[1].MoveCounterClockwise();

    }
}