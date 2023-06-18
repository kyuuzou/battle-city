using UnityEngine;

public class Projectile : MonoBehaviour {

    [SerializeField]
    private float speed = 10.0f;

    private void OnTriggerEnter (Collider other) {
        GameObject.Destroy(this.gameObject);
    }

    private void Update() {
        this.transform.position = Vector3.MoveTowards(
            this.transform.position,
            this.transform.position + this.transform.forward, 
            this.speed * Time.deltaTime
        );
    }
}
