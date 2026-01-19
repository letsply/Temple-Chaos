using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    private int[] _items = { 0,0,0,0,0 };
    private int _invSpace = 0;

    [SerializeField]private float _pickingRange;

    [SerializeField] private GameObject _invFullPopUp;
    [SerializeField] private GameObject _invUI;
    [SerializeField] private GameObject _invItem;

    [SerializeField]private LayerMask _itemLayer;

    private bool _itemIsPickable;

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
        _itemIsPickable = Physics2D.OverlapBox(transform.position,new Vector2(gameObject.GetComponent<CapsuleCollider2D>().size.x + _pickingRange, gameObject.GetComponent<CapsuleCollider2D>().size.y),_itemLayer);

        if (_invUI.activeSelf)
        {
            int margin = 0;
            GameObject anchor = GameObject.Find("Anchor");

            foreach (var item in _items)
            {
                margin -= 50;

                Instantiate(_invItem, new Vector2(0, margin), new Quaternion() , anchor.transform );

            }
        }
       
    }

    public void PickItem(InputAction.CallbackContext context)
    {
        if (_itemIsPickable && context.performed)
        {
            Collider2D hit = Physics2D.OverlapBox(transform.position, new Vector2(gameObject.GetComponent<CapsuleCollider2D>().size.x + _pickingRange, gameObject.GetComponent<CapsuleCollider2D>().size.y), _itemLayer);
            GameObject item = hit.gameObject;

            for (int i = 0; i < _items.Length; i++)
            {
                if (_items[i] != 0)
                {
                    _items[i] = item.GetComponent<BaseItem>().ItemID();
                }
                else if (_invSpace == 0)
                {
                    _invFullPopUp.SetActive(true);
                }
            }
        }

    }

}
