using TMPro;
using UnityEngine;

public class ShopUIItem : MonoBehaviour
{
    [SerializeField] GameObject infoHolder;
    [SerializeField] BaseItem[] items;
    [SerializeField] TextMeshProUGUI nameText;
    GameObject itemHolder;

    string info;
    public int id;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
