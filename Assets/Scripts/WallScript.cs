using UnityEngine;

public class WallScript : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("TowerBullet") || col.gameObject.CompareTag("TankBullet")) {
            Destroy(col.gameObject);
        }
    }
}