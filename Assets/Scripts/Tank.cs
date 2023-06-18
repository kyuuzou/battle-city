using UnityEngine;

public class Tank : MonoBehaviour {

    [SerializeField]
    private float speed = 10.0f;

    private new Rigidbody rigidbody;

    private void Awake() {
        this.rigidbody = GetComponent<Rigidbody>();
    }

    private void HandleInput() {
        float horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float vertical = Input.GetAxis("Vertical") * Time.deltaTime * speed;

        this.rigidbody.AddForce(horizontal, 0, vertical);
        
        //this.transform.Translate(horizontal, 0, vertical);
    }

    private void Update() {
        this.HandleInput();        
    }
}
