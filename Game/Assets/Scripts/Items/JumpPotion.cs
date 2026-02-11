using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.UI;
public class JumpPotion : BaseItem
{
    [SerializeField]GameObject ItemDuraBar;
    [SerializeField] Image ItemDura;
    float Duration = 0;
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
        StartCoroutine(JumpBoost(10,1.5f));
    }
    public void Update()
    {
        if (ItemDuraBar != null)
        {
            if (ItemDuraBar.activeSelf)
            {
                Duration -= 1 * Time.deltaTime;
                ItemDura.GetComponentInChildren<Image>().fillAmount = Duration / 10;
            }
        }
       
    }


    IEnumerator JumpBoost(float time,float jumpForceMultiplyer)
    {
        Duration = time;
        ItemDuraBar.SetActive(true);
        float startJumpForce = GetComponentInParent<PMovment>().JumpForce();

        GetComponentInParent<PMovment>().ChangeJumpForce(startJumpForce * jumpForceMultiplyer);
        yield return new WaitForSeconds(time);

        GetComponentInParent<PMovment>().ChangeJumpForce(startJumpForce);
        ItemDuraBar.SetActive(false);

        yield return null;
    }

}
