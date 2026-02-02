using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;


public class UIItem : MonoBehaviour
{
    [SerializeField] private GameObject m_infoHolder;
    [SerializeField] private BaseItem[] _items;
    [SerializeField] private TextMeshProUGUI _nameText;
    private GameObject m_itemHolder;

    private BaseItem _item;
    private string _info;
    private int _itemUses;
    public int _id;

    void Start()
    {

        m_itemHolder = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<BaseItem>().gameObject;

        _items = m_itemHolder.GetComponents<BaseItem>();

        foreach (BaseItem item in _items)
        { 
            if(item.ItemID() == _id)
            {
                _item = item;
                _info = item.ItemInfo();
                _nameText.text = item.ItemName();
            }
        }
    }

    public void Info()
    {
        m_infoHolder.SetActive(true);

        TextMeshProUGUI infoText = m_infoHolder.GetComponentInChildren<TextMeshProUGUI>();
        infoText.text = _info;
    }
    public void Use()
    {
        if (_itemUses != _item.ItemUses())
        {
            _item.UseItem();
            _itemUses += 1;
        }
        else
        {
            _item.UseItem();
            int i = 0;
            foreach (var item in GetComponentInParent<Inventory>().Items())
            {
                if (_id == item)
                {
                    GetComponentInParent<Inventory>().SetItems(0,i);
                    Destroy(gameObject);
                    break;
                }
                i++;
            }
        }
        
    }
    public void ThrowOut()
    {
        int i = 0;
        foreach (var item in GetComponentInParent<Inventory>().Items())
        {
            if (_id == item)
            {
                GetComponentInParent<Inventory>().SetItems(0, i);
                Destroy(gameObject);
                break;
            }
            i++;
        }
    }

}
