using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Rotatable : MonoBehaviour, IRotatable
{
    [SerializeField] private Transform rotatingObject;

    void Start()
    {

    }

    void Update()
    {

    }
    public void AddRotation(int degrees)
    {
        rotatingObject.transform.Rotate(Vector3.back, degrees, Space.Self);
    }
}
public interface IRotatable
{
    void AddRotation(int degrees);
}