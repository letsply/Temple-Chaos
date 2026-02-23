using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EndofMapDoor : MonoBehaviour
{
    bool isInRange;
    GameObject player;

    [SerializeField] GameObject keyWarning;
    public void Interaction(InputAction.CallbackContext context)
    {
        bool hasKey = false;
        if (isInRange && player.GetComponent<Inventory>())
        {

            for (int i = 0; i < player.GetComponent<Inventory>().Items().Length; i++)
            {
                if (player.GetComponent<Inventory>().Items()[i] == 3)
                {
                    hasKey = true;
                    GameManager gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
                    gameManager.CompleteTutorial();
                }
            }
        }
        if (hasKey == false && isInRange) 
        {
            keyWarning.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            player = collision.gameObject;
            isInRange = true;
            player.GetComponent<Inventory>().IsInteracteble(true, true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isInRange = false;
            player.GetComponent<Inventory>().IsInteracteble(false, true);
        }
    }
}
