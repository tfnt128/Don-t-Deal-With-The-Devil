using System;
using System.Collections;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private TMP_Text textCunning;
    [SerializeField] private DialogueActivator dialogueActivator;
    [SerializeField] private Animator devilAnim;
    [SerializeField] private Animator humanAnim;
    [SerializeField] private Animator devilCam;
    [SerializeField] private Image cunningBar;
    [SerializeField] private Animator cunningBarAnim;
    [SerializeField] private Animator CunningBarParentAnim;
    [SerializeField] private DialogueObject[] escapeDialogues;
    [SerializeField] private HumansMovement humans;
    
    private int cunningPer;
    private float cunningAmount = 100f;
    private float duration = 2f;
    
    private ResponseHadler responseHadler;
    private TypewriterEffect typewriterEffect;

    public bool isHuman { get; private set; }
    public bool isOpen { get; private set; }

    
    private void Start()
    {
        typewriterEffect = GetComponent<TypewriterEffect>();
        responseHadler = GetComponent<ResponseHadler>();
    }

    private void Update()
    {
        textCunning.text = ((cunningBar.fillAmount) * 100).ToString("0.0") + "%";
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        isOpen = true;
        dialogueBox.SetActive(true);
        if (dialogueObject.IsDevil)
        {
            textLabel.color = Color.red;
            devilCam.SetBool("Appers", true);
            isHuman = false;
        }
        else
        {
            textLabel.color = Color.white;
            isHuman = true;
            if (!dialogueObject.WrongAnswer)
            {
                devilCam.SetBool("Appers", false);
            }
            else
            {
                StartCoroutine(angryDevilDelay());
            }
            
        }
        StartCoroutine(StepThroughtDialogue(dialogueObject));
    }

    public void AddResponseEvents(ResponseEvent[] responseEvents)
    {
        responseHadler.AddResponseEvents(responseEvents);
    }

    public bool isOver = false;
    private bool doOnce = false;
    
    private IEnumerator StepThroughtDialogue(DialogueObject dialogueObject)
    {
        
        isOver = false;
        doOnce = false;
        for (int i = 0; i < dialogueObject.Dialogue.Length; i++)
        {
            if (dialogueObject.IsDevil)
            {
                if (dialogueObject.devilAngryTalk)
                {
                    devilAnim.SetBool("IsAngry", true);
                }
                devilAnim.SetBool("IsSpeaking", true);
            }
            else
            {
                humanAnim.SetBool("IsSpeaking", true);
            }
            string dialogue = dialogueObject.Dialogue[i];

            yield return runTypingEffect(dialogue);

            textLabel.text = dialogue;


            devilAnim.SetBool("IsSpeaking", false);
            humanAnim.SetBool("IsSpeaking", false);
            yield return new WaitForSeconds(.5f);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
            isOver = false;
        }

        
        if (cunningBar.fillAmount <= 0)
        {
            humans.escaped = true;
        }
        
        if (dialogueObject.HasResponses)
        {
            humanAnim.SetBool("IsSpeaking", false);
            if (!isOver)
            {
                yield return new WaitUntil(() => isOver = true);
            }
            
            
            if (isOver)
            {
               // yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
                HideDialogueBox();
                responseHadler.ShowResponses(dialogueObject.Responses);
                isOver = false;
            }
            
        }

        if (dialogueObject.Continue != null)
        {
            ShowDialogue(dialogueObject.Continue);
        } 
        if(!dialogueObject.HasResponses && dialogueObject.Continue == null)
        {
            CloseDialogueBox();
            if (!doOnce && dialogueObject.WrongAnswer)
            {
                doOnce = true;
                StartCoroutine(cunningPlus());
            }
            
            devilCam.SetBool("Appers", false);
            devilAnim.SetBool("Escaped", false);
        }
    }

    private IEnumerator runTypingEffect(string dialogue)
    {
        typewriterEffect.Run(dialogue, textLabel);

        while (typewriterEffect.isRunning)
        {
            yield return null;

            if (Input.GetKeyDown(KeyCode.E))
            {
                typewriterEffect.Stop();
                StartCoroutine(isOverCoroutine());
            }
        }
    }

    IEnumerator isOverCoroutine()
    {
        yield return new WaitForSeconds(1f);
        if (!typewriterEffect.isRunning)
        {
            isOver = true;
        }
    }

    public void HideDialogueBox()
    {
        dialogueBox.SetActive(false);
    }
    public void UnhideDialogueBox()
    {
        dialogueBox.SetActive(true);
    }
    public void CloseDialogueBox()
    {
        textLabel.text = string.Empty;
        StartCoroutine(dialogueBoxFadeOut());
       // if (dialogueActivator != null)
      //  {
      //      dialogueActivator.dialogueEnded = true;
      //  }
    }
    private IEnumerator dialogueBoxFadeOut()
    {
        
        yield return new WaitForSeconds(1f);
        isOpen = false;
        dialogueBox.SetActive(false);
        dialogueActivator.isActivate = false;
    }
    private IEnumerator angryDevilDelay()
    {
        
        yield return new WaitForSeconds(.5f);
        devilAnim.SetBool("IsAngry", false);
        devilAnim.SetBool("Escaped", true);
        
    }

    private IEnumerator cunningPlus()
    {
        CunningBarParentAnim.SetTrigger("Act");
        yield return new WaitForSeconds(.5f);
        cunningBarAnim.SetTrigger("Act");
        yield return new WaitForSeconds(1.5f);
        
        float startTime = Time.time;
        float endTime = startTime + duration;

        while (Time.time < endTime)
        {
            float elapsedTime = Time.time - startTime;
            float lerpAmount = elapsedTime / .5F;
            cunningBar.fillAmount = Mathf.Lerp(cunningAmount / 100, (cunningAmount - 50) / 100, lerpAmount);
            yield return null;
        }
        
        cunningAmount -= 50f;
        cunningBar.fillAmount = cunningAmount / 100;
        CunningBarParentAnim.SetTrigger("Back");
        yield return new WaitForSeconds(1f);
        if (cunningBar.fillAmount <= 0)
        {
            dialogueActivator.canInteract = false;
            ShowDialogue(escapeDialogues[dialogueActivator.id]);
        }
        
    }
}
