using UnityEngine;
using UnityEngine.InputSystem;

public class StoneOfAja : MonoBehaviour
{
    private bool _isInRange = false;
    [SerializeField] private TimeChange _timeChange;
    [SerializeField] private GameObject _past;
    [SerializeField] private GameObject _present;

    public void Start()
    {
        GameManager gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        if (gameManager.TutorialCompleted() > 0)
        {
            Destroy(gameObject);
        }
    }

    public void Interaction(InputAction.CallbackContext context)
    {
        if (_isInRange)
        {
            _timeChange.enabled = true;
            _timeChange.findPastAndPresent(_past, _present);
            _timeChange.TimeSwap();

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
