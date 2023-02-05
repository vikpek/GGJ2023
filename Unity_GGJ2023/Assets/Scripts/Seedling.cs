using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            ActivatePhase(1);
            StartCoroutine(InitiateGrowing());
        }

        private IEnumerator InitiateGrowing()
        {
            yield return new WaitForSeconds(Configs.Instance.Get.growingPhase1Duration);
            ActivatePhase(2);
            yield return new WaitForSeconds(Configs.Instance.Get.growingPhase2Duration);
            ActivatePhase(3);
            IsReadyToHarvest = true;
        }
        private void ActivatePhase(int phase)
        {
            foreach (GameObject seedlingPhaseObject in seedlingPhaseObjects)
            {
                seedlingPhaseObject.SetActive(false);
            }

            seedlingPhaseObjects[phase].SetActive(true);
        }
    }
}