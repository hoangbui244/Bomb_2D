using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace io.lockedroom.Games.Bomberman2 {
    public class Destructible : MonoBehaviour {
        /// <summary>
        /// Thời gian để vật biến mất hoàn toàn
        /// </summary>
        public float destructionTime = 1f;
        /// <summary>
        /// Tỉ lệ item xuất hiện khi phá vỡ gạch
        /// </summary>
        [Range(0f, 1f)]
        public float itemSpawnChance = 0.2f;
        /// <summary>
        /// Vật phẩm nào có thể xuất hiện
        /// </summary>
        public GameObject[] spawnableItems;
        /// <summary>
        /// Hàm Start
        /// </summary>
        private void Start() {
            Destroy(gameObject, destructionTime);
        }
        /// <summary>
        /// Hàm khi phá vỡ vật cản thì rơi item
        /// </summary>
        private void OnDestroy() {
            if (spawnableItems.Length > 0 && Random.value < itemSpawnChance) {
                int randomIndex = Random.Range(0, spawnableItems.Length);
                Instantiate(spawnableItems[randomIndex], transform.position, Quaternion.identity);
            }
        }
    }
}
