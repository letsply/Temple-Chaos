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
    [SerializeField] GameObject _past;
    [SerializeField] GameObject _present;
    [SerializeField] GameObject _pastBG;
    [SerializeField] GameObject _presentBG;
    bool inPresent = true;

    [Header("Light&Volume")]
    [SerializeField] GameObject _globalLight;
    [SerializeField] GameObject _globalBGLight;

    [SerializeField] GameObject _pastVolume;
    [SerializeField] GameObject _presentVolume;

    [SerializeField] GameObject _flash;
    [Header("UI")]
    [SerializeField] TextMeshProUGUI m_warning;

    RoomSystem roomSystem;
    bool isInOverlayArea;
    bool _canSwitch = true;
    AudioSource backgroundMusic;

    public void findPastAndPresent(GameObject past,GameObject present)
    {
        GameManager gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        if (gameManager.FlashbangMode == false)
        {
            _flash.GetComponent<Light2D>().intensity = 2;
        }
        else {
            _flash.GetComponent<Light2D>().intensity = 20;
        }

        if (past == null || present == null)
        {
            _past = roomSystem.Rooms[roomSystem.CurentRoom].transform.Find("Past").gameObject;
            _present = roomSystem.Rooms[roomSystem.CurentRoom].transform.Find("Present").gameObject;
        }
        else 
        {
            _past = past;
            _present = present;
        }
        
    }


    public void Start()
    {
        backgroundMusic = GameObject.Find("BgMusic").GetComponent<AudioSource>();

        // find the roomSystem that has the rooms
        roomSystem = GameObject.Find("RoomManager").GetComponent<RoomSystem>();
        // get the past and present from the roomsystem
        findPastAndPresent(null,null);

    }

    public void NewRoom()
    {
        if (inPresent == true)
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

    public void TimeSwap()
    {
        StartCoroutine(Flash(15, 0.75f));

        if (_past.activeSelf)
        {
            _present.SetActive(true);
            _past.SetActive(false);

            _presentBG.SetActive(true);
            _pastBG.SetActive(false);

            _presentVolume.SetActive(true);
            _pastVolume.SetActive(false);

            backgroundMusic.volume /= 4;
            backgroundMusic.pitch = 0.95f;

            GetComponentInChildren<Light2D>().enabled = true;

            _globalLight.GetComponent<Light2D>().color = new Color(0.81f, 0.81f, 0.95f);
            _globalBGLight.GetComponent<Light2D>().color = new Color(0.81f, 0.81f, 0.95f);

            _globalBGLight.GetComponent<Light2D>().intensity = 0.1f;
            _globalLight.GetComponent<Light2D>().intensity = 0.3f;
            inPresent = true;
        }
        else
        {
            _present.SetActive(false);
            _past.SetActive(true);

            _presentBG.SetActive(false);
            _pastBG.SetActive(true);

            backgroundMusic.volume *= 4;
            backgroundMusic.pitch = 1f;

            GetComponentInChildren<Light2D>().enabled = false;

            _globalLight.GetComponent<Light2D>().color = new Color(0.93f, 0.92f, 0.75f);
            _globalBGLight.GetComponent<Light2D>().color = new Color(0.93f, 0.92f, 0.75f);

            _globalBGLight.GetComponent<Light2D>().intensity = 0.2f;
            _globalLight.GetComponent<Light2D>().intensity = 0.7f;

            _presentVolume.SetActive(false);
            _pastVolume.SetActive(true);
            inPresent = false;
        }
    }

    public void Activate(InputAction.CallbackContext context)
    {
        // swap between past and present
        if (isInOverlayArea == false && _canSwitch && context.performed && this.isActiveAndEnabled)
        {
            TimeSwap();
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
            GetComponent<Rigidbody2D>().linearDamping = 15;
            _flash.GetComponent<Volume>().enabled = true;
            yield return null;
        }
        if (aValue != 0)
        {
            StartCoroutine(Flash(0, 0.25f));

        }
        GetComponent<Rigidbody2D>().linearDamping = 0;
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
