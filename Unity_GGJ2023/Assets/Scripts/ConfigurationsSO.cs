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

    public int ripOutStrength = 25;
    public int growingPercentageForVFXRootWarning = 60;
    public float growingStateSpeedSlowDown = 1000f;
    public Sprite waterSprite;
    public Sprite flowerSprite;
}