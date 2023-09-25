using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    private DialogueManager dialogueManager;
    private bool playerInRange;
    Camera cam;
    public LayerMask mask;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    public void Awake()
    {
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

    private void OnMouseUpAsButton()
    {
        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying)
        {
            TextAsset currentDialogue = gameObject.GetComponent<DialogueTrigger>().inkJSON;
            DialogueManager.GetInstance().EnterDialogueMode(currentDialogue);
        }
    }
}