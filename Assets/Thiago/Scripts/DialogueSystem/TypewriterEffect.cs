using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] public float typewriterSpeed = 50f;

    public bool isRunning { get; private set; }
    public bool isFaster;
    public AudioSource speak1;
    private AudioSource[] audioSources;
    public AudioSource devilSpeak;
    public DialogueUI dialogue;
    private int currentAudioIndex = 0;
    private bool isPlayingAudio = false;
    private int lastAudioIndex = -1;

    private void Start()
    {
        audioSources = new AudioSource[] { speak1};
        ShuffleArray(audioSources);
    }

    private readonly List<Punctuation> punctuations = new List<Punctuation>()
    {
        new Punctuation(new HashSet<char>() {'.', '!', '?'}, 0.6f),
        new Punctuation(new HashSet<char>() { ',', ';', ':' }, 0.3f)
    };

    private Coroutine typingCoroutine;

    public void Run(string textToType, TMP_Text textLabel)
    {
        isFaster = false;
        typewriterSpeed = 20f;
        typingCoroutine = StartCoroutine(TypeText(textToType, textLabel));
    }

    public void updateSpeaker(AudioSource speak)
    {
        speak1 = speak;
    }

    public void Stop()
    {
        StopCoroutine(typingCoroutine);
        isRunning = false;
    }

    private bool doOnce = false;
    private float count = 2;
    private IEnumerator TypeText(string textToType, TMP_Text textLabel)
    {
        isRunning = true;
        textLabel.text = string.Empty;

        float t = 0;
        int charIndex = 0;

        while (charIndex < textToType.Length)
        {
            int lastCharIndex = charIndex;

            t += Time.deltaTime * typewriterSpeed;

            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);

            for (int i = lastCharIndex; i < charIndex; i++)
            {
                count++;
                bool isLast = i >= textToType.Length - 1;

                textLabel.text = textToType.Substring(0, i + 1);

                float random = Random.Range(1f, 2f);
                if (count > random)
                {
                    count = 0;
                    if (dialogue.isHuman)
                    {
                        speak1.Play();
                    }
                    else
                    {
                        devilSpeak.Play();
                    }
                    
                    
                }

                if (!isFaster)
                {
                    if (isPunctuation(textToType[i], out float waitTime) && !isLast && !isPunctuation(textToType[i + 1], out _))
                    {
                        yield return new WaitForSeconds(waitTime);
                    }
                }                
            }
            yield return null;
        }

        isRunning = false;
        //textLabel.text = textToType;
    }
    void ShuffleArray(AudioSource[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            AudioSource temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }

    private bool isPunctuation(char character, out float waitTime)
    {
        foreach (Punctuation punctuationCategory in punctuations)
        {
            if (punctuationCategory.Punctuations.Contains(character))
            {
                waitTime = punctuationCategory.WaitTime;
                return true;
            }
        }

        waitTime = default;
        return false;
    }
    private readonly struct Punctuation
    {
        public readonly HashSet<char> Punctuations;
        public readonly float WaitTime;

        public Punctuation(HashSet<char> punctuations, float waitTime)
        {
            Punctuations = punctuations;
            WaitTime = waitTime;
        }
    }

    
    void PlayRandomAudio()
    {
        int randomIndex;
        randomIndex = Random.Range(0, audioSources.Length);
        lastAudioIndex = randomIndex;
        audioSources[randomIndex].Play();
    }
}
