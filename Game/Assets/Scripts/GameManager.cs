using System.IO;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]GameObject player;
    Inventory inv;
    string path;
    int tutorialsCompleted;
    Save save;
    int levelUnlocked;

    public int TutorialCompleted() => tutorialsCompleted;
    public int LevelUnlocked() => levelUnlocked;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (GameObject.FindGameObjectsWithTag("GameManager").Length > 1)
        {
            Destroy(gameObject);
        }
    }
    public void SaveFile()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            
            save = new Save();

            save.LevelUnlocked = levelUnlocked;
            inv = player.GetComponent<Inventory>();
            save.Gold = inv.Gold();
            save.Inventory = inv.Items();
            save.HasCompleteTutorial = tutorialsCompleted;

            path = Application.persistentDataPath + "/" + "Save.Json";
            CreatejsonFile();
        }
        else {
            Debug.Log("was zur zerbrochenen zeranfeldplatte wilst du hier speicher");
        }

        
    }

    private void CreatejsonFile()
    {
        string saveItemData = JsonUtility.ToJson(save);
        File.WriteAllText(path, saveItemData);
    }

    public void Load(int SceneToLoad)
    {
        StartCoroutine(LoadSceneWithDelay(SceneToLoad));
    }

    void LoadSave()
    {
        path = Application.persistentDataPath + "/" + "Save.Json";

        if (File.Exists(path))
        {
            string itemData = File.ReadAllText(path);
            save = JsonUtility.FromJson<Save>(itemData);

            levelUnlocked = save.LevelUnlocked;
            tutorialsCompleted = save.HasCompleteTutorial;

            inv.SetGold(save.Gold);
            for (int i = 0; i < save.Inventory.Length; i++)
            {
                inv.SetItems(save.Inventory[i], i);
            }

            if (save.HasCompleteTutorial == 0)
            {
                player.GetComponent<TimeChange>().enabled = false;
            }
        }
    }


    public void CompleteTutorial()
    {
        tutorialsCompleted++;
        if (levelUnlocked == 0)
        {
            levelUnlocked = 1;
        }
        SaveFile();
        Load(1);
    }

    IEnumerator LoadSceneWithDelay(int sceneIndex)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        asyncLoad.allowSceneActivation = true;

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        while(player == null)
        {
            player = GameObject.FindWithTag("Player");
            inv = player.GetComponent<Inventory>();
            if (player != null)
            {
                yield return null;
            }
        }
        Time.timeScale = 1.0f;
        Scene scene = SceneManager.GetSceneByBuildIndex(sceneIndex);
        SceneManager.SetActiveScene(scene);
        LoadSave();
    }


    public class Save
    {
        public int Gold;
        public int HasCompleteTutorial;
        public int[] Inventory;
        public int LevelUnlocked;
    }
}
