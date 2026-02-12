using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Splines;

public class RoomSystem : MonoBehaviour
{
    public CinemachineSplineDolly CamPath;
    [SerializeField]private List<GameObject> _rooms = new List<GameObject>();

    int _curentRoom; 
    GameObject _player;

    public int CurentRoom => _curentRoom;
    public List<GameObject> Rooms => _rooms;

    void Start()
    {
        // Get the instantiated Rooms and the Player
        _player = GameObject.FindWithTag("Player");
        _rooms = GameObject.FindGameObjectsWithTag("Room").ToList<GameObject>();
        // sort rooms by x posiotion
        _rooms.Sort((x,y) => x.transform.position.x.CompareTo(y.transform.position.x));

        //Deactivate all rooms the player isnt in
        Rooms.ForEach(room => room.SetActive(false));
        Rooms[0].SetActive(true);
    }

    void Update()
    {
       // for each Room check if the player goes out of the room
        foreach (GameObject Room in Rooms)
        {
            int lenght = Room.GetComponent<DefaultRoom>().GetRoomLenght();
            if (Room.activeSelf && _player.transform.position.x >= Room.transform.position.x + lenght)
            {
                Room.SetActive(false);
                Rooms[_curentRoom += 1].SetActive(true);
                _player.GetComponent<TimeChange>().findPastAndPresent(null,null);
                _player.GetComponent<TimeChange>().NewRoom();
            }
            if (Room.activeSelf && _player.transform.position.x <= Room.transform.position.x)
            {
                Room.SetActive(false);
                Rooms[_curentRoom -= 1].SetActive(true);
                _player.GetComponent<TimeChange>().findPastAndPresent(null, null);
            }
        }
        // Set the Camera path to the one of the room
        CamPath.Spline = Rooms[_curentRoom].GetComponentInChildren<SplineContainer>();
    }
}
