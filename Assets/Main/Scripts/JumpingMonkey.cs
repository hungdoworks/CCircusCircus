using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingMonkey : MonoBehaviour
{
    [SerializeField] float speed = 5.0f;
    [SerializeField] float jumpForce = 5.0f;

    public bool IsRunning { get; set; }

    bool isJumping = false;
    int attackRange = 8;

    Vector3 startPos;

    GameObject player;
    Rigidbody rbMonkey = null;
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;

        player = GameObject.Find("Player");
        rbMonkey = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        BackToStartPoint();
    }

    // Update is called once per frame
    void Update()
    {   
        if (gameManager.IsGamePaused || gameManager.IsGameOver)
        {
            // Do nothing
        }
        else if (IsRunning)
        {
            // The monkey is running
            transform.position += Vector3.left * speed * Time.deltaTime;

            // Detect if player within the range
            float distanceToPlayer = transform.position.x - player.transform.position.x;

            if (isJumping == false && distanceToPlayer > 0 && distanceToPlayer < attackRange)
            {
                isJumping = true;

                rbMonkey.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            rbMonkey.isKinematic = true;
        }
    }

    public void Attack()
    {
        gameObject.SetActive(true);

        IsRunning = true;
    }

    public void BackToStartPoint()
    {
        IsRunning = false;

        gameObject.SetActive(false);

        isJumping = false;
        transform.position = startPos;
    }
}
