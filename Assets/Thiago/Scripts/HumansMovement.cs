using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class HumansMovement : MonoBehaviour
{
        [SerializeField] private Transform posA;
        [SerializeField] private Transform posB;
        [SerializeField] private float speed = 2.0f;
        [SerializeField] private Animator devilAnim;
        public bool escaped { private get; set; }
        
        private Vector3 inicialPos;
        private bool toB = true;
        private Animator anim;
        private GameObject TableCam;
        private Animator Fade;
        private CinemachineVirtualCamera virtualCam;
    
        private void Start()
        {
            GameObject fadego = GameObject.FindGameObjectWithTag("Fade");
            Fade = fadego.GetComponent<Animator>();
            anim = GetComponent<Animator>();
            inicialPos = posA.transform.position;
            transform.position = inicialPos;
            TableCam = GameObject.FindGameObjectWithTag("TableCam");
            virtualCam = TableCam.GetComponent<CinemachineVirtualCamera>();
        }
    
        private void Update()
        {
            if (toB)
            {
                transform.position = Vector3.MoveTowards(transform.position, posB.position, speed * Time.deltaTime);
    
                if (transform.position == posB.position)
                {
                    anim.SetBool("IsWalking", false);
                    StartCoroutine(changeCam());
                    toB = false;
                }
            }
            if(escaped)
            {
                StartCoroutine(Escape());
                
            }
        }

        private bool doOnce = false;
        private int cont;
        IEnumerator changeCam()
        {
            if (!escaped)
            {
                yield return new WaitForSeconds(1f);
                Fade.SetTrigger("FadeIn");
                yield return new WaitForSeconds(.3f);

                if (virtualCam.Priority == 11)
                {
                    virtualCam.Priority = 9;
                }
                else
                {
                    virtualCam.Priority = 11;
                }
                
                Fade.SetTrigger("FadeOut");
            }
            else
            {
                if (!doOnce)
                {
                    doOnce = true;
                    devilAnim.SetBool("Escaped", true);
                    Fade.SetTrigger("FadeIn");
                    yield return new WaitForSeconds(.4f);
                    virtualCam.Priority = 9;
                    Fade.SetTrigger("FadeOut");
                    cont++;

                }
            }
        }

        IEnumerator Escape()
        {
            StartCoroutine(changeCam());
            yield return new WaitForSeconds(1.5f);
            anim.SetBool("Escaped", true);
            transform.position = Vector3.MoveTowards(transform.position, posA.position, speed * Time.deltaTime);
    
            if (transform.position == posA.position)
            {
                devilAnim.SetBool("Escaped", false);
                Destroy(this.gameObject);
            }
        }
}
