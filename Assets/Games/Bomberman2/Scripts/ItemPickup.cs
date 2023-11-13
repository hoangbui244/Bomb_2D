using io.lockedroom.Games.Bomberman2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace io.lockedroom.Games.Bomberman2 {
    public class ItemPickup : MonoBehaviour {
        /// <summary>
        /// Danh sách loại vật phẩm
        /// </summary>
        public enum ItemType {
            BlastRadius,
            ExtraBomb,
            SpeedUp,
            ExtraLife,
            Ghost,
        }
        public ItemType type;
        /// <summary>
        /// Hàm để nhặt vật phẩm
        /// </summary>
        private void OnItemPickup(GameObject player) {
            switch (type) {
                // gọi hàm AddBomb
                case ItemType.ExtraBomb:
                    player.GetComponent<BombController>().AddBomb();
                    break;
                // tăng phạm vi nổ
                case ItemType.BlastRadius:
                    player.GetComponent<BombController>().explosionRadius++;
                    break;
                // tăng tốc độ di chuyển
                case ItemType.SpeedUp:
                    player.GetComponent<MovementController>().speed++;
                    break;
                // thêm 1 mạng
                case ItemType.ExtraLife:
                    player.GetComponent<MovementController>().currentHealth++;
                    break;
                // có thể đi xuyên tường
                case ItemType.Ghost:
                    player.GetComponent<MovementController>().ActivateGhostMode();
                    break;
            }
            AudioManager.instance.PlayItemSound();
            // sau khi nhặt vật phẩm thì xóa gameObject
            Destroy(gameObject);
        }
        /// <summary>
        /// Hàm trigger khi tác động vào 1 item
        /// </summary>
        private void OnTriggerEnter2D(Collider2D other) {
            // So sánh nếu vật có tag Player 
            if (other.CompareTag("Player")) {
                OnItemPickup(other.gameObject);
            }
        }
    }
}