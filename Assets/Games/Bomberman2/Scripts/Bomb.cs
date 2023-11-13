using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
namespace io.lockedroom.Games.Bomberman2 {
    public class Bomb : MonoBehaviour {
        [Header("Bomb")]
        /// <summary>
        /// Khởi tạo GameObject bombPrefab
        /// </summary>
        public GameObject BossBombPrefab;
        /// <summary>
        /// Thời gian bom nổ
        /// </summary>
        public float bombFuseTime = 2f;
        [Header("Explosion")]
        /// <summary>
        /// Prefab
        /// </summary>
        public BombExplosion BossExplosionPrefab;
        /// <summary>
        /// Thời gian hiệu ứng nổ tồn tại
        /// </summary>
        public float ExplosionDuration = 1f;
        /// <summary>
        /// Lớp nổ bom
        /// </summary>
        public LayerMask ExplosionLayerMask;
        /// <summary>
        /// Phạm vi bom nổ
        /// </summary>
        public int ExplosionRadius = 3;
        private void Awake() {
            BossBombPrefab = gameObject;
            StartCoroutine(BombTime());
        }
        private IEnumerator BombTime() {
            Vector2 position = transform.position;
            // Đặt bomb sẽ vừa với ô vuông grid
            position.x = Mathf.Round(position.x);
            position.y = Mathf.Round(position.y);
            yield return new WaitForSeconds(bombFuseTime);
            position = BossBombPrefab.transform.position;
            position.x = Mathf.Round(position.x);
            position.y = Mathf.Round(position.y);
            BombExplosion explosion = Instantiate(BossExplosionPrefab, position, Quaternion.identity);
            explosion.SetActiveRenderer(explosion.start);
            // Xóa hiệu ứng nổ
            explosion.DestroyAfter(ExplosionDuration);
            // Gọi hàm Explode
            Explode(position, Vector2.up, ExplosionRadius);
            Explode(position, Vector2.down, ExplosionRadius);
            Explode(position, Vector2.left, ExplosionRadius);
            Explode(position, Vector2.right, ExplosionRadius);
            // Xóa bomb sau khi nổ
            Destroy(BossBombPrefab);
        }
        /// <summary>
        /// Hàm Explode để xử lý phạm vi hiệu ứng bom nổ trên nhiều tiles
        /// </summary>
        private void Explode(Vector2 position, Vector2 direction, int length) {
            // Nếu chiều dài <= 0 thì dừng
            while (length > 0) {
                position += direction;
                // Ktra nếu gặp vật cản thì hiệu ứng bom nổ dừng lại
                if (Physics2D.OverlapBox(position, Vector2.one / 2f, 0f, ExplosionLayerMask)) {
                    return;
                }
                BombExplosion explosion = Instantiate(BossExplosionPrefab, position, Quaternion.identity);
                explosion.SetActiveRenderer(length > 1 ? explosion.middle : explosion.end);
                explosion.SetDirection(direction);
                explosion.DestroyAfter(ExplosionDuration);
                length--;
            }
        }
    }
}