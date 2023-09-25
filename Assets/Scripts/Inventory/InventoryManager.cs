using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    #region Singleton
    public static InventoryManager instance { get; private set; }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
            Destroy(this);
    }
    #endregion
    
    // limit space of inventory
    public List<Item> items = new List<Item>();
    public int space = 1;

    // create event when inventory item changes
    public delegate void OnItemChanged();
    public OnItemChanged itemChanged;

    public string removeItem;

    private void Start()
    {
        LoadState();
    }

    private void Update()
    {
        removeItem = ((Ink.Runtime.StringValue)DialogueManager.GetInstance().GetVariableState("setItem")).value;
        if (removeItem == "")
            foreach (var item in items)
                RemoveItem(item);
    }

    public void Add(Item item)
    {
        if(items.Count >= space)
            return; 
        items.Add(item);
        itemChanged?.Invoke();
        SaveState(item);
        InventoryUI.instance.AddItem(item);
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
        InventoryUI.instance.ClearItem();
        itemChanged?.Invoke();
        SaveState(item, false);
    }

    void SaveState(Item item, bool isAdded = true)
    {
        if (isAdded)
            GameManager.AddInventoryItem(item);
        else GameManager.RemoveInventoryItem(item);
    }

    public void LoadState()
    {
        items.Clear();
        List<Item> heldItems = new List<Item>(GameManager.currentInventoryItems);
        foreach (Item heldItem in heldItems.ToArray())
            Add(heldItem);
    }
}
