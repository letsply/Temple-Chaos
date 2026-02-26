using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] GameObject BG1;
    [SerializeField] GameObject BG2;
    [SerializeField] GameObject BG3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartGame()
    {
        StartCoroutine(ZoomIn(1.5f, 1f));
    }

    public void ResetSaveGame()
    {
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().ResetSave();
    }
    
    public void Link()
    {
        Application.OpenURL("https://www.youtube.com/@ContextSensitive");
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    IEnumerator ZoomIn(float Zoom, float aTime)
    {
        float BG1zoom = BG1.transform.localScale.x;
        float BG2zoom = BG2.transform.localScale.x;
        float BG3zoom = BG3.transform.localScale.x;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            float newZoom1 = Mathf.Lerp(BG1zoom, Zoom * 2f, t);
            BG1.transform.localScale = new Vector3(newZoom1, newZoom1, 0);

            float newZoom2 = Mathf.Lerp(BG2zoom, Zoom * 1.5f, t);
            BG2.transform.localScale = new Vector3(newZoom2, newZoom2, 0);

            float newZoom3 = Mathf.Lerp(BG3zoom, Zoom, t);
            BG3.transform.localScale = new Vector3(newZoom3, newZoom3, 0);
            yield return null;
        }

        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().Load(1);
    }

}
