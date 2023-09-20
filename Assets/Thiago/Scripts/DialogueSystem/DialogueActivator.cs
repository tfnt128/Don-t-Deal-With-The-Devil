using UnityEngine;

public class DialogueActivator : MonoBehaviour, Interectible
{
    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private PlayerDialogue player;
    public bool isActivate { private get; set; } = false;
    public int id{ get; private set; }
    public bool canInteract { private get; set; } = true;

    public void UpdateDialogueObejct(DialogueObject dialogueObject)
    {
        this.dialogueObject = dialogueObject;
    }
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isActivate && canInteract)
        {
            Interect(player);
            isActivate = true;
        }
    }

    public void Interect(PlayerDialogue player)
    {
        foreach(DialogueResponseEvents responseEvents in GetComponents<DialogueResponseEvents>())
        {
            if(responseEvents.DialogueObject == dialogueObject)
            {
                player.DialogueUI.AddResponseEvents(responseEvents.Events);
                break;
            }
        }
        player.DialogueUI.ShowDialogue(dialogueObject);
    }
}