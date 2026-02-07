using System.Net.Security;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class TimeChange : MonoBehaviour
{
    [Header("Past&Present")]
    [SerializeField]private GameObject _past;
    [SerializeField]private GameObject _present;

    [Header("Light&Volume")]
    [SerializeField] private GameObject _globalLight;
    [SerializeField] private GameObject _globalBGLight;

    [SerializeField] private GameObject _pastVolume;
    [SerializeField] private GameObject _presentVolume;

    [SerializeField] private GameObject _flash;
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI m_warning;

    private RoomSystem roomSystem;
    private bool isInOverlayArea;
    private bool _canSwitch = true;

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
        if(isInOverlayArea == false && _canSwitch)
        {
            StartCoroutine(Flash(15, 1.5f));

            if (_past.activeSelf)
            {
                _present.SetActive(true);
                _past.SetActive(false);

                _presentVolume.SetActive(true);
                _pastVolume.SetActive(false);

                GetComponentInChildren<Light2D>().enabled = true;

                _globalLight.GetComponent<Light2D>().color = new Color(0.81f, 0.81f, 0.95f);
                _globalBGLight.GetComponent<Light2D>().color = new Color(0.81f, 0.81f, 0.95f);

                _globalBGLight.GetComponent<Light2D>().intensity = 0.1f;
                _globalLight.GetComponent<Light2D>().intensity = 0.3f;

            }
            else
            {
                _present.SetActive(false);
                _past.SetActive(true);

                GetComponentInChildren<Light2D>().enabled = false;

                _globalLight.GetComponent<Light2D>().color = new Color(0.93f, 0.92f, 0.75f);
                _globalBGLight.GetComponent<Light2D>().color = new Color(0.93f, 0.92f, 0.75f);

                _globalBGLight.GetComponent<Light2D>().intensity = 0.2f;
                _globalLight.GetComponent<Light2D>().intensity = 0.7f;

                _presentVolume.SetActive(false);
                _pastVolume.SetActive(true);

            }
        }
        else if(context.performed && isInOverlayArea == true)
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

    IEnumerator Flash(float aValue, float aTime)
    {
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            float radiusCurrent = _flash.GetComponent<Light2D>().pointLightOuterRadius;
            float newradius = Mathf.Lerp(radiusCurrent, aValue, t);
            _flash.GetComponent<Light2D>().pointLightOuterRadius = newradius;

            _canSwitch = false;
            gameObject.GetComponent<PMovment>().enabled = false;
            _flash.GetComponent<Volume>().enabled = true;
            yield return null;
        }
        if (aValue != 0)
        {
            StartCoroutine(Flash(0, 1f));
        }

        gameObject.GetComponent<PMovment>().enabled = true;
        _flash.GetComponent<Volume>().enabled = false;
        _canSwitch = true;
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
