using UnityEngine;

public class TankOnRails : MonoBehaviour {

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

    private Vector3 movementInput;
    private new Rigidbody rigidbody;

    private void Awake() {
        this.rigidbody = this.GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        Vector3 localVelocity = this.transform.InverseTransformDirection(this.rigidbody.velocity);

        if (!Mathf.Approximately(this.movementInput.z, 0.0f)) {
            if (localVelocity.z < maximumSpeed && localVelocity.z > -maximumSpeed){
                this.rigidbody.AddForce(
                    this.transform.forward * this.movementInput.z * acceleration * this.rigidbody.mass
                );
            }
        }
 
        if (!Mathf.Approximately(this.movementInput.x, 0.0f)) {
            if (localVelocity.x < maximumSpeed && localVelocity.x > -maximumSpeed) {
                this.rigidbody.AddForce(
                    this.transform.right * this.movementInput.x * acceleration * this.rigidbody.mass
                );
            }
        }
    }

    private void HandleMovementInput() {
        this.movementInput.Set(Input.GetAxis(this.horizontalAxis), 0.0f, Input.GetAxis(this.verticalAxis));
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

    private void LateUpdate() {
        if (!Mathf.Approximately(this.movementInput.magnitude, 0.0f)) {
            this.visuals.LookAt(this.visuals.position + this.movementInput);
        }
    }

    private void Update() {
        this.HandleMovementInput();
        this.HandleShootingInput();
    }
}
