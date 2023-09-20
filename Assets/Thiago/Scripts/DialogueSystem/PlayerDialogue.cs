using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDialogue : MonoBehaviour
{
    [SerializeField] private DialogueUI dialogueUi;
    public DialogueUI DialogueUI => dialogueUi;
    public Interectible interectible { get; set; }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (interectible != null && !dialogueUi.isOpen)
            {
                interectible.Interect(this);
            }
        }        
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Interact"))
        {
            dialogueUi = other.GetComponentInChildren<DialogueUI>();
        }
    }

    private bool canMoveAgain;
    private void FixedUpdate()
    {
        if(dialogueUi != null)
        {
            if (dialogueUi.isOpen)
            {
                canMoveAgain = true;
            }
            else if (!dialogueUi.isOpen && canMoveAgain)
            {
                canMoveAgain = false;
            }
        }        
    }
}