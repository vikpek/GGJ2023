namespace DefaultNamespace
{
    public class CalculationHelper
    { public static float CalculatePercentage(float currentTime, float totalTime)
        {
            float calculatePercentage = (100f - (currentTime / totalTime * 100)) / 100f;
            return calculatePercentage;
        }
    }
}