using UnityEngine;

public class CircuitBreakerDialogue : MonoBehaviour
{
    private DialogueManager dialogueManager;
    private bool playerInRange;
    Camera cam;
    public LayerMask mask;
    public Item keyItem;
    private InventoryManager inventoryManager;


    [Header("Ink JSON")]
    [SerializeField] private TextAsset CircuitBreaker01_JSON;
    [SerializeField] private TextAsset CircuitBreaker02_JSON;

    public void Awake()
    {
        if (dialogueManager != null)
            Debug.Log("Found more than one Dialogue Manager in the scene");
        dialogueManager = new DialogueManager();
        inventoryManager = new InventoryManager();
        playerInRange = false;
    }

    private void Start()
    {
        cam = Camera.main;
        if(keyItem != null)
            Debug.Log(keyItem.name + "is needed.");
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
            playerInRange = true;
    }


    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
            playerInRange = false;
    }

    private void Update()
    {
        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying)
        {
            Vector2 mousePos = Input.mousePosition;
            mousePos = cam.ScreenToWorldPoint(mousePos);

            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit;
                hit = Physics2D.Raycast(mousePos, Vector2.down);
                if (hit.collider.CompareTag("Interactable"))
                    if (hit.collider.name == gameObject.name)
                        if (inventoryManager.items.Contains(keyItem))
                            DialogueManager.GetInstance().EnterDialogueMode(CircuitBreaker02_JSON);
                        else
                            DialogueManager.GetInstance().EnterDialogueMode(CircuitBreaker01_JSON);

            }
        }
    }
}