using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    [SerializeField] private Planet planetPrefab;
    [SerializeField] private Player playerPrefab;

    [SerializeField] private int playerAmount = 2;


    private IRotatable planet;
    private List<IRotatable> players = new();

    void Start()
    {
        planet = Instantiate(planetPrefab, transform);
        for (int i = 0; i < playerAmount; i++)
        {
            players.Add(Instantiate(playerPrefab, transform));
        }
    }

    void Update()
    {
        planet.AddRotation(1);

        if (Input.GetKey(KeyCode.RightArrow))
            players[0].AddRotation(10);

        if (Input.GetKey(KeyCode.LeftArrow))
            players[0].AddRotation(-10);

        if (Input.GetKey(KeyCode.A))
            players[1].AddRotation(10);

        if (Input.GetKey(KeyCode.D))
            players[1].AddRotation(-10);
    }
}