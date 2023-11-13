using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace io.lockedroom.Games.Bomberman2 {
    public class EnemyBehavior : MonoBehaviour {
        /// <summary>
        /// Khởi tạo biến public rigidbody
        /// {get; private set;} nghĩa là các thành phần khác ngoài lớp không thể gán trực tiếp giá trị cho rigidbody
        /// mà chỉ có thể đọc giá trị hiện tại của nó.
        /// </summary>
        public new Rigidbody2D rigidbody { get; private set; }
        /// <summary>
        /// Hướng mặc định cho di chuyển là xuống
        /// </summary>
        private Vector2 direction = Vector2.down;
        /// <summary>
        /// Khởi tạo tốc độ di chuyển mặc định là 2f
        /// </summary>
        public float speed = 2f;
        public SpriteRenderer spriteRenderer;
        public BoxCollider2D boxCollider;
        /// <summary>
        /// Nhận component
        /// </summary>
        private void Awake() {
            rigidbody = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            boxCollider = GetComponent<BoxCollider2D>();
        }
        /// <summary>
        /// Tính trước vị trí tương lai và check collider để chuyển hướng
        /// </summary>
        private void Update() {
            // Ví trí tương lai
            Vector2 futurePos = (Vector2)transform.position + direction * speed * Time.deltaTime;
            Collider2D[] colliders = Physics2D.OverlapBoxAll(futurePos + boxCollider.offset, boxCollider.size * 0.9f, 0);
            bool shouldChangeDirection = false;
            foreach (Collider2D collider in colliders) {
                // Ktra nếu collider của gameObject khác thì nên đổi hướng
                if (collider.gameObject != gameObject) {
                    shouldChangeDirection = true;
                    break;
                }
            }
            if (shouldChangeDirection) {
                ChangeDirectionRandomly();
            }
            // Nếu không thì vị trí hiện tại = vị trí tương lai
            else {
                transform.position = futurePos;
            }
        }
        /// <summary>
        /// Hàm dùng để đổi hướng ngẫu nhiên
        /// </summary>
        private void ChangeDirectionRandomly() {
            // Chọn ngẫu nhiên 4 hướng vector gắn với 4 số
            // Random số và chọn hướng đi tiếp theo
            Vector2[] possibleDirections = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
            int randomIndex = Random.Range(0, possibleDirections.Length);
            direction = possibleDirections[randomIndex];
        }
        /// <summary>
        /// Nếu quái chơi đi vào phạm vi bom nổ thì die
        /// </summary>
        private void OnTriggerEnter2D(Collider2D other) {
            // Ktra điều kiện với layer Explosion
            if (other.gameObject.layer == LayerMask.NameToLayer("Explosion")) {
                EnemyDeathSequence();
                // Cập nhật UI
                ScoreManager.instance.AddPoint();
            }
        }
        /// <summary>
        /// Hàm xử lý khi quái va chạm vào bom
        /// </summary>
        private void EnemyDeathSequence() {
            Destroy(gameObject);
            GameManager.instance.numberOfEnemy--;
            GameManager.instance.CheckWin();
            AudioManager.instance.PlayEnemySound();
        }
    }
}
