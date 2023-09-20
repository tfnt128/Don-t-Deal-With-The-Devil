using UnityEngine;

[CreateAssetMenu(menuName = "dialogue/dialogueObject")]
public class DialogueObject : ScriptableObject
{
    [SerializeField] [TextArea] private string[] dialogue;
    [SerializeField] private Response[] responses;


    public string[] Dialogue => dialogue;
    public bool HasResponses => Responses != null && Responses.Length > 0;
    public bool IsDevil;
    public bool WrongAnswer;
    public bool devilAngryTalk;
    public DialogueObject Continue ;
    public Response[] Responses => responses;
}
