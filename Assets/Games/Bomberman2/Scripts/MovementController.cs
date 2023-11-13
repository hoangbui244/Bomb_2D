using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

namespace io.lockedroom.Games.Bomberman2 {
    public class MovementController : MonoBehaviour {
        /// <summary>
        /// Khởi tạo biến public rigidbody
        /// {get; private set;} nghĩa là các thành phần khác ngoài lớp không thể gán trực tiếp giá trị cho rigidbody
        /// mà chỉ có thể đọc giá trị hiện tại của nó.
        /// </summary>
        public new Rigidbody2D rigidbody { get; private set; }
        public int maxHealth = 3;
        public int currentHealth;
        /// <summary>
        /// Hướng mặc định cho di chuyển là xuống
        /// </summary>
        private Vector2 direction = Vector2.down;
        /// <summary>
        /// Khởi tạo tốc độ di chuyển mặc định là 5f
        /// </summary>
        public float speed = 5f;
        /// <summary>
        /// Nhập các input điều khiển nhân vật di chuyển
        /// </summary>
        public KeyCode inputUp = KeyCode.W;
        public KeyCode inputDown = KeyCode.S;
        public KeyCode inputLeft = KeyCode.A;
        public KeyCode inputRight = KeyCode.D;
        /// <summary>
        /// SpritesRenderer các hướng
        /// </summary>
        public AnimatedSpriteRenderer spriteRendererUp;
        public AnimatedSpriteRenderer spriteRendererDown;
        public AnimatedSpriteRenderer spriteRendererLeft;
        public AnimatedSpriteRenderer spriteRendererRight;
        public AnimatedSpriteRenderer spriteRendererDeath;
        /// <summary>
        /// Dùng để ktra xem hiện tại Sprite nào đang được kích hoạt
        /// </summary>
        private AnimatedSpriteRenderer activeSpritesRenderer;
        private bool isFlashing = false;
        /// <summary>
        /// Thêm biến để lưu trữ Tilemap có tag "Destructible"
        /// </summary>
        private Tilemap destructibleTilemap;
        /// <summary>
        /// Hàm Awake sử dụng để khởi tạo các thành phần của đối tượng trước khi bất kỳ phương thức nào khác của đối tượng được gọi.
        /// </summary>
        private void Awake() {
            /// Tìm kiếm GameObject mà đoạn script này chạy, tìm theo loại được nhập trong ngoặc của GetComponent
            rigidbody = GetComponent<Rigidbody2D>();
            /// GetComponent cho Sprite được kích hoạt hiện tại (mặc định là xuống)
            activeSpritesRenderer = spriteRendererDown;
        }
        public void Start() {
            currentHealth = maxHealth;
        }
        /// <summary>
        /// Hàm Update được gọi mỗi khung hình của scene
        /// </summary>
        private void Update() {
            /// Kiểm tra mỗi khung hình có phím nào được nhấn và kích hoạt sprite của hướng đó
            if (Input.GetKey(inputUp)) {
                SetDirection(Vector2.up, spriteRendererUp);
            }
            else if (Input.GetKey(inputDown)) {
                SetDirection(Vector2.down, spriteRendererDown);
            }
            else if (Input.GetKey(inputLeft)) {
                SetDirection(Vector2.left, spriteRendererLeft);
            }
            else if (Input.GetKey(inputRight)) {
                SetDirection(Vector2.right, spriteRendererRight);
            }
            else {
                SetDirection(Vector2.zero, activeSpritesRenderer);
            }
        }
        /// <summary>
        /// Hàm FixedUpdate xử lý các hoạt động vật lý và chuyển động của các đối tượng trong trò chơi mỗi khung hình
        /// để đảm bảo tính đồng bộ và độ chính xác của các hoạt động.
        /// </summary>
        private void FixedUpdate() {
            /// <summary>
            /// Vị trí sau mỗi khung hình là vị trí của rigidbody
            /// Sự chuyển đổi = hướng di chuyển * tốc độ * tgian
            /// Nếu ở hàm Update thì dùng DeltaTime còn FixedUpdate thì dùng fixedDeltaTime
            /// </summary>
            Vector2 position = rigidbody.position;
            Vector2 translation = direction * speed * Time.fixedDeltaTime;
            /// Vị trí rigidbody = vị trí nhận được ở trên + độ chuyển dịch
            rigidbody.MovePosition(position + translation);
        }
        /// <summary>
        /// Hàm SetDirection để update direction mới
        /// </summary>
        private void SetDirection(Vector2 newDirection, AnimatedSpriteRenderer spriteRenderer) {
            direction = newDirection;
            /// Enable render các sprites theo các hướng nếu thỏa mãn điều kiện
            /// Enable 1 hướng và disable các hướng còn lại
            spriteRendererUp.enabled = spriteRenderer == spriteRendererUp;
            spriteRendererDown.enabled = spriteRenderer == spriteRendererDown;
            spriteRendererLeft.enabled = spriteRenderer == spriteRendererLeft;
            spriteRendererRight.enabled = spriteRenderer == spriteRendererRight;
            /// nếu sprite hiện tại là idle thì hướng đứng yên
            activeSpritesRenderer = spriteRenderer;
            activeSpritesRenderer.idle = direction == Vector2.zero;
        }
        /// <summary>
        /// Nếu người chơi chạm vào Enemy thì thua
        /// </summary>
        private void OnCollisionEnter2D(Collision2D collision) {
            // So sánh với tag Enemy để kích hoạt hàm DeathSequence
            if (collision.gameObject.CompareTag("Enemy")) {
                currentHealth--;
                StartCoroutine(FlashingEffect());
                if (currentHealth <= 0) {
                    DeathSequence();
                }
            }
        }
        /// <summary>
        /// Nếu ng chơi đi vào bom nổ thì die
        /// </summary>
        private void OnTriggerEnter2D(Collider2D other) {
            // Ktra điều kiện với layer Explosion
            if (other.gameObject.layer == LayerMask.NameToLayer("Explosion")) {
                currentHealth--;
                StartCoroutine(FlashingEffect());
                if (currentHealth <= 0) {
                    DeathSequence();
                }
            }
            else if (other.gameObject.CompareTag("WaterSlice")) {
                BoggedDown();
            }
            else if (other.gameObject.CompareTag("NextStage")) {
                int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene(currentSceneIndex + 1);
            }
        }
        /// <summary>
        /// Bộ đếm hiệu ứng flashing bằng cách thay đổi chỉ số alpha
        /// </summary>
        private IEnumerator FlashingEffect() {
            if (isFlashing)
                yield break;
            isFlashing = true;
            SpriteRenderer spriteRenderer = activeSpritesRenderer.GetComponent<SpriteRenderer>();
            // Thời gian của hiệu ứng flashing
            float duration = 1f;
            float timer = 0f;
            while (timer < duration) {
                // Giá trị alpha dao động từ 0 đến 1
                float alpha = Mathf.PingPong(timer, duration) / duration;
                Color color = spriteRenderer.color;
                // Đặt giá trị alpha cho màu sắc
                color.a = alpha;
                spriteRenderer.color = color;
                timer += Time.deltaTime;
                yield return null;
            }
            // Đặt lại giá trị alpha về 1 sau khi kết thúc hiệu ứng flashing
            Color finalColor = spriteRenderer.color;
            finalColor.a = 1f;
            spriteRenderer.color = finalColor;
            isFlashing = false;
        }
        private void OnTriggerStay2D(Collider2D other) {
            if (other.gameObject.CompareTag("WaterSlice")) {
                BoggedDown();
            }
        }
        private void OnTriggerExit2D(Collider2D other) {
            if (other.gameObject.CompareTag("WaterSlice")) {
                speed = 5;
            }
        }
        /// <summary>
        /// Khi người chơi thua
        /// </summary>
        private void DeathSequence() {
            // Khi người chơi thua thì disable hành động, hoạt ảnh,...
            enabled = false;
            GetComponent<BombController>().enabled = false;
            spriteRendererDown.enabled = false;
            spriteRendererUp.enabled = false;
            spriteRendererLeft.enabled = false;
            spriteRendererRight.enabled = false;
            spriteRendererDeath.enabled = true;
            Invoke(nameof(OnDeathSequenceEnded), 1.25f);
        }
        /// <summary>
        /// Khi người chơi thua
        /// </summary>
        private void OnDeathSequenceEnded() {
            // Hàm OnDeathSequenceEnded được invoke sau 1,25s và disable gameObject
            // Kiểm tra trạng thái WinState
            gameObject.SetActive(false);
            FindObjectOfType<GameManager>().CheckWinState();
        }
        private void BoggedDown() {
            speed = 2;
        }
        /// <summary>
        /// Hàm kích hoạt GhostMode để tắt tạm thời collider giữa 2 layer
        /// </summary>
        public void ActivateGhostMode() {
            // Tạm thời tắt va chạm giữa người chơi và destructible tilemap
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Wall"), true);
            // Hẹn giờ để chuyển đổi sang Deactive Ghost Mode sau 4 giây
            StartCoroutine(SwitchGhostMode(4f));
        }
        public IEnumerator DeactivateGhostMode() {
            // khai báo biến isoverlap 
            bool isOverlapped = true;
            // khi mà đè
            while (isOverlapped) {
                // lấy collider của người chơi
                if (!TryGetComponent<Collider2D>(out var collider)) {
                }
                // tạo 1 list kết quả cho hàm overlap 
                List<Collider2D> results = new List<Collider2D>();
                Physics2D.OverlapCollider(collider, new ContactFilter2D().NoFilter(), results);
                // cho = false
                isOverlapped = false;
                // vòng lặp check nếu đè lên layer Wall
                foreach (var other in results) {
                    if (other.gameObject.layer == LayerMask.NameToLayer("Wall")) {
                        // tính là overlapped
                        isOverlapped = true;
                        break;
                    }
                }
                yield return null;
            }
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Wall"), false);
        }
        /// <summary>
        /// Hàm để đếm thời gian kích hoạt của Ghostmode
        /// </summary>
        private IEnumerator SwitchGhostMode(float delay) {
            yield return new WaitForSeconds(delay);
            yield return DeactivateGhostMode();
        }
    }
}