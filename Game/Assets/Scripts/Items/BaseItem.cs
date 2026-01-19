using UnityEngine;

public abstract class BaseItem
{
    protected string itemName = "Default";
    protected int itemIndex = 1;
    protected int itemPrice = 0;

    protected string itemInfo = "info";

    public int ItemID() => itemIndex;

    public virtual void UseItem()
    {

    }
  
}
