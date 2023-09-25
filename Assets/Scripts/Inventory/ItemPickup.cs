using Unity.VisualScripting;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public static ItemPickup instance;

    InventoryManager inventory;
    GameManager gameManager;

    public string setItemName;
    public Item item;
    private bool isCollectable = false;
    [SerializeField] private AudioSource addItemSound;
    [SerializeField] private TextAsset inkJSON;

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        inventory = InventoryManager.instance;
        gameManager = GameManager.instance;
        if(gameManager.IsItemDestroyed(gameObject.name))
            Destroy(gameObject);
    }

    private void Update()
    {
        setItemName = ((Ink.Runtime.StringValue)DialogueManager.GetInstance().GetVariableState("setItem")).value;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
            isCollectable = true;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
            isCollectable = false;
    }

    void OnMouseDown()
    {
        if (isCollectable && inventory.items.Count < inventory.space)
            if (setItemName == item.name)
            {
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
                addItemSound.Play();
                Invoke("Collect", 0.2f);
            }
    }

    private void Collect()
    {
        inventory.Add(item);
        Destroy(gameObject);
    }

}
