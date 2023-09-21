using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private DialogueUI dialogue;
    private HumansMovement _humansMovement;

    private void Start()
    {
        dialogue = FindObjectOfType<DialogueUI>();
        _humansMovement = FindObjectOfType<HumansMovement>();
        dialogue.UiHuman += _humansMovement.OnUiHuman;
        _humansMovement.humanSpawned += dialogue.OnHumanSpawned;
    }
}