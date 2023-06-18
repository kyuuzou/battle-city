using UnityEngine;

public class Tank : MonoBehaviour {

    [SerializeField]
    private Transform actorRoot;

    [SerializeField]
    private Projectile projectilePrefab;

    [SerializeField]
    private float speed = 10.0f;

    [SerializeField]
    private Transform visuals;

    private Vector3 direction;
    private new Rigidbody rigidbody;

    private void AdjustVisuals() {
        this.visuals.LookAt(this.visuals.position + this.direction);
    }

    private void Awake() {
        this.rigidbody = this.GetComponent<Rigidbody>();
        this.direction = Vector3.forward;
    }

    private void HandleMovement() {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        movement *= Time.deltaTime * speed;
        this.rigidbody.transform.Translate(movement);

        if (!Mathf.Approximately(movement.magnitude, 0.0f)) {
            this.direction = movement.normalized;
        }
    }

    private void HandleShooting() {
        if (Input.GetButtonDown("Fire1")) {
            Projectile projectile = Object.Instantiate<Projectile>(
                this.projectilePrefab,
                this.transform.position + this.projectilePrefab.transform.position,
                this.visuals.rotation,
                this.actorRoot
            );
        }
    }

    private void LateUpdate() {
        this.AdjustVisuals();
    }

    private void Update() {
        this.HandleMovement();
        this.HandleShooting();
    }
}
