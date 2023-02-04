using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using Random = System.Random;
public class GameManager : MonoBehaviour
{
    [SerializeField] private Planet planetPrefab;
    [SerializeField] private Player playerPrefab;
    [SerializeField] private WeedRoot weedRootPrefab;
    [SerializeField] private int playerAmount = 2;

    private IRotatable planet;
    private List<Player> players = new();
    private List<WeedRoot> weedRoots = new();

    void Start()
    {
        planet = Instantiate(planetPrefab, transform);
        for (int i = 0; i < playerAmount; i++)
        {
            players.Add(Instantiate(playerPrefab, transform));
        }
    }

    Random rand = new();
    private WeedRoot SpawnWeedRoot()
    {
        var weedRootInstance = Instantiate(weedRootPrefab, transform);
        weedRootInstance.transform.Rotate(Vector3.back, rand.Next(0, 360), Space.Self);
        return weedRootInstance;
    }

    public float interval = 1.0f;  // The interval in seconds
    private float timer = 0.0f;  // The timer

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            weedRoots.Add(SpawnWeedRoot());
            timer = 0.0f;

        }


        planet.AddRotation(1);

        if (Input.GetKey(KeyCode.RightArrow))
            players[0].MoveClockwise();

        if (Input.GetKey(KeyCode.LeftArrow))
            players[0].MoveCounterClockwise();

        if (Input.GetKey(KeyCode.A))
            players[1].MoveClockwise();

        if (Input.GetKey(KeyCode.D))
            players[1].MoveCounterClockwise();



        // if(Input.GetKey(KeyCode.S))
        //
        //
        // if(Input.GetKey(KeyCode.DownArrow))
        //
    }
}