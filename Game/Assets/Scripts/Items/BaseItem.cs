using UnityEngine;

public abstract class BaseItem : MonoBehaviour
{
    protected string itemName = "Default";
    protected int itemIndex = 1;
    protected int itemPrice = 0;
    protected int uses = 0;

    protected string itemInfo = "info";

    public int ItemID() => itemIndex;
    public string ItemInfo() => itemInfo;
    public int ItemUses() => uses;
    public string ItemName() => itemName;

    public virtual void UseItem()
    {

    }
  
}
