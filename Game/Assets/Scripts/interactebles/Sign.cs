using UnityEngine;
using UnityEngine.InputSystem;

public class Sign : MonoBehaviour
{
    [SerializeField] GameObject _ui;
    private bool _isInRange;

    public void Interaction(InputAction.CallbackContext context)
    {
        if (_isInRange)
        {
            _ui.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void Close(InputAction.CallbackContext context)
    {
        if (_isInRange)
        {
            _ui.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void CloseUI()
    {
        if (_isInRange)
        {
            _ui.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _isInRange = true;
            collision.gameObject.GetComponent<Inventory>().IsInteracteble(_isInRange, true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _isInRange = false;
            collision.gameObject.GetComponent<Inventory>().IsInteracteble(_isInRange, false);
        }
    }
}
