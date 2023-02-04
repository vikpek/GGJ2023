using UnityEngine;
public class Rotatable : MonoBehaviour, IRotatable
{
    [SerializeField] private Transform rotatingObject;
    public void AddRotation(float degrees)
    {
        rotatingObject.transform.Rotate(Vector3.back, degrees, Space.Self);
    }    
    public void Reset(){
        rotatingObject.transform.SetPositionAndRotation(new Vector3(0,0,0), new Quaternion(0,0,0,0));
    }
}
public interface IRotatable
{
    void AddRotation(float degrees);
}