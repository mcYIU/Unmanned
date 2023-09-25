using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    private InventoryManager inventoryManager;
    private DialogueVariables dialogueVariables;

    public static List<Item> currentInventoryItems = new List<Item>();
    public static List<string> destroyedItems = new List<string>();

    private GameObject player;
    Dictionary<int, Vector2> savedPositions = new Dictionary<int, Vector2>();

    public Animator sceneTransition;
    public float trasitionTime = 2f;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    public static void SetCurrentInventoryItems(List<Item> items)
    {
        // Convert the item list to a array of names
        foreach (Item item in items)
            items.Add(item);

        // Include new items added to inventory
        foreach (Item newItem in items)
            if (!currentInventoryItems.Contains(newItem))
                currentInventoryItems.Add(newItem);

        // Discard items removed from inventory
        foreach (Item usedItem in currentInventoryItems.ToArray())
            if (!items.Contains(usedItem))
                currentInventoryItems.Remove(usedItem);
    }

    public static void AddInventoryItem(Item item)
    {
        if (!currentInventoryItems.Contains(item)) 
        {
            currentInventoryItems.Add(item);
            destroyedItems.Add(item.name);
        }
    }

    public static void RemoveInventoryItem(Item item)
    {
        if (currentInventoryItems.Contains(item))
            currentInventoryItems.Remove(item);
    }

    public bool IsItemDestroyed(string itemPickup)
    {
        return destroyedItems.Contains(itemPickup);
    }

    public void SavePosition(int sceneIndex, Vector2 lastPosition)
    {
        savedPositions[sceneIndex] = lastPosition;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        SoundManager.instance.Pause();
        DialogueManager.instance.dialogueIsPlaying = true;
    }

    public void ResumeGames()
    {
        Time.timeScale = 1f;
        SoundManager.instance.Resume();
        DialogueManager.instance.dialogueIsPlaying = false;
    }

    public void ExitGame()
    {
        SoundManager.instance.Restart();
        if (dialogueVariables != null)
            dialogueVariables.ClearVariables();
        currentInventoryItems.Clear();
        destroyedItems.Clear();
        savedPositions.Clear();
        Time.timeScale = 1f;
        LevelChanger.instance.DelayLoadScene("Start");
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = GameObject.FindWithTag("Player");
        if (player != null && savedPositions.ContainsKey(scene.buildIndex))
            player.transform.position = savedPositions[scene.buildIndex];
        else return;
    }
}
