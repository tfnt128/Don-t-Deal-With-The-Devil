using System;
using UnityEngine;

public class TalkAnimator : MonoBehaviour
{
    [SerializeField] private Sprite[] talkingSprites;
    private SpriteRenderer spriteRenderer;

    private bool isTalking;
    private float nextFrameTime;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isTalking)
        {
            AnimateTalking();
        }
        else
        {
            spriteRenderer.sprite = talkingSprites[0];
        }
    }

    public void Talk(bool talking)
    {
        isTalking = talking;
    }


    private void AnimateTalking()
    {
        int frameIndex = Mathf.FloorToInt(Time.time * 5) % talkingSprites.Length;
        spriteRenderer.sprite = talkingSprites[frameIndex];
    }
    
}
