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
            Debug.Log("RipOut, growingState = " +growingState);
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

            for (int i = interactiveRotatableRenderer.Count - 1; i >= 0; i--)
            {
                if (interactiveRotatableRenderer[i] == null)
                {
                    interactiveRotatableRenderer.RemoveAt(i);
                    continue;
                }
                interactiveRotatableRenderer[i].material.SetFloat("_Progress", nextAlpha);
            }

            // if (growingState > 60)
            // particleSystem.emission= true;
        }
    }
}