using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace io.lockedroom.Games.Bomberman2 {
    public class CameraController : MonoBehaviour {
        public Transform target;
        // Start is called before the first frame update
        void Start() {

        }
        void Update() {
            transform.position = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
        }
    }
}