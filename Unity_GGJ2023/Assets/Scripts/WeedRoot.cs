using System;
using System.Collections.Generic;
using UnityEngine;
namespace DefaultNamespace
{
    public class WeedRoot : InteractiveRotatable
    {
        [SerializeField] private List<SpriteRenderer> interactiveRotatableRenderer;
        [SerializeField] private ParticleSystem particleSystem;
        private ParticleSystem.EmissionModule emissionModule;
        private void Start()
        {
            emissionModule  = particleSystem.emission;
        }
        public void RipOut()
        {
            growingState -= Configs.Instance.Get.ripOutStrength;
            Debug.Log("RipOut, growingState = " + growingState);
            UpdateWeedRootState();

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

            if (progressInPercent > 0.6 && !emissionModule.enabled)
            {
                emissionModule.enabled = true;
            }

            if (progressInPercent >= 1 && !Configs.Instance.Get.godModeON)
                SceneHelper.Instance.GoToDefeat();
        }
    }
}