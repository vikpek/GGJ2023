using UnityEngine;
public class Configs : SingletonMonoBehaviour<Configs>
{
    [SerializeField] private ConfigurationsSO configurationsSo;
    public ConfigurationsSO Get => configurationsSo;

}