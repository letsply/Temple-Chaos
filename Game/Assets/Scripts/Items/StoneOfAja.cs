using UnityEngine;
using UnityEngine.InputSystem;

public class StoneOfAja : MonoBehaviour
{
    private bool _isInRange = false;
    [SerializeField] private TimeChange _timeChange;
    [SerializeField] private GameObject _past;
    [SerializeField] private GameObject _present;

    public void Interaction(InputAction.CallbackContext context)
    {
        if (_isInRange)
        {
            _timeChange.enabled = true;
            _present.SetActive(false);
            _past.SetActive(true);
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _isInRange = true;
            collision.gameObject.GetComponent<Inventory>().IsInteracteble(_isInRange,true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _isInRange = false;
            collision.gameObject.GetComponent<Inventory>().IsInteracteble(_isInRange,false);
        }
    }
}
