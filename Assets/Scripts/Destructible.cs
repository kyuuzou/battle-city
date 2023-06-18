using UnityEngine;

public class Destructible : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        GameObject.Destroy(this.gameObject);
    }
}
