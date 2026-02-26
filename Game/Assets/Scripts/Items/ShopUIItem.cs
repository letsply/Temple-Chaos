using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIItem : MonoBehaviour
{
    [SerializeField] GameObject goldDeficincyWarning;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] Image image;

    BaseItem[] items;
    GameObject itemHolder;
    Inventory inv;
    Sprite shopItemImage;

    int id;
    int price;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        inv = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

        itemHolder = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<BaseItem>().gameObject;

        items = itemHolder.GetComponents<BaseItem>();

        foreach (BaseItem item in items)
        {
            if (item.ItemID() == id)
            {
                nameText.text = item.ItemName();
                priceText.text = item.ItemPrice().ToString();
                price = item.ItemPrice();
                if (item.ItemImage() != null)
                {
                    shopItemImage = item.ItemImage();
                    image.sprite = shopItemImage;
                }
            }
        }
    }

    public void Buy()
    {
        for (int i = 0; i < inv.Items().Length;i++)
        {
            if (price > inv.Gold())
            {
                goldDeficincyWarning.SetActive(true);
                return;
            }
            else {
                if (inv.Items()[i] == 0)
                {
                    inv.SetItems(id, i);
                    inv.SetGold(inv.Gold() - price);
                    Destroy(gameObject);
                    return;
                }
            }
        }
        inv.InvFull();
    }

    public void SetID(int value)
    {
        id = value;
    }
}
