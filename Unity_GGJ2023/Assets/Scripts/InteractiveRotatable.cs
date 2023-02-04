using System;
using TMPro;
using UnityEngine;
public class InteractiveRotatable : Rotatable
{
    [SerializeField] private SpriteRenderer weedRootRenderer;
    [SerializeField] private TMP_Text buttonHint;

    public event Action OnGrow = delegate { };
    public event Action<InteractiveRotatable> OnInteract = delegate { };
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
        UpdateWeedRootVisuals();
    }

    public void RipOut()
    {
        growingState--;
        UpdateWeedRootVisuals();

        if (growingState < 0)
            OnInteract(this);
    }
    private void UpdateWeedRootVisuals()
    {
        float nextAlpha = Clamp((growingState * 10) / 100f, 0, 1);
        buttonHint.text = nextAlpha.ToString();
        weedRootRenderer.color = new Color(1, 0, 0, nextAlpha);
    }

    private static float Clamp(float value, float min, float max)
    {
        return Math.Min(Math.Max(value, min), max);
    }

    public void ShowActionButtonHint()
    {
        buttonHint.gameObject.SetActive(true);
    }
    public void HideActionButtonHint()
    {
        buttonHint.gameObject.SetActive(true);
    }
}