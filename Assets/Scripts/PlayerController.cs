using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float speed = 5.0f;
    [SerializeField] float turnSmoothTime = 0.1f;
    [SerializeField] float jumpSpeed = 10.0f;
    [SerializeField] float gravity = 10f;
    [SerializeField] Transform playerCamera;
    [SerializeField] GameObject playerObject;

    CharacterController characterController;
    Animator playerAnimator;

    float turnSmothVelocity;
    float directionY = 0f;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = playerObject.GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        characterController.detectCollisions = true;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        // Get movement axis input 
        float verticalInput = Input.GetAxisRaw("Vertical");
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        // Init move vector
        Vector3 moveDirection = new Vector3(0f, 0f, 0f);

        // Calculate movement direction based on input axis and camera look
        if (verticalInput != 0 || horizontalInput != 0)
        {
            Vector3 direction = new Vector3(horizontalInput, 0, verticalInput).normalized;
            float lookAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, lookAngle, ref turnSmothVelocity, turnSmoothTime);
            moveDirection = (Quaternion.Euler(0f, lookAngle, 0f) * Vector3.forward).normalized * speed;
            
            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

            // Trigger running animation
            playerAnimator.SetFloat("MoveSpeed", 2f);
        }
        else {
            // Trigger idle animation
            playerAnimator.SetFloat("MoveSpeed", 0f);
        }

        // Get vertical direction in case of jump
        moveDirection.y = GetJumpDirection();

        // Move character
        characterController.Move(moveDirection * Time.deltaTime);
    }

    float GetJumpDirection()
    {
        if (characterController.isGrounded)
        {

            playerAnimator.SetBool("Grounded", true);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Set jump speed
                directionY = jumpSpeed;
                // Trigger jump animation
                playerAnimator.SetBool("Grounded", false);
            }
        }
        else
        {
            // Decrease vertical movement 
            directionY -= gravity * Time.deltaTime;
        }

        return directionY;
    }
}
