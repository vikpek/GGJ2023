using System;
using UnityEngine;
public class WeedRoot : Rotatable
{
    [SerializeField] private SpriteRenderer weedRootRenderer;
    public event Action OnGrow = delegate { };
    private int growingState = 0;
    private void OnEnable()
    {
        OnGrow += Grow;
    }
    private void OnDisable()
    {
        OnGrow -= Grow;
    }

    public void Grow()
    {
        growingState++;
        int nextAlpha = growingState * 10;
        if (nextAlpha > 255)
            nextAlpha = 255;
        weedRootRenderer.color = new Color(1, 0, 0, nextAlpha);
    }
}