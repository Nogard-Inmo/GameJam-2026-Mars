using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteAnimator
{
    SpriteRenderer SpriteRenderer;
    List<Sprite> frames;
    float frameRate;

    int currentFrame;
    float timer;

    public SpriteAnimator(List<Sprite> frames, SpriteRenderer spriteRenderer, float frameRate = 0.16f)
    {
        this.frames = frames;
        this.SpriteRenderer = spriteRenderer;
        this.frameRate = frameRate;
    }

    public void Start()
    {
        currentFrame = 0;
        timer = 0;
        SpriteRenderer.sprite = frames[0];
    }

    public void HandleUpdate()
    {
        timer += Time.deltaTime;
        if (timer > frameRate)
        {
            currentFrame = (currentFrame + 1) % frames.Count;
            SpriteRenderer.sprite = frames[currentFrame];
            timer -= frameRate;

        }
    }

    public List<Sprite> Frames
    {
        get { return frames; }
    }
}
