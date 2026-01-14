using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using System.Linq;
using UnityEngine.Splines;

public class RoomSystem : MonoBehaviour
{
    public CinemachineSplineDolly CamPath;
    [SerializeField]private List<GameObject> Rooms = new List<GameObject>();
    private int _curentRoom;
    private GameObject _player;

    void Start()
    {
        // Get the instantiated Rooms and the Player
        _player = GameObject.FindWithTag("Player");
        Rooms = GameObject.FindGameObjectsWithTag("Room").ToList<GameObject>();
        _curentRoom = Rooms.Count - 1;

        Rooms.ForEach(room => room.SetActive(false));
        Rooms[Rooms.Count - 1].SetActive(true);
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
                Rooms[_curentRoom -= 1].SetActive(true);
            }
            if (Room.activeSelf && _player.transform.position.x <= Room.transform.position.x)
            {
                Room.SetActive(false);
                Rooms[_curentRoom += 1].SetActive(true);
            }
        }
        // Set the Camera path to the one of the room
        CamPath.Spline = Rooms[_curentRoom].GetComponentInChildren<SplineContainer>();
    }
}
