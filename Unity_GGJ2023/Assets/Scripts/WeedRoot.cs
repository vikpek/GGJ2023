namespace DefaultNamespace
{
    public class WeedRoot : InteractiveRotatable
    {
        public void RipOut(int strength)
        {
            growingState -= strength;
            UpdateWeedRootVisuals();

            if (growingState <= 0)
                RaiseOnRemove(this);
        }

        protected override void UpdateVisuals()
        {
            base.UpdateVisuals();
            UpdateWeedRootVisuals();
        }
        private void UpdateWeedRootVisuals()
        {
            float nextAlpha = Clamp(growingState / 1000f, 0, 1);
            interactiveRotatableRenderer.material.SetFloat("_Progress", nextAlpha);
        }
    }
}