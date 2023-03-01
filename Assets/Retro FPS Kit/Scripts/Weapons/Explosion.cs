using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSRetroKit
{
    public class Explosion : MonoBehaviour
    {

        [HideInInspector]
        public AudioClip explosionSound;

        AudioSource source;

        private void Awake()
        {
            source = GetComponent<AudioSource>();
        }

        void Start()
        {
            source.PlayOneShot(explosionSound); //Play sound on explosion in explosion's place
        }
    }
}