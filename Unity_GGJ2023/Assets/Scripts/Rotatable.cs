using UnityEngine;
public class Rotatable : MonoBehaviour, IRotatable
{
    [SerializeField] private Transform rotatingObject;
    public void AddRotation(int degrees)
    {
        rotatingObject.transform.Rotate(Vector3.back, degrees, Space.Self);
    }
}
public interface IRotatable
{
    void AddRotation(int degrees);
}