using UnityEngine;

public class DistanceShow : MonoBehaviour
{
    [SerializeField] private Transform _obj1;
    [SerializeField] private Transform _obj2;
    
    void OnEnable()
    {
        Debug.Log(Vector3.Distance(_obj1.position, _obj2.position));
    }
}
