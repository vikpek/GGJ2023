using System;
using UnityEngine;
namespace DefaultNamespace
{
    public class StartMusic : MonoBehaviour
    {
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private MusicPurpose musicPurpose;
        
        private void Start() => audioManager.PlayMusic(musicPurpose);
    }
}