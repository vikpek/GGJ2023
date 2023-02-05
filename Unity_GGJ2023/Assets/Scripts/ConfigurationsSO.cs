using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Global Config")]
public class ConfigurationsSO : ScriptableObject
{
    public float musicVolume;
    public float soundEffectsVolume;

    public int minNightAngle = 150;
    public int maxNightAngle = 320;

    public float rotationSpeed = 0.01f;

    public int playerAmount = 2;
    public int maxSpeed = 5;
    public float speedFadeTime = 0.4f;

    public float spawnInterval = 3.0f;
    public float interactionDelay = 0.5f;

    public int ripOutStrength = 25;
    public int growingPercentageForVFXRootWarning = 60;
    public float growingStateSpeedSlowDown = 1000f;
    public Sprite waterSprite;
    public Sprite flowerSprite;
    public Sprite leafSprite;
    public float growingPhase1Duration = 2f;
    public float growingPhase2Duration = 2f;
    public float flowerDestructionDelay = 2f;
    public float growingPhaseLastDuration = 5f;
    public float aoeRadius = 3f;
    public float spawnSeedDuration = 3f;
    public float harvestDuration = 3f;
    public float wateringDuration = 3f;
    public float durationUntilWin = 30f;
    public float difficultyIncreaseDuration = 10f;
    public float increaseDifficultyStep = 0.1f;
    public bool godModeON = false;
}