using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private float jumpHeight = 5f;

    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private Transform targetTransform;

    private float inputMovement;
    private Rigidbody rbody;
    private Animator animator;
    private bool isGrounded;
    [SerializeField] Camera cam;
    [SerializeField] private LayerMask mouseAimMask;

    private int FacingSign
    {
        get
        {
            Vector3 perp = Vector3.Cross(transform.forward, Vector3.forward);
            float dir = Vector3.Dot(perp, transform.up);
            return dir > 0f ? -1 : dir < 0f ? 1 : 0;
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody>();
    }


    private void Update()
    {
        inputMovement = Input.GetAxis("Horizontal");

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mouseAimMask))
        {
            targetTransform.position = hit.point;
        }
    }

    private void FixedUpdate()
    {

        //rbody.linearVelocity = new Vector3(inputMovement * moveSpeed, rbody.linearVelocity.y, 0);
        if (FacingSign > 0 && inputMovement < 0)
        {
            rbody.linearVelocity = new Vector3(inputMovement * moveSpeed * 0.75f, rbody.linearVelocity.y, 0);
        }
        else if (FacingSign < 0 && inputMovement > 0)
        {
            rbody.linearVelocity = new Vector3(inputMovement * moveSpeed * 0.75f, rbody.linearVelocity.y, 0);
        }
        else
        {
            rbody.linearVelocity = new Vector3(inputMovement * moveSpeed, rbody.linearVelocity.y, 0);
        }

        // Movement
        if (FacingSign > 0)
        {

            animator.SetFloat("Speed", (rbody.linearVelocity.x) / moveSpeed);
        }
        else
        {
            // Slower
            animator.SetFloat("Speed", -(rbody.linearVelocity.x) / (moveSpeed)); 
        }

        // Facing Orientation
        rbody.MoveRotation(Quaternion.Euler(new Vector3(0, 90 * Mathf.Sign(targetTransform.position.x - transform.position.x), 0)));
    }
}
