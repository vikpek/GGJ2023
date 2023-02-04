using UnityEngine;
public class Rotatable : MonoBehaviour, IRotatable
{
    [SerializeField] private Transform rotatingObject;
    public void AddRotation(float degrees)
    {
        rotatingObject.transform.Rotate(Vector3.back, degrees, Space.Self);
    }
}
public interface IRotatable
{
    void AddRotation(float degrees);
}