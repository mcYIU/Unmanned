using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class Player : MonoBehaviour
{
    PlayerMovement movement;
    public Camera mainCamera;
    public Interactable focus;

    void Start()
    {
        movement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit;
            Vector2 clickedPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            hit = Physics2D.Raycast(clickedPos, Vector2.zero);

            if (hit)
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null)
                    SetFocus(interactable);
            }
        }
    }

    void SetFocus(Interactable newFocus)
    {
        focus = newFocus;
        movement.FaceOn(newFocus);
    }

}
