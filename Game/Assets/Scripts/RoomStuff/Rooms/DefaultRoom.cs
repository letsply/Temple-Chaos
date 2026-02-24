using UnityEngine;

public class DefaultRoom : MonoBehaviour
{
    protected string roomName = "TestRoom1";
    protected string roomType = "Undefined";
    protected int roomLenght = 33;

    public int GetRoomLenght() {  return roomLenght; }
}
