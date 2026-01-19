using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;

public class UIItem : MonoBehaviour
{
    [SerializeField] private GameObject m_info;
    void Start()
    {
        foreach (BaseItem baseItem in )
        {

        }
    }

    public void Info()
    {
        m_info.SetActive(true);

        TMPro.TextMeshPro infoText = m_info.GetComponentInChildren<TMPro.TextMeshPro>();
        infoText.text = "";

    }
}
