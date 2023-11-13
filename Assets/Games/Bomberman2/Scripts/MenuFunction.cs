using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace io.lockedroom.Games.Bomberman2 {
    public class MenuFunction : MonoBehaviour {
        public GameObject optionButton;
        [SerializeField] Slider musicSlider;
        /// <summary>
        /// Hàm kích hoạt button normal game
        /// </summary>
        public void NormalGame() {
            SceneManager.LoadScene(1);
        }
        /// <summary>
        /// Hàm kích hoạt button battle game
        /// </summary>
        public void BattleGame() {
            SceneManager.LoadScene(6);
        }
        /// <summary>
        /// Hàm mở option menu
        /// </summary>
        public void OpenClose_OptionsMenu() {
            optionButton.SetActive(!optionButton.activeSelf);
        }
        /// <summary>
        /// Hàm để thay đổi âm lượng
        /// </summary>
        public void ChangeVolume() {
            AudioListener.volume = musicSlider.value;
        }
    }
}