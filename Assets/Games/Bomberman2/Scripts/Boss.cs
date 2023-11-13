using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

namespace io.lockedroom.Games.Bomberman2 {
    /// <summary>
    /// 1 enum gồm 4 state
    /// </summary>
    public enum BossState {
        Idle,
        Movement,
        PlaySkill,
        Die
    }
    public class Boss : MonoBehaviour {
        /// <summary> 
        /// Tốc độ di chuyển của boss
        /// </summary>
        public float speed = 5f;
        public GameObject BossBombPrefab;
        public int MaxHealth = 3;
        public int CurrentHealth;
        /// <summary> 
        /// Hướng di chuyển ban đầu là xuống
        /// </summary>
        private Vector2 direction = Vector2.down;
        private Animator animator;
        /// <summary>
        /// Biến isFlashing khi boss nhận dame
        /// </summary>
        private bool isFlashing = false;
        /// <summary>
        /// Biến canMove khi boss có thể di chuyển
        /// </summary>
        private bool canMove = false;
        private Rigidbody2D rb;
        /// <summary>
        /// Trạng thái hiện tại
        /// </summary>
        private BossState currentState;
        private Coroutine m_LastStateCoroutine;
        private void Awake() {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }
        private void Start() {
            // Bắt đầu coroutine để thay đổi hướng di chuyển của boss sau một khoảng thời gian
            SetState(BossState.Idle);
            CurrentHealth = MaxHealth;
        }
        private void Update() {
            UpdateState();
        }
        private void FixedUpdate() {
            // Di chuyển boss theo hướng và tốc độ đã cho
            if (canMove) {
                rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
            }
            UpdateAnimation();
        }
        private void SetState(BossState state) {
            currentState = state;
            if (m_LastStateCoroutine != null) {
                StopCoroutine(m_LastStateCoroutine);
            }
            switch (currentState) {
                case BossState.Idle:
                    m_LastStateCoroutine = StartCoroutine(IdleState());
                    break;
                case BossState.Movement:
                    m_LastStateCoroutine = StartCoroutine(MovementState());
                    break;
                case BossState.PlaySkill:
                    m_LastStateCoroutine = StartCoroutine(PlaySkillState());
                    break;
                case BossState.Die:
                    m_LastStateCoroutine = StartCoroutine(DieState());
                    break;
            }
        }
        private void UpdateState() {
            switch (currentState) {
                case BossState.Idle:
                    // Kiểm tra điều kiện chuyển trạng thái
                    break;
                case BossState.Movement:
                    // Kiểm tra điều kiện chuyển trạng thái
                    break;
                case BossState.PlaySkill:
                    // Kiểm tra điều kiện chuyển trạng thái
                    break;
                case BossState.Die:
                    // Trạng thái Die không thể chuyển sang trạng thái khác
                    break;
            }
            Debug.Log($"CurrentState: {currentState}");
        }
        /// <summary>
        /// Thời gian để boss dùng skill
        /// </summary>
        private IEnumerator PlaySkillState() {
            int minX = -6;
            int minY = -5;
            int maxX = 6;
            int maxY = 5;
            // Thực hiện hành động trong trạng thái PlaySkill
            direction = Vector2.zero;
            canMove = false;
            // TODO: Doing skill logic (assuming it took 3 secs)
            for (int i = 0; i < 10; i++) {
                int randomX = Random.Range(minX, maxX);
                int randomY = Random.Range(minY, maxY);
                // Tạo quả bom tại vị trí ngẫu nhiên
                if (IsPositionValid(new Vector3(randomX, randomY, 0))) {
                    Instantiate(BossBombPrefab, new Vector3(randomX, randomY, 0f), Quaternion.identity);
                }
                else {
                    i--;
                }
            }
            yield return new WaitForSeconds(3.5f);
            SetState(BossState.Movement);
        }
        private bool IsPositionValid(Vector3 position) {
            Vector2 checkPosition = new Vector2(position.x, position.y);
            // Kiểm tra xem có bất kỳ collider nào nằm ở vị trí checkPosition không
            Collider2D collider = Physics2D.OverlapPoint(checkPosition);
            // Nếu không có collider nào tại vị trí này, vị trí hợp lệ
            return collider == null;
        }
        /// <summary>
        /// Thời gian đợi 3s trước khi boss chuyển sang trạng thái di duyển
        /// </summary>
        private IEnumerator IdleState() {
            direction = Vector2.zero;
            // Thực hiện hành động trong trạng thái Idle
            yield return new WaitForSeconds(3f);
            SetState(BossState.Movement);
        }
        /// <summary>
        /// Thời gian boss ở trạng thái di chuyển
        /// </summary>
        private IEnumerator MovementState() {
            StartCoroutine(MovementStateEnd());
            // Thực hiện hành động trong trạng thái Movement
            canMove = true;
            while (canMove) {
                direction = GetRandomDirection();
                float delay = Random.Range(0.5f, 2f);
                yield return new WaitForSeconds(delay);
            }
        }
        private IEnumerator MovementStateEnd() {
            yield return new WaitForSeconds(7);
            SetState(BossState.PlaySkill);
        }
        private IEnumerator DieState() {
            animator.SetBool("Moving", false);
            animator.SetBool("IsDead", true);
            // Thực hiện hành động trong trạng thái Die
            yield break;
        }
        /// <summary>
        /// Chọn hướng ngẫu nhiên để di chuyển
        /// </summary>
        private Vector2 GetRandomDirection() {
            // Lấy một số ngẫu nhiên từ 0 đến 3 để chọn hướng di chuyển
            int randomIndex = Random.Range(0, 4);
            switch (randomIndex) {
                case 0:
                    return Vector2.up;
                case 1:
                    return Vector2.down;
                case 2:
                    return Vector2.left;
                case 3:
                    return Vector2.right;
                default:
                    return Vector2.down;
            }
        }
        /// <summary>
        /// Chạy animation tương ứng
        /// </summary>
        private void UpdateAnimation() {
            // Xác định hướng di chuyển dựa trên vector direction
            float moveX = direction.x;
            float moveY = direction.y;
            // Cập nhật các thông số trong animator
            animator.SetFloat("MoveX", moveX);
            animator.SetFloat("MoveY", moveY);
            animator.SetBool("Moving", true);
        }
        /// <summary>
        /// Nếu ng chơi đi vào bom nổ thì die
        /// </summary>
        private void OnTriggerEnter2D(Collider2D other) {
            // Ktra điều kiện với layer Explosion
            if (other.gameObject.layer == LayerMask.NameToLayer("Explosion")) {
                CurrentHealth--;
                StartCoroutine(FlashingEffect());
                if (CurrentHealth <= 0) {
                    DieState();
                }
            }
        }
        private IEnumerator FlashingEffect() {
            if (isFlashing)
                yield break;
            isFlashing = true;
            SpriteRenderer spriteRenderer = rb.GetComponent<SpriteRenderer>();
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
    }
}
