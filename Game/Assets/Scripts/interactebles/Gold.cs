using UnityEngine;

public class Gold : MonoBehaviour
{
    [SerializeField] private int _value = 0;

    [SerializeField] public GameObject goldDust;
    public int Value() { return _value; }
   
}
