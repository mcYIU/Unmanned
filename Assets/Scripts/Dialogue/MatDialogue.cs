using UnityEngine;

public class MatDialogue : MonoBehaviour
{
    private DialogueManager dialogueManager;
    private bool playerInRange;
    Camera cam;
    public LayerMask mask;

    public bool isMatHit = false;
    private SpriteRenderer matColor;
    private Animator matAnim;
    private Rigidbody2D matRB;
    [SerializeField] private AudioSource matFlapSound;
    [SerializeField] private AudioSource matHitSound;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    public void Awake()
    {
        playerInRange = false;
    }

    private void Start()
    {
        cam = Camera.main;
        matColor = gameObject.GetComponent<SpriteRenderer>();
        matAnim = gameObject.GetComponent<Animator>();
        matRB = gameObject.GetComponent<Rigidbody2D>();
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
        IsMatHit(isMatHit);

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
                    {
                        TextAsset currentDialogue = hit.collider.GetComponent<MatDialogue>().inkJSON;
                        DialogueManager.GetInstance().EnterDialogueMode(currentDialogue);
                    }
            }
        }
    }

    void IsMatHit(bool isHit)
    {
        isHit = ((Ink.Runtime.BoolValue) DialogueManager.GetInstance().GetVariableState("isMatHit")).value;
        if (isHit)
        {
            matHitSound.Play();
            matColor.color = Color.red;
            matAnim.enabled = false;
            matRB.bodyType = RigidbodyType2D.Dynamic;
            matFlapSound.Stop();
        }
    }
}