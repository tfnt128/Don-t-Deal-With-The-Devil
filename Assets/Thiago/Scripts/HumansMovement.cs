using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class HumansMovement : MonoBehaviour
{
    
        public delegate void humanSpawnedHandler(HumansMovement selfReference, Animator anim, DialogueObject newDialogue, DialogueObject escapeDialogue, AudioSource speak1);
        public event humanSpawnedHandler humanSpawned;
        
        
        [SerializeField] private float speed = 2.0f;
        [SerializeField] private DialogueObject characterDialogue;
        [SerializeField] private DialogueObject escapeDialogue;
        [SerializeField] private Animator talkAnim;
        
        private RandomPersons nextPerson;
        public bool escaped;
        private Transform posA;
        private Transform posB;
        private bool toB = true;
        private Animator anim;
        private Animator Fade;
        private CinemachineVirtualCamera virtualCam;
        private AudioSource speak;
        private Animator devilAnim;
        private DialogueUI dialogueUI;
        
        
        private void Start()
        {
            speak = GetComponentInChildren<AudioSource>();
            dialogueUI = FindObjectOfType<DialogueUI>();
            if (dialogueUI != null)
            {
                posA = dialogueUI.posA;
                posB = dialogueUI.posB;
                devilAnim = dialogueUI.devilAnimEscaped;
                HumansMovement selfReference = this;
                dialogueUI.OnHumanSpawned(selfReference,talkAnim, characterDialogue, escapeDialogue, speak);
            }
            anim = GetComponent<Animator>();
            nextPerson = FindObjectOfType<RandomPersons>();
            GameObject fadego = GameObject.FindGameObjectWithTag("Fade");
            Fade = fadego.GetComponent<Animator>();
            transform.position = posA.transform.position;
            virtualCam = GetComponentInChildren<CinemachineVirtualCamera>();


        }
        public void OnUiHuman(Transform posA, Transform posB)
        {
            this.posA = posA;
            this.posB = posB;
        }
        

        private void Update()
        {
            if (toB)
            {
                MoveToPosition(posB);
                dialogueUI.dialogueActivator.canInteract = false;
            }
            else if (escaped)
            {
                StartCoroutine(Escape());
            }
        }

        private void MoveToPosition(Transform target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            if (transform.position == target.position)
            {
                anim.SetBool("IsWalking", false);
                StartCoroutine(changeCam());
                toB = false;
            }
        }

        private bool doOnce = false;

        private IEnumerator changeCam()
        {
            if (!escaped)
            {
                yield return new WaitForSeconds(1f);
                Fade.SetTrigger("FadeIn");
                yield return new WaitForSeconds(.3f);
                virtualCam.Priority = 11;
                yield return new WaitForSeconds(.1f);
                Fade.SetTrigger("FadeOut");
                dialogueUI.dialogueActivator.canInteract = true;
            }
            else if (!doOnce)
            {
                doOnce = true;
                devilAnim.SetBool("Escaped", true);
                Fade.SetTrigger("FadeIn");
                yield return new WaitForSeconds(.4f);
                virtualCam.Priority = 9;
                Fade.SetTrigger("FadeOut");
            }
        }

        private IEnumerator Escape()
        {
            StartCoroutine(changeCam());
            yield return new WaitForSeconds(1.5f);
            anim.SetBool("Escaped", true);
            MoveToPosition(posA);
            if (transform.position == posA.position)
            {
                devilAnim.SetBool("Escaped", false);
                GameObject go = nextPerson.nextPerson();
                if (doOnce)
                {
                    doOnce = false;
                    Instantiate(go, transform.position, Quaternion.identity);
                    dialogueUI.dialogueActivator.canInteract = true;
                }

                Destroy(this.gameObject);
            }
        }
}
