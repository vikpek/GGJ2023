using System.Collections.Generic;
using UnityEngine;
namespace DefaultNamespace
{
    public class WeedRoot : InteractiveRotatable
    {
        [SerializeField] private List<SpriteRenderer> interactiveRotatableRenderer;
        [SerializeField] private ParticleSystem particleSystem;
        public void RipOut()
        {
            growingState -= Configs.Instance.Get.ripOutStrength;
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
            float nextAlpha = Clamp(growingState / Configs.Instance.Get.growingStateSpeedSlowDown, 0, 1);

            foreach (SpriteRenderer spriteRenderer in interactiveRotatableRenderer)
                spriteRenderer.material.SetFloat("_Progress", nextAlpha);

            // if (growingState > 60)
                 // particleSystem.emission= true;
        }
    }
}