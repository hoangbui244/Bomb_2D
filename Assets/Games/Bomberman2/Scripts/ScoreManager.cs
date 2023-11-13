using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace io.lockedroom.Games.Bomberman2 {
    public class ScoreManager : MonoBehaviour {
        public static ScoreManager instance;
        public Text scoreText;
        public Text timeLeft;
        /// <summary>
        /// Khởi tạo biến score = 0
        /// </summary>
        int score = 0;
        /// <summary>
        /// Khởi tạo biến time mỗi bàn chơi là 300;
        /// </summary>
        int time = 300;
        private Coroutine countdownCoroutine;
        /// <summary>
        /// Hàm Awake
        /// </summary>
        private void Awake() {
            instance = this;
        }
        /// <summary>
        /// Hàm start chạy đếm ngược
        /// </summary>
        private void Start() {
            countdownCoroutine = StartCoroutine(CountdownRoutine());
        }
        /// <summary>
        /// Hàm đếm  ngược
        /// </summary>
        private IEnumerator CountdownRoutine() {
            // Nếu time còn > 0 thì cứ sau 1s biến time = time - 1
            while (time > 0) {
                yield return new WaitForSeconds(1f);
                time--;
                UpdateTimeLeftText();
            }
        }
        /// <summary>
        /// Hàm thay đổi cập nhật tgian
        /// </summary>
        private void UpdateTimeLeftText() {
            int seconds = time;
            // Định dạng thời gian dạng mm:ss
            string formattedTime = string.Format("T: {000}", seconds);
            timeLeft.text = formattedTime;
        }
        /// <summary>
        /// Hàm để tính điểm
        /// </summary>
        public void AddPoint() {
            score += 200;
            // Định dạng số điểm với 8 chữ số
            string formattedScore = score.ToString("D8"); 
            scoreText.text = formattedScore;
        }
    }
}