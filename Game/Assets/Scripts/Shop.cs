using UnityEngine;
using UnityEngine.InputSystem;

public class Shop : MonoBehaviour
{
    [SerializeField] GameObject shopUI;
    [SerializeField] GameObject shopUIItem;
    [SerializeField] RectTransform anchor;
    [SerializeField] BaseItem[] shopItems = { null, null, null };
    [SerializeField] BaseItem[] itemsAvailable;
    bool isInRange;
    bool isRandom;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {

        int margin = -100;
        for (int i = 0; i < shopItems.Length; i++)
        {
            int r = Random.Range(0, itemsAvailable.Length);
            shopItems[i] = itemsAvailable[r];

            margin += 200;

            GameObject uiItem = Instantiate(shopUIItem, anchor.transform);
            uiItem.GetComponent<ShopUIItem>().SetID(shopItems[i].ItemID());
            uiItem.GetComponent<RectTransform>().anchoredPosition += new Vector2(margin,0);
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
