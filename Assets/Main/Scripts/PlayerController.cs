using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum JumpingDirection
{
    Up = 1,
    Forward,
    Backward
}

public enum UserAction
{
    None = 1,
    TouchRight,
    TouchLeft,
    Tap,
    SwipeRight,
    SwipeLeft
}

public class PlayerController : MonoBehaviour
{
    // Editor available
    [SerializeField] GameObject horse = null;
    [SerializeField] float jumpForce = 5.0f;
    [SerializeField] float jumpRange = 2.0f;
    [SerializeField] float runSpeed = 5.0f;

    // A little workaround for animation, need to remove later
    [SerializeField] Vector3 animationHoldingRopeOffset;


    // Public fields
    public bool UseUserInput { get; set; }
    public bool KeepLowBounce { get; set; }

    // In scene
    GameObject stage;
    GameObject currentRope;
    GameManager gameManager;
    Horse horseCs;
    Rope ropeCs;
    SfxController sfxController;
    Camera mainCamera;

    // Private properties
    bool isJumping = false;
    bool isJumpToStage = false;
    bool isHoldingRope = false;
    float colliderHeight;
    JumpingDirection jumpingDirection = JumpingDirection.Up;
    Vector3 startPos;

    // Private components
    Rigidbody rbPlayer;
    Animator playerAnim;
    CapsuleCollider playerCollider;

    // Start is called before the first frame update
    void Start()
    {
        UseUserInput = true;
        KeepLowBounce = false;

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        stage = GameObject.Find("Stage");
        if (horse != null)
        {
            horseCs = horse.GetComponent<Horse>();
        }
        sfxController = GameObject.Find("SFX").GetComponent<SfxController>();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

        rbPlayer = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider>();

        colliderHeight = playerCollider.height;
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInputDebug();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground") ||
            other.gameObject.CompareTag("Ball") ||
            other.gameObject.CompareTag("Sphere"))
        {
            isJumping = false;

            UpdateCollider();

            playerAnim.SetBool("Jumping_b", false);
        }
        else if (other.gameObject.CompareTag("Stage"))
        {
            isJumping = false;
            isHoldingRope = false;

            UpdateCollider();

            playerAnim.SetBool("Jumping_b", false);
            playerAnim.SetBool("Running_b", false);
            playerAnim.SetBool("Waving_b", true);

            // Turn the player's face to the audiences in background
            transform.Rotate(Vector3.up, -80.0f);
            transform.Translate(Vector3.right * (stage.transform.position - transform.position).x);

            gameManager.LevelComplete();
        }
        else if (other.gameObject.CompareTag("Trampoline"))
        {
            currentRope = null;

            if (horse != null)
                UpdateSpeedForBouncing();
            else
                UpdateDirectionForBouncing();

            if (isJumping == false)
            {
                isJumping = true;
                jumpingDirection = JumpingDirection.Up;

                playerAnim.SetBool("Jumping_b", true);
            }
        }
        else
        {
            rbPlayer.isKinematic = true;
            playerAnim.speed = 0;

            sfxController.PlayCrashedSfx();

            gameManager.LevelFailed();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Rope"))
        {
            if (currentRope != null && currentRope == other.gameObject)
            {
                // Do nothing
            }
            else
            {
                isHoldingRope = true;
                isJumping = false;

                rbPlayer.isKinematic = true;

                currentRope = other.gameObject;
                ropeCs = currentRope.GetComponentInParent<Rope>();
                transform.position = ropeCs.GetLooseEndPosition() - animationHoldingRopeOffset;
                transform.rotation = Quaternion.identity;

                // Should replace later by holding rope animation
                playerAnim.SetBool("Jumping_b", true);
            }
        }
    }


    Vector3 mouseStartPos;
    Vector3 mouseHoldPos;
    Vector3 mousePrevHoldPos;
    float mouseStartDownTime = -1.0f;
    float mouseStartMoveTime = -1.0f;
    float mouseTapTime = 0.1f;
    float mouseSwipeTime = 0.2f;
    float swipeLength = 50.0f;

    UserAction GetUserActionByMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseStartPos = Input.mousePosition;
            mousePrevHoldPos = mouseStartPos;
            mouseStartDownTime = Time.time;
            mouseStartMoveTime = Time.time;
        }
        else if (Input.GetMouseButton(0))
        {
            mouseHoldPos = Input.mousePosition;
            float mouseUpTime = Time.time;

            if (mouseHoldPos.x == mousePrevHoldPos.x && mouseHoldPos.y == mousePrevHoldPos.y)
            {
                mouseStartMoveTime = Time.time;
            }

            if (mouseUpTime - mouseStartDownTime > mouseSwipeTime)
            {
                if (mouseStartPos.x / mainCamera.pixelWidth > 0.5f)
                {
                    return UserAction.TouchRight;
                }
                else if (mouseStartPos.x / mainCamera.pixelWidth <= 0.5f)
                {
                    return UserAction.TouchLeft;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Vector3 mousePos = Input.mousePosition;
            float mouseUpTime = Time.time;

            // Debug.Log("" + action.ToString());
            // Debug.Log($"{mouseUpTime}, {mouseStartDownTime}, {mouseStartMoveTime}");
            // Debug.Log($"{mouseUpTime - mouseStartMoveTime}");
            // Debug.Log($"{mouseUpTime - mouseStartDownTime}");

            if (mouseUpTime - mouseStartDownTime <= mouseTapTime &&
                mouseStartPos.x == mousePos.x &&
                mouseStartPos.y == mousePos.y)
            {
                return UserAction.Tap;
            }
            else if (mouseUpTime - mouseStartMoveTime <= mouseSwipeTime &&
                Mathf.Abs(mouseStartPos.x - mousePos.x) >= swipeLength &&
                mousePos.x - mouseStartPos.x > 0)
            {
                return UserAction.SwipeRight;
            }
            else if (mouseUpTime - mouseStartMoveTime <= mouseSwipeTime &&
                Mathf.Abs(mouseStartPos.x - mousePos.x) >= swipeLength &&
                mousePos.x - mouseStartPos.x < 0)
            {
                return UserAction.SwipeLeft;
            }
        }

        return UserAction.None;
    }

    void UpdateInputDebug()
    {
        // Prevent user's input
        if (UseUserInput == false)
            return;

        // If game is over
        if (gameManager.IsGameOver)
        {
            // Do nothing
        }
        else
        {
            UserAction action = GetUserActionByMouse();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                gameManager.PauseResumeGame();
            }

            // If game is paused
            if (gameManager.IsGamePaused)
            {
                // Do nothing
            }
            // If game continue
            else if (gameManager.IsLevelComplete == false)
            {
                // If jump to stage in Level 4
                if (isJumpToStage)
                {
                    MoveToStage();

                    return;
                }

                // If holding rope in Level 5
                if (isHoldingRope)
                {
                    transform.position = ropeCs.GetLooseEndPosition() - animationHoldingRopeOffset;

                    if (Input.GetKey(KeyCode.Space) || 
                        action == UserAction.Tap ||
                        action == UserAction.SwipeLeft ||
                        action == UserAction.SwipeRight)
                    {
                        isHoldingRope = false;
                        isJumping = true;

                        transform.rotation = Quaternion.Euler(0, 90, 0);
                        transform.position += (Vector3.forward * animationHoldingRopeOffset.z);

                        jumpingDirection = JumpingDirection.Up;

                        if (action == UserAction.SwipeRight)
                            jumpingDirection = JumpingDirection.Forward;
                        else if (action == UserAction.SwipeLeft)
                            jumpingDirection = JumpingDirection.Backward; 

                        rbPlayer.isKinematic = false;
                        rbPlayer.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

                        sfxController.PlayJumpSfx();
                    }

                    if (Input.GetKey(KeyCode.RightArrow) || action == UserAction.TouchRight)
                    {
                        jumpingDirection = JumpingDirection.Forward;
                    }
                    else if (Input.GetKey(KeyCode.LeftArrow) || action == UserAction.TouchLeft)
                    {
                        if (isJumping)
                            transform.rotation = Quaternion.Euler(0, -90, 0);

                        jumpingDirection = JumpingDirection.Backward;
                    }
                }
                // If jumping
                else if (isJumping)
                {
                    if (horse != null)
                    {
                        if (Input.GetKey(KeyCode.LeftArrow) || action == UserAction.TouchLeft)
                        {
                            KeepLowBounce = true;
                        }
                        else
                        {
                            KeepLowBounce = false;
                        }

                        transform.position = new Vector3(horse.transform.position.x, transform.position.y, transform.position.z);
                    }
                    else
                    {
                        if (jumpingDirection == JumpingDirection.Forward)
                            MoveForward();
                        else if (jumpingDirection == JumpingDirection.Backward)
                            MoveBackward();
                    }
                }
                // If walk/run normally
                else
                {
                    if (Input.GetKeyDown(KeyCode.Space) || 
                        action == UserAction.Tap ||
                        action == UserAction.SwipeLeft ||
                        action == UserAction.SwipeRight)
                    {
                        isJumping = true;
                        jumpingDirection = JumpingDirection.Up;

                        if (action == UserAction.SwipeRight)
                            jumpingDirection = JumpingDirection.Forward;
                        else if (action == UserAction.SwipeLeft)
                            jumpingDirection = JumpingDirection.Backward;

                        playerAnim.SetBool("Jumping_b", true);

                        UpdateCollider();

                        rbPlayer.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

                        sfxController.PlayJumpSfx();
                    }

                    if (Input.GetKey(KeyCode.RightArrow) || action == UserAction.TouchRight)
                    {
                        if (horse != null)
                        {
                            // Do nothing
                        }
                        else
                        {
                            if (isJumping)
                            {
                                jumpingDirection = JumpingDirection.Forward;
                            }
                            else
                            {
                                playerAnim.SetBool("Running_b", true);
                                playerAnim.SetInteger("Direction_i", 1);

                                MoveForward();
                            }
                        }
                    }
                    else if (Input.GetKey(KeyCode.LeftArrow) || action == UserAction.TouchLeft)
                    {
                        if (horse != null)
                        {
                            horseCs.ChangeSpeed(false);
                        }
                        else
                        {
                            if (isJumping)
                            {
                                jumpingDirection = JumpingDirection.Backward;
                            }
                            else
                            {
                                playerAnim.SetBool("Running_b", true);
                                playerAnim.SetInteger("Direction_i", -1);

                                MoveBackward();
                            }
                        }
                    }
                    else
                    {
                        playerAnim.SetBool("Running_b", false);
                        playerAnim.SetInteger("Direction_i", 0);

                        if (horse != null)
                        {
                            horseCs.ChangeSpeed(true);
                        }
                    }
                }
            }
        }
    }

    void MoveForward()
    {
        float finalSpeed = isJumping ? jumpRange : runSpeed;

        transform.position += Vector3.right * finalSpeed * Time.deltaTime;

        MoveForwardEnd();
    }

    void MoveBackward()
    {
        float finalSpeed = isJumping ? jumpRange : runSpeed;

        transform.position += Vector3.left * finalSpeed * Time.deltaTime;

        MoveBackwardEnd();
    }

    void MoveForwardEnd()
    {
        if (transform.position.x > stage.transform.position.x)
        {
            transform.position = new Vector3(stage.transform.position.x, transform.position.y, transform.position.z);
        }
    }

    void MoveBackwardEnd()
    {
        if (transform.position.x < startPos.x)
        {
            transform.position = new Vector3(startPos.x, transform.position.y, transform.position.z);
        }
    }

    void UpdateSpeedForBouncing()
    {
        horseCs.ChangeSpeed(!KeepLowBounce);
    }

    void UpdateDirectionForBouncing()
    {
        UserAction action = GetUserActionByMouse();

        if (Input.GetKey(KeyCode.RightArrow) || action == UserAction.TouchRight)
        {
            jumpingDirection = JumpingDirection.Forward;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || action == UserAction.TouchLeft)
        {
            jumpingDirection = JumpingDirection.Backward;
        }
        else
            jumpingDirection = JumpingDirection.Up;
    }

    public bool IsJumpingUp()
    {
        return jumpingDirection == JumpingDirection.Up;
    }

    public bool IsJumpingToStage()
    {
        return isJumpToStage;
    }

    public void JumpToStage()
    {
        UseUserInput = true;
        isJumpToStage = true;
        isJumping = true;
        jumpingDirection = JumpingDirection.Up;

        playerAnim.SetBool("Jumping_b", true);

        // A fixed number for calculate collider when player jump up
        playerCollider.height -= 0.2f;

        rbPlayer.constraints |= RigidbodyConstraints.FreezePositionX;
        rbPlayer.AddForce(Vector3.up * jumpForce * 1.8f, ForceMode.Impulse);
    }

    public void MoveToStage()
    {
        transform.position = Vector3.MoveTowards(transform.position, stage.transform.position, jumpForce * Time.deltaTime);
    }

    public void UpdateCollider()
    {
        if (isJumping)
        {
            // A fixed number for calculate collider when player jump up
            playerCollider.height -= 0.2f;
        }
        else
        {
            playerCollider.height = colliderHeight;
        }
    }
}
