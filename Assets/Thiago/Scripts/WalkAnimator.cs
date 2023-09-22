using System;
using UnityEngine;

public class WalkAnimator : MonoBehaviour
{
    [SerializeField] private Sprite[] walkingBackSprites;
    [SerializeField] private Sprite[] walkingFrontSprites;
    private SpriteRenderer spriteRenderer;
    
    private bool isWalking;
    private bool isWalkingFront;
    private int currentWalkingFrame;
    public float frameChangeDelay = 0.2f;
    private int currentWalkingSpriteIndex = 0;
    private float nextFrameTime;
    private int[] walkingSequence = { 0, 1, 2, 1 };
    private bool doOnce = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isWalking)
        {
            AnimateWalking();
        }
        else
        {
            doOnce = false;
            spriteRenderer.sprite = walkingBackSprites[1];
        }
    }

    public void StartWalking(bool front)
    {
        if (!doOnce)
        {
            doOnce = true;
            isWalking = true;
            isWalkingFront = front;
            currentWalkingFrame = 0;
        }
        
    }

    public void StopAnimations()
    {
        doOnce = false;
        isWalking = false;
        isWalkingFront = false;
    }
    

    private void AnimateWalking()
    {
        if (Time.time >= nextFrameTime)
        {
            int frameIndex = walkingSequence[currentWalkingSpriteIndex];
            Sprite[] walkingSprites = isWalkingFront ? walkingFrontSprites : walkingBackSprites;
            spriteRenderer.sprite = walkingSprites[frameIndex];
            currentWalkingSpriteIndex = (currentWalkingSpriteIndex + 1) % walkingSequence.Length;
            nextFrameTime = Time.time + frameChangeDelay;
        }
    }



}
