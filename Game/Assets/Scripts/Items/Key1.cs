using UnityEngine;

public class Key1 : BaseItem
{
    [SerializeField]GameObject onlyUsableAtDoor;
    void Awake()
    {
        itemName = "TutorialKey";
        itemIndex = 3;
        itemPrice = 25;
        uses = 0;

        itemInfo = $"this is a Key and it lets u open the door at the end of the tutorial";
      
    }
    public override void UseItem()
    {
       onlyUsableAtDoor.SetActive(true);   
    }
}
