using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace io.lockedroom.Games.Bomberman2 {
    public class AudioManager : MonoBehaviour {
        public static AudioManager instance;
        [SerializeField] private AudioSource mainSound, itemSound, enemySound;
        /// <summary>
        /// Hàm Awake
        /// </summary>
        private void Awake() {
            if (instance == null) {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else {
                Destroy(gameObject);
            }
        }
        /// <summary>
        /// Hàm chạy audio khi pickup item
        /// </summary>
        public void PlayItemSound() {
            itemSound.PlayOneShot(itemSound.clip);
        }
        /// <summary>
        /// Hàm chạy audio khi quái chết
        /// </summary>
        public void PlayEnemySound() {
            enemySound.PlayOneShot(enemySound.clip);
        }
    }
}