using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
namespace io.lockedroom.Games.Bomberman2 {
    public class GameManager : MonoBehaviour {
        public static GameManager instance;
        /// <summary>
        /// GameObject người chơi
        /// </summary>
        public GameObject[] players;
        public GameObject stair;
        /// <summary>
        /// Số lượng enemy
        /// </summary>
        public int numberOfEnemy;
        /// <summary>
        /// Hàm Start
        /// </summary>
        private void Awake() {
            instance = this;
        }
        private void Start() {
            stair.SetActive(false);
            numberOfEnemy = GameObject.FindGameObjectsWithTag("Enemy").Length;
        }
        /// <summary>
        /// Hàm ktra điều kiện thắng normal game
        /// </summary>
        public void CheckWin() {
            if (numberOfEnemy <= 0) {
                stair.SetActive(true);
            }
        }
        /// <summary>
        /// Hàm ktra điều kiện thắng battle game
        /// </summary>
        public void CheckWinState() {
            // Khởi tạo biến đếm aliveCount
            int aliveCount = 0;
            // Với mỗi người chơi thì biến đếm tăng
            foreach (GameObject player in players) {
                if (player.activeSelf) {
                    aliveCount++;
                }
            }
            // Nếu biến đếm còn lại 1 người thì gọi hàm NewRound sau 2s
            if (aliveCount <= 1) {
                Invoke(nameof(NewRound), 2f);
            }
        }
        // Reset lại bàn chơi mới
        private void NewRound() {
            // Load lại scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
