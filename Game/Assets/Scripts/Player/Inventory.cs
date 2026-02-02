using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    [Header("Inv Array")]
    [SerializeField]private int[] _items = { 0,0,0,0,0 };
    [SerializeField] private bool[] _itemIsInUI = { false, false, false, false, false };
    public int[] Items() => _items;
    public void SetItems(int value,int i) { _items[i] = value; }

    private int _invSpace = 0;
    private int _gold = 0;
    private bool otherItem;

    [Header("Picking up Stuff")]
    [SerializeField]private float _pickingRange;
    [SerializeField] private LayerMask _itemLayer;
    [SerializeField] private bool _itemIsPickable;

    [Header("UI")]
    [SerializeField] private GameObject _invFullPopUp;
    [SerializeField] private GameObject _invUI;
    [SerializeField] private GameObject _invItem;
    [SerializeField] private GameObject _goldCount;
    [SerializeField] private GameObject _eObject;



    private void Start()
    {
        // get the saved items and setting them
        for (int i = 0; i < _items.Length; i++)
        {
            _items[i] = PlayerPrefs.GetInt($"Item{i}");
            _gold = PlayerPrefs.GetInt("Gold");

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
        // if yes show it otherwise dont if there is no other thing that is interacteble
        if (otherItem == false) { _eObject.SetActive(_itemIsPickable); }
        #endregion

        // Check if the player is looking in the inv
        if (_invUI.activeSelf)
        {
            int margin = 0;
            GameObject anchor = GameObject.Find("Anchor");

            // for every item in inv spawn the ui object if it isnt already existing
            for(int i = 0; i < _items.Length; i++)
            {
                margin -= 150;
                if (_items[i] != 0 && _itemIsInUI[i] == false)
                {
                    GameObject uiItem = Instantiate(_invItem, new Vector2(anchor.transform.position.x, anchor.transform.position.y + margin), new Quaternion(), anchor.transform);
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
                    else if (_invSpace == 0)
                    {
                        //if no space in inv show it
                        _invFullPopUp.SetActive(true);
                    }
                }
            }
            else
            {
                // if item = gold add the value and destroy the gold
                _gold += item.GetComponent<Gold>().Value();
                Destroy(item);
            }
           
        }
    }

    // method to save items and Gold
    public void Save()
    {
        for (int i = 0; i < _items.Length; i++)
        {
            int saveVal = _items[i];
            string saveName = $"Item{i}";
            PlayerPrefs.SetInt(saveName, saveVal);

            PlayerPrefs.SetInt("Gold", _gold);

            PlayerPrefs.Save();
        }
    }

}
