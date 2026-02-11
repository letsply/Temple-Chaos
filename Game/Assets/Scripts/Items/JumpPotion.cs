using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.UI;
public class JumpPotion : BaseItem
{
    [SerializeField]GameObject ItemDuraBar;
    void Start()
    {
        itemName = "JumpPotion";
        itemIndex = 2;
        itemPrice = 25;
        uses = 0;

        if (uses == 0)
        {
            itemInfo = $"this is a JumpPotion and it lets u Jump twice as high also it has {uses + 1} use.";
        }
        else {
            itemInfo = $"this is a JumpPotion and it lets u Jump twice as high also it has {uses + 1} uses.";
        }

    }
    public override void UseItem()
    {
        StartCoroutine(JumpBoost(10,1.75f));
    }

    IEnumerator JumpBoost(float time,float jumpForceMultiplyer)
    {
        ItemDuraBar.SetActive(true);
        float startJumpForce = GetComponentInParent<PMovment>().JumpForce();

        if (startJumpForce == GetComponentInParent<PMovment>().JumpForce())
        {
            for (float t = 1f; t > 0f; t -= Time.deltaTime / time)
            {
                GetComponentInParent<PMovment>().ChangeJumpForce(startJumpForce * jumpForceMultiplyer);
                ItemDuraBar.GetComponentInChildren<Image>().fillAmount = t;
            }
        }

        GetComponentInParent<PMovment>().ChangeJumpForce(startJumpForce);
        ItemDuraBar.SetActive(false);

        yield return null;
    }

}
