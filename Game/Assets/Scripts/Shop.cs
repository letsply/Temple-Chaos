using UnityEngine;
using UnityEngine.InputSystem;

public class Shop : MonoBehaviour
{
    [SerializeField] GameObject shopUI;
    [SerializeField] GameObject shopUIItem;
    [SerializeField] GameObject anchor;
    [SerializeField] BaseItem[] shopItems = { null, null, null };
    [SerializeField] BaseItem[] itemsAvailable;
    bool isInRange;
    bool isRandom;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        int margin = 0;
        for (int i = 0; i < shopItems.Length; i++)
        {
            int r = Random.Range(0, itemsAvailable.Length);
            shopItems[i] = itemsAvailable[r];

            margin += 225;

            GameObject uiItem = Instantiate(shopUIItem, new Vector2(anchor.transform.localPosition.x + margin, anchor.transform.position.y), new Quaternion(), anchor.transform);
            uiItem.GetComponent<ShopUIItem>().SetID(shopItems[i].ItemID());
        }
    }

    public void Interaction(InputAction.CallbackContext context)
    {
        if (isInRange)
        {
            shopUI.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void close()
    {
        shopUI.SetActive(false);
        Time.timeScale = 1f;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<Inventory>().IsInteracteble(true,true);
            isInRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<Inventory>().IsInteracteble(false, true);
            isInRange = false;
        }
    }
}
