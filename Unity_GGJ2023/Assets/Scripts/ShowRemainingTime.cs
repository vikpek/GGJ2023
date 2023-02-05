using UnityEngine;
namespace DefaultNamespace
{
    public class ShowRemainingTime : MonoBehaviour
    {
        private float timeRemaining;
        [SerializeField] private SpriteMask spriteMask;



        private void Start()
        {
            timeRemaining = Configs.Instance.Get.durationUntilWin;
        }
        void Update()
        {
            timeRemaining -= Time.deltaTime;
            float percentage = CalculationHelper.CalculatePercentage(timeRemaining, Configs.Instance.Get.durationUntilWin);

            Vector3 modifiedScale = spriteMask.transform.localScale;
            modifiedScale.y = percentage;
            spriteMask.transform.localScale = modifiedScale;

        }
    }
}