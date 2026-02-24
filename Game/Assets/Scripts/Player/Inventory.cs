using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    [Header("Inv Array")]
    [SerializeField] int[] _items = { 0,0,0,0,0 };
    [SerializeField] bool[] _itemIsInUI = { false, false, false, false, false };
    public int[] Items() => _items;
    // sets specific slots to item and if its null sets it the same in the ui
    public void SetItems(int value,int i) { _items[i] = value; if (value == 0) { _itemIsInUI[i] = false; } }

    int _gold = 0;
    public int Gold() => _gold;
    public void SetGold(int value) {  _gold = value; }

    int _invSpace = 0;
    // if a interacteble isnt a item
    bool otherItem;

    [Header("Picking up Stuff")]
    [SerializeField] float _pickingRange;
    [SerializeField] LayerMask _itemLayer;
    [SerializeField] bool _itemIsPickable;

    [Header("UI")]
    [SerializeField] GameObject _invFullPopUp;
    [SerializeField] GameObject _invUI;
    [SerializeField] GameObject _invItem;
    [SerializeField] GameObject _goldCount;
    [SerializeField] GameObject _eObject;
    RectTransform anchor = null;


    private void Start()
    {
        for (int i = 0; i < _items.Length; i++)
        {
            if (_items[i] == 0)
            {
                _invSpace++;
            }
        }
    }

    private void Update()
    {
        #region Interactability
        // Check if item can be hold 
        _itemIsPickable = Physics2D.OverlapBox(transform.position,new Vector2(gameObject.GetComponent<CapsuleCollider2D>().size.x + _pickingRange, gameObject.GetComponent<CapsuleCollider2D>().size.y),0,_itemLayer);
        if (otherItem == false) { _eObject.SetActive(_itemIsPickable); }
        #endregion

        // Check if the player is looking in the inv
        if (_invUI.activeSelf)
        {
            int margin = 0;
            if (anchor == null)
            {
                anchor = GameObject.Find("Anchor").GetComponent<RectTransform>();
            }

            // for every item in inv spawn the ui object if it isnt already existing
            for(int i = 0; i < _items.Length; i++)
            {
                margin -= 60;
                if (_items[i] != 0 && _itemIsInUI[i] == false)
                {
                    GameObject uiItem = Instantiate(_invItem,  anchor.transform);
                    uiItem.GetComponent<RectTransform>().anchoredPosition += new Vector2(0,margin);
                    uiItem.GetComponent<UIItem>()._id = _items[i];
                    _itemIsInUI[i] = true;
                }
                else if(_items[i] == 0)
                {
                    _itemIsInUI[i] = false;
                }

            }
        }

        // show how much gold is in inv
        _goldCount.GetComponent<TextMeshProUGUI>().text = "Gold: " + _gold; 
    }

    // for other scripts to show if it is an Interacteble
    public void IsInteracteble(bool state, bool isOtherThanItem)
    {
        _eObject.SetActive(state);
        otherItem = isOtherThanItem;
    }

    public void PickItem(InputAction.CallbackContext context)
    {
        // if a item is Pickeble and [Button] is pressed
        if (_itemIsPickable && context.performed)
        {
            // Getting the gameObject of that item
            Collider2D hit = Physics2D.OverlapBox(transform.position, new Vector2(gameObject.GetComponent<CapsuleCollider2D>().size.x + _pickingRange, gameObject.GetComponent<CapsuleCollider2D>().size.y),0, _itemLayer);
            GameObject item = hit.gameObject;

            // if item isnt gold put it in inv
            if (item.GetComponent<BaseItem>())
            {
                for (int i = 0; i < _items.Length; i++)
                {
                    // setting the int in the inv array to the item id an destroying the item + removing itemSpace
                    if (_items[i] == 0)
                    {
                        _items[i] = item.GetComponent<BaseItem>().ItemID();
                        Destroy(item);
                        _invSpace--;
                        break;
                    }
                    
                }
                if (_invSpace == 0)
                {
                    //if no space in inv show it
                    InvFull();
                }
            }
            else
            {
                // if item = gold add the value and destroy the gold
                _gold += item.GetComponent<Gold>().Value();
                Instantiate(item.GetComponent<Gold>().goldDust,item.transform.position,Quaternion.identity);
                Destroy(item);
            }
           
        }
    }


    public void InvFull()
    {
        _invFullPopUp.SetActive(true);
    }
}
