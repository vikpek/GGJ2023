using System;
using TMPro;
using UnityEngine;
public class InteractiveRotatable : Rotatable
{
    [SerializeField] protected TMP_Text buttonHint;

    public event Action OnGrow = delegate { };
    public event Action<InteractiveRotatable> OnRemove = delegate { };
    public event Action<InteractiveRotatable> OnInteract = delegate { };

    protected int growingState = 0;
    private void OnEnable()
    {
        OnGrow += Grow;
    }
    private void OnDisable()
    {
        OnGrow -= Grow;
    }

    protected void RaiseOnRemove(InteractiveRotatable interactiveRotatable)
    {
        OnRemove(interactiveRotatable);
    }

    protected void RaiseOnInteract(InteractiveRotatable interactiveRotatable)
    {
        OnInteract(interactiveRotatable);
    }

    public void Grow()
    {
        growingState++;
        UpdateVisuals();
    }

    protected virtual void UpdateVisuals() { }

    protected static float Clamp(float value, float min, float max)
    {
        return Math.Min(Math.Max(value, min), max);
    }

    public void ShowActionButtonHint()
    {
        // buttonHint.gameObject.SetActive(true);
    }
    public void HideActionButtonHint()
    {
        // buttonHint.gameObject.SetActive(false);
    }
}