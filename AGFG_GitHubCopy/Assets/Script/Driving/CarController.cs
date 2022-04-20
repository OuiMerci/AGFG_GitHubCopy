using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AGFGDriving
{
    public class CarController : MonoBehaviour
    {
        public bool limitFPS;

        [SerializeField] private Rigidbody sphereRB;
        [SerializeField] private Rigidbody carRB;
        [SerializeField] private LayerMask GroundLayer;
        [SerializeField] private bool isOnGravityZone;
        [SerializeField] private float fwdSpeed;
        [SerializeField] private float reverseSpeed;
        [SerializeField] private float turnSpeed;
        [SerializeField] private float reverseTurnSpeed;
        [SerializeField] private float noMoveInputTurnSpeed;
        [SerializeField] private float startMovingThreshold;
        [SerializeField] private float velocityToForwardLerpSpeed;
        [SerializeField] private float airborneExtraGravity;
        [SerializeField] private float airDrag;
        [SerializeField] private float groundAlignSpeed;

        public bool isMov;

        private float moveInput;
        private float turnInput;
        private bool isGrounded;
        private float groundDrag;

        public float MovementMagnitude => new Vector2(sphereRB.velocity.x, sphereRB.velocity.z).magnitude;
        public bool IsMoving => MovementMagnitude > startMovingThreshold;

        void Start()
        {
            if (limitFPS)
                Application.targetFrameRate = 60;

            sphereRB.transform.parent = null;
            carRB.transform.parent = null;
            groundDrag = sphereRB.drag;
        }

        void Update()
        {
            var vAxis = moveInput = Input.GetAxisRaw("Vertical");
            turnInput = Input.GetAxisRaw("Horizontal");

            // Set speeds
            var moveSpeed = moveInput > 0 ? fwdSpeed : reverseSpeed;
            var tmpTurnSpeed = moveInput == 0 ? noMoveInputTurnSpeed : moveInput > 0 ? turnSpeed : reverseTurnSpeed;

            //apply speed
            moveInput *= moveSpeed;

            // Car postion follows the sphere's
            transform.position = sphereRB.transform.position;

            // Check velocity
            var rbMagnitude = new Vector2(sphereRB.velocity.x, sphereRB.velocity.z).normalized.magnitude;
            var dotProduct = Vector3.Dot(transform.forward.normalized, sphereRB.velocity.normalized);
            rbMagnitude *= dotProduct > 0 ? 1 : -1;

            // Adjust Rotation
            if (IsMoving)
            {
                var rotationVector = new Vector3(0, turnInput * tmpTurnSpeed * rbMagnitude * Time.deltaTime, 0);
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + rotationVector);
            }

            // Ground check Raycast
            isGrounded = Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, maxDistance: 1, GroundLayer);

            // Rotate car according to ground
            var rotationTarget = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, rotationTarget, groundAlignSpeed * Time.deltaTime);

            // Check Ground Type
            isOnGravityZone = isGrounded && hit.collider.gameObject.tag == "GravityGround";

            // Update drag
            sphereRB.drag = isGrounded ? groundDrag : airDrag;
        }

        private void FixedUpdate()
        {
            if (isGrounded)
            {
                if (moveInput != 0)
                {
                    // apply input force
                    sphereRB.AddForce(transform.forward * moveInput, ForceMode.Acceleration);
                }
                else if (IsMoving)
                {
                    ApplyNoAccelerationRotation();
                }
            }
            else
            {
                // add extra gravity
                ApplyAirborneGravity();
            }

            carRB.MoveRotation(transform.rotation);
        }

        [ContextMenu("reset Car")]
        public void ResetCar()
        {
            transform.eulerAngles = Vector3.zero;
            sphereRB.velocity = Vector3.zero;
        }

        private void ApplyAirborneGravity()
        {
            if (isOnGravityZone)
                sphereRB.AddForce(transform.up * -airborneExtraGravity);
            else
                sphereRB.AddForce(Vector3.up * -airborneExtraGravity);
        }

        private void ApplyNoAccelerationRotation()
        {
            //get tmp vector between forward and velovity
            var tmpVector3 = Vector3.Lerp(sphereRB.velocity.normalized, transform.forward.normalized, velocityToForwardLerpSpeed);

            // no input, but redirect the remaining velocity to transform.forward
            var mag = sphereRB.velocity.magnitude;
            sphereRB.velocity = tmpVector3 * mag;
        }
    }
}