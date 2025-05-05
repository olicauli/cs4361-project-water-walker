using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;
    public float rotationSpeed = 720f;  // degrees per second
    public float jumpForce = 5f;

    [Header("References")]
    public Transform cameraTransform;

    private Rigidbody rb;
    private Animator animator;
    private bool isGrounded;
    private Vector3 movementInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        // If you forgot to assign a camera, fall back to the main one:
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        // 1) Read player input
        movementInput.x = Input.GetAxis("Horizontal");
        movementInput.z = Input.GetAxis("Vertical");

        // // 2) Jump
        CheckJump();

        // 3) Animate
        bool isMoving = movementInput.sqrMagnitude > 0.01f;
        animator.SetBool("IsMoving", isMoving);
    }

    void FixedUpdate()
    {
        // 1) Build a direction vector relative to the camera
        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = cameraTransform.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 moveDir = camForward * movementInput.z + camRight * movementInput.x;
        if (moveDir.sqrMagnitude > 0.01f)
            moveDir.Normalize();

        // 2) Move the Rigidbody
        Vector3 newPos = rb.position + moveDir * speed * Time.fixedDeltaTime;
        newPos.y = rb.position.y; // preserve vertical velocity
        rb.MovePosition(newPos);
        // Jump();

        // 3) Rotate toward movement direction
        if (moveDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime));
        }
    }

    void CheckJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isGrounded = false;
            // animator.SetBool("isJumping", true);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isGrounded = false;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        // simple “I’m on something” check
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            // animator.SetBool("isJumping", false);
        }
    }
}