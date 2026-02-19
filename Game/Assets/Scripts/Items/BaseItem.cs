using UnityEngine;

public abstract class BaseItem : MonoBehaviour
{
    protected string itemName = "Default";
    protected int itemIndex = 0;
    protected int itemPrice = 0;
    protected int uses = 0;
    protected bool inUse = false;

    protected string itemInfo = "info";

    public bool InUse() => inUse;
    public int ItemID() => itemIndex;
    public string ItemInfo() => itemInfo;
    public int ItemPrice() => itemPrice;
    public int ItemUses() => uses;
    public string ItemName() => itemName;

    public virtual void UseItem()
    {

    }
  
}
