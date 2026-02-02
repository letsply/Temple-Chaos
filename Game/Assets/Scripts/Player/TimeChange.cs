using System.Net.Security;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class TimeChange : MonoBehaviour
{
    [Header("Past&Present")]
    [SerializeField]private GameObject _past;
    [SerializeField]private GameObject _present;
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI m_warning;

    private RoomSystem roomSystem;
    private bool isInOverlayArea;

    public void Start()
    {
        // find the roomSystem that has the rooms
        roomSystem = GameObject.Find("GameManager").GetComponent<RoomSystem>();
        // get the past and present from the roomsystem
        _past = roomSystem.Rooms[roomSystem.CurentRoom].transform.Find("Past").gameObject;
        _present = roomSystem.Rooms[roomSystem.CurentRoom].transform.Find("Present").gameObject;

        if (_present.activeSelf && _past.activeSelf)
        {
            _past.SetActive(false);
            _present.SetActive(true);
        }
    }
    public void Activate(InputAction.CallbackContext context)
    {
        // get the past and present from the roomsystem
        _past = roomSystem.Rooms[roomSystem.CurentRoom].transform.Find("Past").gameObject;
        _present = roomSystem.Rooms[roomSystem.CurentRoom].transform.Find("Present").gameObject;

        // swap between past and present
        if(isInOverlayArea == false)
        {
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
        else
        {
            // if the player is in an area where time swap is disabled show warning
            StartCoroutine(FadeTo(1,1));
        }
        
    }

    IEnumerator FadeTo(float aValue, float aTime)
    {
        float alpha = m_warning.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
            m_warning.color = newColor;
            yield return null;
        }
        if (aValue != 0)
        {
            StartCoroutine(FadeTo(0, 1));
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "OverlapAreas")
        {
            isInOverlayArea = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "OverlapAreas")
        {
            isInOverlayArea = false;
        }
    }
}
