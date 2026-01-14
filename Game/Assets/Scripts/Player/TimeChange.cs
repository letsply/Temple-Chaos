using UnityEngine;
using UnityEngine.InputSystem;

public class TimeChange : MonoBehaviour
{

    public GameObject Past;
    public GameObject Present;

    public void Activate(InputAction.CallbackContext context)
    {
        if(Past.activeSelf)
        {
            Present.SetActive(true);
            Past.SetActive(false);
        }
        else
        {
            Present.SetActive(false);
            Past.SetActive(true);
        }
    }
}
