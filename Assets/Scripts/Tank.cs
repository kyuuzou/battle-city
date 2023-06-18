using UnityEngine;

public class Tank : MonoBehaviour {

    [SerializeField]
    private float speed = 10.0f;

    [SerializeField]
    private Transform visuals;

    private new Rigidbody rigidbody;

    private void Awake() {
        this.rigidbody = this.GetComponent<Rigidbody>();
    }

    private void HandleInput() {
        float horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float vertical = Input.GetAxis("Vertical") * Time.deltaTime * speed;

        this.rigidbody.transform.Translate(horizontal, 0, vertical);

        float angleRadians = Mathf.Atan2(vertical, horizontal);
        float angleDegrees = (angleRadians * 180.0f / Mathf.PI + 360.0f) % 360.0f;

        Vector3 visualsEulerAngles = this.visuals.eulerAngles;
        visualsEulerAngles.y = 180.0f - (angleDegrees + 90.0f);
        this.visuals.eulerAngles = visualsEulerAngles;
    }

    private void Update() {
        this.HandleInput();        
    }
}
