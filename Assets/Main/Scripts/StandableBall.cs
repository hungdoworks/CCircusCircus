using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandableBall : MonoBehaviour
{
    [SerializeField] float bouncedBackSpeed = 10.0f;
    [SerializeField] float bouncedBackRotationSpeed = 3.0f;

    bool isFacingLeft = true;
    bool isBouncedBack = false;

    GameObject player = null;
    PlayerController playerController;
    GameManager gameManager;

    GameObject sphere = null;
    Rigidbody rbBall = null;
    MoveForward moveForward = null;

    float playerOldPos;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        rbBall = GetComponent<Rigidbody>();
        moveForward = GetComponent<MoveForward>();
        sphere = transform.Find("Sphere").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.IsGamePaused || gameManager.IsGameOver || gameManager.IsLevelComplete)
        {
            // Do nothing
        }
        // Update ball's position if have player on top of its
        else if (player != null && playerOldPos != player.transform.position.x)
        {
            float playerCurPosX = player.transform.position.x;
            isFacingLeft = playerOldPos > playerCurPosX;

            sphere.transform.Rotate(isFacingLeft ? Vector3.back * 2.0f : Vector3.forward * 2.0f);

            transform.position = new Vector3(playerCurPosX,
                transform.position.y,
                transform.position.z
            );

            playerOldPos = playerCurPosX;
        }
        else if (player == null)
        {
            float rotationSpeedUp = isBouncedBack ? bouncedBackRotationSpeed : 1;

            sphere.transform.Rotate(isFacingLeft ? Vector3.forward * rotationSpeedUp :
                                                Vector3.back * rotationSpeedUp);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
        }
        else if (other.CompareTag("Sphere"))
        {
            StandableBall otherBall = other.gameObject.GetComponentInParent<StandableBall>();

            if (isBouncedBack == false && otherBall.isBouncedBack == false)
            {
                isFacingLeft = transform.position.x > other.gameObject.transform.position.x;

                otherBall.OnBouncingBack();

                OnBouncingBack();
            }
        }
        else if (other.CompareTag("Player"))
        {
            // Check to avoid enter fast roll mode if player successfully jump on top of the ball
            if (player == null)
            {
                isFacingLeft = other.gameObject.transform.position.x < transform.position.x;

                OnBouncingBack();
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Check to avoid set player on top of the ball if player failed to made it 
            if (isBouncedBack == false)
            {
                player = other.gameObject;
                moveForward.enabled = false;
                playerController = player.GetComponent<PlayerController>();

                isFacingLeft = false;
                playerOldPos = player.transform.position.x;

                // Fix player's position does not correct when jump on from another the ball
                //sphere.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);

                transform.position = new Vector3(player.transform.position.x,
                    transform.position.y,
                    transform.position.z
                );
            }
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (player != null && playerController.IsJumpingUp() == false)
        {
            OnBouncingBack();
        }
    }

    public void OnBouncingBack()
    {
        isBouncedBack = true;
        player = null;

        moveForward.enabled = true;
        moveForward.Speed = bouncedBackSpeed;
        moveForward.Direction = isFacingLeft ? Vector3.right : Vector3.left;
    }

    public void OnReset()
    {
        // hack to avoid trigger and collision happen at the same time when player contact with the ball
        //sphere.transform.localPosition = new Vector3(0.0f, -0.04f, 0.0f);

        isFacingLeft = true;
        isBouncedBack = false;

        moveForward.Speed = 1;
        moveForward.Direction = Vector3.left;
    }
}
