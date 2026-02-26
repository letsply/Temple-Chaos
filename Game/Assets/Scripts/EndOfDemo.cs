using UnityEngine;
using UnityEngine.InputSystem;

public class EndOfDemo : MonoBehaviour
{
    bool isInRange;
    GameObject player;

    [SerializeField] GameObject keyWarning;
    public void Interaction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            bool hasKey = false;
            if (isInRange && player.GetComponent<Inventory>())
            {

                for (int i = 0; i < player.GetComponent<Inventory>().Items().Length; i++)
                {
                    if (player.GetComponent<Inventory>().Items()[i] == 5)
                    {
                        player.GetComponent<Inventory>().SetItems(0,i);
                        hasKey = true;
                        GameManager gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
                        gameManager.EndOfDemo();
                    }
                }
            }
            if (hasKey == false && isInRange)
            {
                keyWarning.SetActive(true);
            }
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
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
