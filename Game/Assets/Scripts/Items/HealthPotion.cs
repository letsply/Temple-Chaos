using UnityEngine;

public class HealthPotion : BaseItem
{
    [SerializeField] Sprite potionImage;
    void Awake()
    {
        itemName = "HealthPotion";
        itemIndex = 4;
        itemPrice = 50;
        itemImage = potionImage;
        uses = 0;

        if (uses == 0)
        {
            itemInfo = $"this is a HealthPotion and remember dont use it while on full health it dosnt work{uses + 1} use.";
        }
        else
        {
            itemInfo = $"this is a HealthPotion and remember dont use it while on full health it dosnt work {uses + 1} uses.";
        }

    }
    public override void UseItem()
    {
        Health health = GetComponentInParent<Health>();
        if (health.Hearts() < 3)
        {
            health.TakeDamage(-1);
        }
    }
}
