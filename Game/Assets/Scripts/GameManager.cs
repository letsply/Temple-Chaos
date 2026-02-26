using System.IO;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    [SerializeField] float musicVolume;
    [SerializeField] float soundVolume;
    public AudioMixerGroup MusicGroup;
    public AudioMixerGroup soundGroup;

    public bool FlashbangMode = false;

    [SerializeField]GameObject player;
    [SerializeField] GameObject endOfDemoScreen;

    Save save;
    Inventory inv;

    string path;
    int tutorialsCompleted;
    int levelUnlocked;

    public int TutorialCompleted() => tutorialsCompleted;
    public int LevelUnlocked() => levelUnlocked;

    private void Awake()
    {
        path = Application.persistentDataPath + "/" + "Save.Json";

        DontDestroyOnLoad(this);
    }

    #region Settings

    public void ChangeMusicVolume(float value)
    {
        musicVolume = value * 100 - 80;
        MusicGroup.audioMixer.SetFloat("MusicVol", musicVolume);
    }

    public void ChangeSFXVolume(float value)
    {
        soundVolume = value * 100 - 80;
        MusicGroup.audioMixer.SetFloat("SoundVol", soundVolume);
    }

    public void ResetSave()
    {
        if (File.Exists(path))
        {
            File.Delete(path);
            tutorialsCompleted = 0;
            levelUnlocked = 0;
        }
    }

    public void ToggleFlashbangMode(bool value)
    {
        FlashbangMode = value;
    }
    #endregion

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

            if (tutorialsCompleted == 0)
            {
                player.GetComponent<TimeChange>().enabled = false;
            }
        }
        else
        {
            player.GetComponent<TimeChange>().enabled = false;
        }
    }

    public void EndOfDemo()
    {
        Time.timeScale = 0f;
        if (levelUnlocked == 1)
        {
            levelUnlocked = 2;
        }
        SaveFile();
        endOfDemoScreen.SetActive(true);
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

    public void CloseGame()
    {
        Application.Quit();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
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

        if (sceneIndex == 0)
        {
            Destroy(gameObject);
        }

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
