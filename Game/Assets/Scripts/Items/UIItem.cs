using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;


public class UIItem : MonoBehaviour
{
    [SerializeField] GameObject m_infoHolder;
    [SerializeField] BaseItem[] _items;
    [SerializeField] TextMeshProUGUI _nameText;
    [SerializeField] Image image;
    GameObject m_itemHolder;

    BaseItem _item;
    Sprite itemImage;
    string _info;
    int _itemUses;
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
                if (item.ItemImage() != null)
                {
                    itemImage = item.ItemImage();
                    image.sprite = itemImage;
                }
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
        if (_itemUses != _item.ItemUses() )
        {
            _item.UseItem();
            _itemUses -= 1;
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
                    GetComponentInParent<PauseMenu>().gameObject.SetActive(false);
                    Time.timeScale = 1;
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
