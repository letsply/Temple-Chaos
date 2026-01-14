using System.Net.Security;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimeChange : MonoBehaviour
{
    [SerializeField]private GameObject _past;
    [SerializeField]private GameObject _present;
    private RoomSystem roomSystem;

    public void Start()
    {
        roomSystem = GameObject.Find("GameManager").GetComponent<RoomSystem>();

        _past = roomSystem.Rooms[roomSystem.CurentRoom].transform.Find("Past").gameObject;
        _present = roomSystem.Rooms[roomSystem.CurentRoom].transform.Find("Present").gameObject;
    }
    public void Activate(InputAction.CallbackContext context)
    {
        _past = roomSystem.Rooms[roomSystem.CurentRoom].transform.Find("Past").gameObject;
        _present = roomSystem.Rooms[roomSystem.CurentRoom].transform.Find("Present").gameObject;

        if (_past.activeSelf)
        {
            _present.SetActive(true);
            _past.SetActive(false);
        }
        else
        {
            _present.SetActive(false);
            _past.SetActive(true);
        }
    }
}
