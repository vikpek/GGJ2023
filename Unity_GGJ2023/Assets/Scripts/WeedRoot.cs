using System;
using System.Collections.Generic;
using UnityEngine;
namespace DefaultNamespace
{
    public class WeedRoot : InteractiveRotatable
    {
        [SerializeField] private List<SpriteRenderer> interactiveRotatableRenderer;
        [SerializeField] private ParticleSystem sparkles;

        public event Action OnCriticalState = delegate { };

        public void RipOut()
        {
            growingState -= Configs.Instance.Get.ripOutStrength;
            if (growingState <= 0)
                RaiseOnRemove(this);
        }

        protected override void UpdateVisuals()
        {
            base.UpdateVisuals();
            UpdateWeedRootState();
        }

        private void UpdateWeedRootState()
        {
            float progressInPercent = Clamp(growingState / Configs.Instance.Get.growingStateSpeedSlowDown, 0, 1);

            for (int i = interactiveRotatableRenderer.Count - 1; i >= 0; i--)
            {
                if (interactiveRotatableRenderer[i] == null)
                {
                    interactiveRotatableRenderer.RemoveAt(i);
                    continue;
                }
                interactiveRotatableRenderer[i].material.SetFloat("_Progress", progressInPercent);
            }

            if (progressInPercent >= 0.6f)
                OnCriticalState();

            if (progressInPercent >= 1f){
                SceneHelper.Instance.GoToDefeat();                
                AudioManager.Instance.PlayAudio(ClipPurpose.GameOverSound);
            }
        }
    }
}