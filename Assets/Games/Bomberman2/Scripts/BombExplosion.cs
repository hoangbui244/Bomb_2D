using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace io.lockedroom.Games.Bomberman2 {
    public class BombExplosion : MonoBehaviour {
        /// <summary>
        /// hoạt ảnh bomb render (start)
        /// </summary>
        public AnimatedSpriteRenderer start;
        /// <summary>
        /// hoạt ảnh bomb render (middle)
        /// </summary>
        public AnimatedSpriteRenderer middle;
        /// <summary>
        /// hoạt ảnh bomb render (end)
        /// </summary>
        public AnimatedSpriteRenderer end;
        /// <summary>
        /// Hàm kích hoạt render hoạt ảnh
        /// quyết định xem enable trạng thái nào của bomb nổ
        /// </summary>
        public void SetActiveRenderer(AnimatedSpriteRenderer renderer) {
            start.enabled = renderer == start;
            middle.enabled = renderer == middle;
            end.enabled = renderer == end;
        }
        /// <summary>
        /// Rotate hoạt ảnh bomb nổ ra các hướng
        /// </summary>
        public void SetDirection(Vector2 direction) {
            // xoay hướng bomb nổ
            float angle = Mathf.Atan2(direction.y, direction.x);
            transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
        }
        /// <summary>
        /// Hàm xóa gameObject (Làm gọn code)
        /// </summary>
        public void DestroyAfter(float seconds) { 
            Destroy(gameObject, seconds);
        }
    }
}
