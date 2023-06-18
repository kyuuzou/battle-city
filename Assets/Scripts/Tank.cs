using UnityEngine;

public class Tank : MonoBehaviour {

    [Header("Input")]
    [SerializeField]
    private string fireAxis = "Fire1";

    [SerializeField]
    private string horizontalAxis = "Horizontal";

    [SerializeField]
    private string verticalAxis = "Vertical";

    [Header("Movement: Horizontal")]
    [SerializeField]
    private float angularAcceleration = 10.0f;

    [SerializeField]
    private float maximumTurnSpeed = 1.0f;

    [Header("Movement: Vertical")]
    [SerializeField]
    private float acceleration = 10.0f;

    [SerializeField]
    private float maximumSpeed = 20.0f;

    [Header("World")]
    [SerializeField]
    private Transform actorRoot;

    [SerializeField]
    private Projectile projectilePrefab;

    [SerializeField]
    private Transform visuals;

    private Vector2 movementInput;
    private new Rigidbody rigidbody;

    private void Awake() {
        this.rigidbody = this.GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        if (!Mathf.Approximately(this.movementInput.y, 0.0f)) {
            Vector3 localVelocity = this.transform.InverseTransformDirection(this.rigidbody.velocity);

            if (localVelocity.z < maximumSpeed && localVelocity.z > -maximumSpeed){
                this.rigidbody.AddForce(
                    this.transform.forward * this.movementInput.y * acceleration * this.rigidbody.mass
                );
            }
        }

        if (! Mathf.Approximately(this.movementInput.x, 0.0f)) {
            if (this.rigidbody.angularVelocity.magnitude < this.maximumTurnSpeed) {
                // invert turn direction when we're moving backwards
                float direction = Mathf.Sign(this.movementInput.y);

                this.rigidbody.AddTorque(
                    this.transform.up * this.movementInput.x * this.angularAcceleration * this.rigidbody.mass * direction
                );
            }
        }
    }

    private void HandleMovementInput() {
        this.movementInput.Set(Input.GetAxis(this.horizontalAxis), Input.GetAxis(this.verticalAxis));
    }

    private void HandleShootingInput() {
        if (Input.GetButtonDown(this.fireAxis)) {
            Projectile projectile = Object.Instantiate<Projectile>(
                this.projectilePrefab,
                this.transform.position + this.projectilePrefab.transform.position,
                this.visuals.rotation,
                this.actorRoot
            );
        }
    }

    private void Update() {
        this.HandleMovementInput();
        this.HandleShootingInput();
    }
}
