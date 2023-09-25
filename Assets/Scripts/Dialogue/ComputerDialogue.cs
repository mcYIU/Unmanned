using UnityEngine;

public class ComputerDialogue : MonoBehaviour
{
    private DialogueManager dialogueManager;
    private bool playerInRange;
    Camera cam;
    public LayerMask mask;
    private int matState;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset Computer01_JSON;
    [SerializeField] private TextAsset Computer02_JSON;

    public void Awake()
    {
        if (dialogueManager != null)
            Debug.Log("Found more than one Dialogue Manager in the scene");
        dialogueManager = new DialogueManager();
        playerInRange = false;
    }

    private void Start()
    {
        cam = Camera.main;
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
                        if (matState==4)
                        {
                            TextAsset currentComputerDialogue02 = hit.collider.GetComponent<ComputerDialogue>().Computer02_JSON;
                            DialogueManager.GetInstance().EnterDialogueMode(currentComputerDialogue02);
                        }
                        else
                        {
                            TextAsset currentComputerDialogue01 = hit.collider.GetComponent<ComputerDialogue>().Computer01_JSON;
                            DialogueManager.GetInstance().EnterDialogueMode(currentComputerDialogue01);
                        }

            }
        }
    }
}