using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FirstPersonMover : Mover
{
    #region Variables
    // Sensitivity
    private float sensX;
    private float sensY;

    // Orientation & Rotation
    public GameObject playerCam;
    public float xRotation;
    public float yRotation;

    // Base stuff
    public Pawn pawn;
    private Rigidbody rb;

    // Ground Check
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;
    public float groundDrag;
    public float airDrag;

    // Jumping
    public float jumpForce;
    public float airMultiplier;
    #endregion Variables

    // Noice
    private NoiseMaker noiseMaker;
    public void Awake()
    {
        pawn = GetComponent<Pawn>();
        rb = GetComponent<Rigidbody>();
        noiseMaker = GetComponent<NoiseMaker>();

        if (playerCam == null)
        {
            PlayerCam playerCamera = FindObjectOfType<PlayerCam>();
            playerCam = playerCamera.gameObject;
        }
    }

    // Start is called before the first frame update
    public override void Start()
    {
        if (pawn != null)
        {
            // Lock the cursor to the middle
            Cursor.lockState = CursorLockMode.Locked;
            // Make in invisible
            Cursor.visible = false;
        }

        if (rb != null)
        {
            // Freeze the PlayerObj rotation
            rb.freezeRotation = true;
        }
    }

    public void OnDestroy()
    {
        // Lock the cursor to the middle
        Cursor.lockState = CursorLockMode.None;
        // Make in invisible
        Cursor.visible = true;
    }

    public void Update()
    {
        // Ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        // Drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = airDrag;
    }
    
    public override void Jump()
    {
        if (grounded)
        {
            // Reset y velocity
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void SpeedControl(float speed)
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Limit the velocity if needed
        if (flatVel.magnitude > speed)
        {
            Vector3 limitedVel = flatVel.normalized * speed;
        }
    }

    public override void Move(Vector3 direction, float speed)
    {
        // Add force in the direction I want to go
        // On the ground
        if (grounded)
        {
            rb.AddForce(direction.normalized * speed * 10f, ForceMode.Force);
        }
        // In the air
        else if (!grounded)
        {
            rb.AddForce(direction.normalized * speed * 10f * airMultiplier, ForceMode.Force);
        }

        if (noiseMaker != null)
        {
            noiseMaker.volumeDistance = 10;
        }
    }
    public override void Rotate(float turnSpeed)
    {
        sensX = turnSpeed;
        sensY = turnSpeed;

        // Camera movement
        if (pawn != null)
        {
            // Get mouse input
            float mouseX = Input.GetAxisRaw("Mouse X") * sensX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * sensY;

            // Rotates left and right
            yRotation += mouseX;

            // Rotates up and down
            xRotation -= mouseY;
            // Locks it to 90 degrees up and down
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            // Rotate camera and orientation
            playerCam.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
            verticalOrientation.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        }
    }
}
