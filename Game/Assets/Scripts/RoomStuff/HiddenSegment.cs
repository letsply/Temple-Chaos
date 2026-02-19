using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

public class HiddenSegment : MonoBehaviour
{
    TilemapRenderer tR;
    float outOfArea = 0;
    bool inArea = false;

    private void Start()
    {
        tR = GetComponent<TilemapRenderer>();
    }


    IEnumerator FadeTo(float aValue, float aTime)
    {
        float alpha = tR.material.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
            tR.material.color = newColor;
            yield return null;
        }
    }

    private void Update()
    {
        if (inArea == false && outOfArea <= 0.5f) {
            outOfArea += Time.deltaTime;
        }
        if (inArea == false && outOfArea >= 0.5f)
        {
            StartCoroutine(FadeTo(1, 1));
        }
        Debug.Log(inArea);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            inArea = true;
            outOfArea = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            StartCoroutine(FadeTo(0, 1));
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            inArea = false;
        }
    }
}
