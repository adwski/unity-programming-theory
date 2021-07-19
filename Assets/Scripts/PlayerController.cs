using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float speed = 5.0f;
    [SerializeField] float turnSmoothTime = 0.1f;
    [SerializeField] float jumpForce = 10.0f;
    [SerializeField] Transform playerCamera;
    [SerializeField] Animator playerAnimator;

    CharacterController characterController;
    Rigidbody playerRb;

    float turnSmothVelocity;

    public bool IsOnGround;
    public Vector3 moveDirection;
    public Vector3 externalPosition;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
        if (characterController != null)
        {
            characterController.detectCollisions = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (IsOnGround)
        {
            JumpChecker(moveDirection / 2f);
        }
    }

    void FixedUpdate()
    {
        if (IsOnGround)
        {
            playerRb.MovePosition(playerRb.position + moveDirection * speed * Time.deltaTime);
        }
    }

    void Move()
    {
        // Get movement axis input 
        float verticalInput = Input.GetAxisRaw("Vertical");
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        // Init move vector
        moveDirection = new Vector3(0f, 0f, 0f);

        // Calculate movement direction based on input axis and camera look
        if (verticalInput != 0 || horizontalInput != 0)
        {
            Vector3 direction = new Vector3(horizontalInput, 0, verticalInput).normalized;
            float lookAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, lookAngle, ref turnSmothVelocity, turnSmoothTime);
            moveDirection = (Quaternion.Euler(0f, lookAngle, 0f) * Vector3.forward).normalized;
            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

            // Trigger running animation
            playerAnimator.SetFloat("MoveSpeed", 2f);
        }
        else {
            // Trigger idle animation
            playerAnimator.SetFloat("MoveSpeed", 0f);
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            playerRb.MovePosition(playerRb.position + other.attachedRigidbody.position - externalPosition);
            externalPosition = other.attachedRigidbody.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            externalPosition = other.attachedRigidbody.position;
            Debug.Log("Player is on the platform");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            Debug.Log("Player left the platform");
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform"))
        {
            playerAnimator.SetBool("Grounded", true);
            IsOnGround = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform"))
        {
            playerAnimator.SetBool("Grounded", false);
            IsOnGround = false;
        }
    }

    void JumpChecker(Vector3 direction)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        { 
            // Trigger jump animation
            playerAnimator.SetBool("Grounded", false);

            // Jump
            Jump(direction);

            IsOnGround = false;
        }
    }

    void Jump(Vector3 direction)
    {
        // Add jump force
        Vector3 force = (Vector3.up + direction).normalized * jumpForce;
        playerRb.AddForce(force, ForceMode.Impulse);
        Debug.Log("Jumping with force: " + force);
    }
}
