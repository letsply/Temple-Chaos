using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;

public class LayeredCam : MonoBehaviour
{
    private List<GameObject> _camPaths = new List<GameObject>();
    [SerializeField] private float[] m_hights;

    private void Start()
    {
        for (int i = 0;i < transform.childCount; i++)
        {
            _camPaths.Add( transform.GetChild(i).gameObject );
        }
    }

    private void Update()
    {
        for (int i = 0; i < _camPaths.Count; i++)
        {
            float playerPosY = GameObject.FindGameObjectWithTag("Player").transform.position.y;
            if (i + 1 < _camPaths.Count)
            {
                if (playerPosY <= m_hights[i] && playerPosY > m_hights[i + 1])
                {
                    _camPaths[i].SetActive(true);
                }
                else
                {
                    _camPaths[i].SetActive(false);
                }
            }
            else {
                if (playerPosY <= m_hights[i] )
                {
                    _camPaths[i].SetActive(true);
                }
                else
                {
                    _camPaths[i].SetActive(false);
                }
            }
            
        }
    }
}
