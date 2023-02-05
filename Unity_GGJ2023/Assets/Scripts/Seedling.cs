using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DefaultNamespace
{
    public class Seedling : InteractiveRotatable
    {
        public bool IsReadyToHarvest = false;
        public List<GameObject> seedlingPhaseObjects = new();

        public void Start()
        {
            ActivatePhase(0);
        }

        public void Water()
        {
            StartCoroutine(InitiateGrowing());
        }

        private IEnumerator InitiateGrowing()
        {
            ActivatePhase(1);
            yield return new WaitForSeconds(Configs.Instance.Get.growingPhase1Duration);
            ActivatePhase(2);
            yield return new WaitForSeconds(Configs.Instance.Get.growingPhase2Duration);
            ActivatePhase(3);
            IsReadyToHarvest = true;
            yield return new WaitForSeconds(Configs.Instance.Get.growingPhaseLastDuration);
            ActivatePhase(5);
            RaiseOnRemove(this);
        }
        private void ActivatePhase(int phase)
        {
            foreach (GameObject seedlingPhaseObject in seedlingPhaseObjects)
            {
                seedlingPhaseObject.SetActive(false);
            }

            seedlingPhaseObjects[phase].SetActive(true);
        }
        public void DelayedDestroy()
        {
            StartCoroutine(InitiateDestruction());
        }

        private IEnumerator InitiateDestruction()
        {
            ActivatePhase(4);
            yield return new WaitForSeconds(Configs.Instance.Get.flowerDestructionDelay);
            RaiseOnRemove(this);
        }
    }
}