using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    private Animator animator;
    int isWalkingHash, isRunningHash, velocityHash;
    float velocity = 0.0f;
    [SerializeField] float acceleration = 0.15f;
    [SerializeField] float deceleration = 0.5f;
    //[SerializeField] float acceleration = 1.0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        velocityHash = Animator.StringToHash("Velocity");
    }

    // Update is called once per frame
    void Update()
    {
        bool isRunning = animator.GetBool(isRunningHash);
        bool isWalking = animator.GetBool(isWalkingHash);
        bool rightPressed = Input.GetKey(KeyCode.D);
        bool runPressed = Input.GetKey(KeyCode.LeftShift);

        if (rightPressed && velocity < 1.0f)
        {
            velocity += Time.deltaTime * acceleration;
        }

        if (!rightPressed && velocity > 0.0f)
        {
            velocity -= Time.deltaTime * deceleration;
        }

        if (!rightPressed && velocity < 0.0f)
        {
            velocity = 0.0f;
        }

        animator.SetFloat(velocityHash, velocity);


        //// If player presses D key
        //if (!isWalking && rightPressed)
        //{
        //    animator.SetBool(isWalkingHash, true);
        //}

        //// If player releases horizontal movement keys
        //if (isWalking && !rightPressed)
        //{
        //    animator.SetBool(isWalkingHash, false);
        //}

        //// If player moving and presses Left Shift 
        //if (!isRunning && (rightPressed && runPressed))
        //{
        //    animator.SetBool(isRunningHash, true);
        //}

        //// If player stops running or stops walking
        //if (isRunning && (!rightPressed || !runPressed))
        //{
        //    animator.SetBool(isRunningHash, false);
        //}

    }
}
