using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
namespace io.lockedroom.Games.Bomberman2 {
    public class BombController : MonoBehaviour {
        [Header("Bomb")]
        /// <summary>
        /// Khởi tạo GameObject bombPrefab
        /// </summary>
        public GameObject bombPrefab;
        /// <summary>
        /// Khởi tạo nút đặt bomb
        /// </summary>
        public KeyCode inputKey = KeyCode.Space;
        /// <summary>
        /// Thời gian bom nổ
        /// </summary>
        public float bombFuseTime = 3f;
        /// <summary>
        /// Số lượng bomb player có thể đặt
        /// </summary>
        public int BombAmount = 1;
        /// <summary>
        /// Số lượng bomb còn lại
        /// </summary>
        private int m_BombsRemaining;
        [Header("Explosion")]
        /// <summary>
        /// Prefab
        /// </summary>
        public BombExplosion explosionPrefab;
        /// <summary>
        /// Lớp nổ bom
        /// </summary>
        public LayerMask explosionLayerMask;
        /// <summary>
        /// Thời gian hiệu ứng nổ tồn tại
        /// </summary>
        public float explosionDuration = 1f;
        /// <summary>
        /// Phạm vi bom nổ
        /// Khi nhặt PowerUp thì phạm vi tăng
        /// </summary>
        public int explosionRadius = 1;
        [Header("Destrutible")]
        /// <summary>
        /// Tilemap
        /// </summary>
        public Tilemap destructibleTiles;
        /// <summary>
        /// Prefab
        /// </summary>
        public Destructible destructiblePrefab;
        /// <summary>
        /// Hàm OnEnable để kích hoạt số bomb còn lại bằng số lượng bomb
        /// </summary>
        private void OnEnable() {
            m_BombsRemaining = BombAmount;
        }
        /// <summary>
        /// Hàm Update để update khi player ấn nút đặt bomb
        /// </summary>
        private void Update() {
            // Kiểm tra xem số lượng bomb còn lại > 0 và gọi hàm PlaceBomb
            if (m_BombsRemaining > 0 && Input.GetKeyDown(inputKey)) {
                StartCoroutine(PlaceBomb());
            }
        }
        /// <summary>
        /// Hàm PlaceBomb để xử lý đặt bomb
        /// </summary>
        private IEnumerator PlaceBomb() {
            Vector2 position = transform.position;
            // Đặt bomb sẽ vừa với ô vuông grid
            position.x = Mathf.Round(position.x);
            position.y = Mathf.Round(position.y);
            // Gán bomb = Khởi tạo bombPrefab
            GameObject bomb = Instantiate(bombPrefab, position, Quaternion.identity);
            m_BombsRemaining--;
            // Đợi thời gian để bomb nổ xong (3s)
            yield return new WaitForSeconds(bombFuseTime);
            // Reset lại vị trí quả bomb sau khi tác động physic để kèm hiệu ứng nổ tại vị trí mới
            position = bomb.transform.position;
            position.x = Mathf.Round(position.x);
            position.y = Mathf.Round(position.y);
            BombExplosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
            explosion.SetActiveRenderer(explosion.start);
            // Xóa hiệu ứng nổ
            explosion.DestroyAfter(explosionDuration);
            // Check collider
            int modifiedRadius = explosionRadius;
            if (!bomb.TryGetComponent<Collider2D>(out var collider)) {
            }
            List<Collider2D> results = new List<Collider2D>();
            Physics2D.OverlapCollider(collider, new ContactFilter2D().NoFilter(), results);
            foreach (var other in results) {
                //if (!other.TryGetComponent)
                if (other.gameObject.CompareTag("BombBoost")) {
                    modifiedRadius = 10;
                }
            }
            // Gọi hàm Explode
            Explode(position, Vector2.up, modifiedRadius);
            Explode(position, Vector2.down, modifiedRadius);
            Explode(position, Vector2.left, modifiedRadius);
            Explode(position, Vector2.right, modifiedRadius);
            // Xóa bomb sau khi nổ
            Destroy(bomb);
            // Trả lại bombsRemaining
            m_BombsRemaining++;
        }
        /// <summary>
        /// Hàm Explode để xử lý phạm vi hiệu ứng bom nổ trên nhiều tiles
        /// </summary>
        private void Explode(Vector2 position, Vector2 direction, int length) {
            // Nếu chiều dài <= 0 thì dừng
            while (length > 0) {
                position += direction;
                // Ktra nếu gặp vật cản thì hiệu ứng bom nổ dừng lại
                if (Physics2D.OverlapBox(position, Vector2.one / 2f, 0f, explosionLayerMask)) {
                    ClearDestructible(position);
                    return;
                }
                BombExplosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
                // Xét điều kiện
                explosion.SetActiveRenderer(length > 1 ? explosion.middle : explosion.end);
                explosion.SetDirection(direction);
                explosion.DestroyAfter(explosionDuration);
                length--;
            }
        }
        /// <summary>
        /// Hàm để dọn sạch cấu trúc sau khi phá
        /// 1 vật cản 
        /// </summary>
        private void ClearDestructible(Vector2 position) {
            Vector3Int cell = destructibleTiles.WorldToCell(position);
            TileBase title = destructibleTiles.GetTile(cell);
            // Nếu title khác null thì khởi tạo destructiblePrefab vào vị trí đó
            if (title != null) {
                Instantiate(destructiblePrefab, position, Quaternion.identity);
                destructibleTiles.SetTile(cell, null);
            }
        }
        /// <summary>
        /// Khi nhặt item ExtraBomb thì tăng BombAmount và BombRemaining
        /// </summary>
        public void AddBomb() {
            BombAmount++;
            m_BombsRemaining++;
        }
        /// <summary>
        /// Sau khi đặt bomb và ra khỏi quả bomb thì có thể tác động physic
        /// </summary>
        private void OnTriggerExit2D(Collider2D other) {
            // Ktra điều kiện
            if (other.gameObject.layer == LayerMask.NameToLayer("Bomb")) {
                other.isTrigger = false;
            }
        }
    }
}