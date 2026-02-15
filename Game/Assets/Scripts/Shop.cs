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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int margin = 0;
        for (int i = 0; i < itemsAvailable.Length; i++)
        {
            int r = Random.Range(0, itemsAvailable.Length);
            shopItems[i] = itemsAvailable[r];

            GameObject uiItem = Instantiate(shopUIItem, new Vector2(anchor.transform.position.x + margin, anchor.transform.position.y), new Quaternion(), anchor.transform);
            uiItem.GetComponent<ShopUIItem>().id = shopItems[i].ItemID();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
