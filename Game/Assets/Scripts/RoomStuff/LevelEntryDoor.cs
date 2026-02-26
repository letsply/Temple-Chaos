using UnityEngine;
using UnityEngine.InputSystem;

public class LevelEntryDoor : MonoBehaviour
{
    GameManager gameManager;
    bool isOpen = false;
    [SerializeField] int targetLevelNumber;
    [SerializeField] int targetLevelIndex;
    [SerializeField] GameObject Door;
    bool isInRange = false;

    void Update()
    {
        if (gameManager == null) { gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>(); }

        if (gameManager != null && gameManager.LevelUnlocked() >= targetLevelNumber)
        {
            isOpen = true;
            Door.SetActive(false);
        }
    }

    public void Interaction(InputAction.CallbackContext context)
    {
        if (isOpen && isInRange && context.performed)
        {
            gameManager.Load(targetLevelIndex);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isInRange = true;
            collision.GetComponent<Inventory>().IsInteracteble(true,true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isInRange = false;
            collision.GetComponent<Inventory>().IsInteracteble(true, false);
        }
    }
}
