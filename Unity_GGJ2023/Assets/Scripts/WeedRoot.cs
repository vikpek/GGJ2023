using System;
using System.Collections.Generic;
using UnityEngine;
public class WeedRoot : MonoBehaviour
{
    [SerializeField] private SpriteRenderer weedRootRenderer;
    
    public event Action OnGrow = delegate { };
    private int growingState = 0;
    
    
    private void OnEnable()
    {
        OnGrow += HandleOnGrow;
    }
    private void OnDisable()
    {
        OnGrow -= HandleOnGrow;
    }

    private void HandleOnGrow()
    {
        growingState++;
        weedRootRenderer.color = new Color(1,0,0, growingState);
    }
}