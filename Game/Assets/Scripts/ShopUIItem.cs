using TMPro;
using UnityEngine;

public class ShopUIItem : MonoBehaviour
{
    [SerializeField] GameObject infoHolder;
    [SerializeField] TextMeshProUGUI nameText;

    BaseItem[] items;
    GameObject itemHolder;
    Inventory inv;

    string info;
    int id;
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
                info = item.ItemInfo();
                nameText.text = item.ItemName();
            }
        }
    }
    public void Info()
    {
        infoHolder.SetActive(true);

        TextMeshProUGUI infoText = infoHolder.GetComponentInChildren<TextMeshProUGUI>();
        infoText.text = info;
    }

    public void Buy()
    {
        for (int i = 0; i < inv.Items().Length;i++)
        {
            if (inv.Items()[i] == 0)
            {
                inv.SetItems(id, i);
                Destroy(gameObject);
                return;
            }
        }
        inv.InvFull();
    }

    public void SetID(int value)
    {
        id = value;
    }
}
