using System.Collections;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class TestItem : BaseItem
{
    [SerializeField] private GameObject m_TestText;

    void Start()
    {
        itemName = "Test";
        itemIndex = 1;
        itemPrice = 99999999;
        uses = 0;

        itemInfo = $"Test also it has {uses + 1} uses.";
    }
    public override void UseItem()
    {
        StartCoroutine(Wait(2));

    }

    IEnumerator Wait(float time)
    {
        m_TestText.SetActive(true);
        yield return new WaitForSeconds(time);
        m_TestText.SetActive(false);
    }

}
