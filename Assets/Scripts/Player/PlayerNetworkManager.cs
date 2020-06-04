using UnityEngine;

namespace Player {
    public class PlayerNetworkManager: MonoBehaviour {
        public int id;
        public string username;
        
        public void Initialize(int id, string username) {
            this.id = id;
            this.username = username;
        }
    }
}