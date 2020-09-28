using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse : MonoBehaviour
{
    [SerializeField] GameObject player = null;
    [SerializeField] GameObject stage = null;

    public float WalkSpeed = 1.0f;
    public float RunSpeed = 7.0f;

    PlayerController playerController;

    MoveForward moveForward;

    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();

        moveForward = GetComponent<MoveForward>();
        moveForward.Speed = RunSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.IsJumpingToStage() == false)
        {
            if (player.transform.position.x != transform.position.x)
            {
                player.transform.position = new Vector3(transform.position.x,
                    player.transform.position.y,
                    player.transform.position.z);
            }
            else if (stage.transform.position.x - transform.position.x < 1.0f)
            {
                playerController.JumpToStage();
            }
        }
    }

    public void ChangeSpeed(bool runFast)
    {
        moveForward.Speed = runFast ? RunSpeed : WalkSpeed;

        // Quick update position
        player.transform.position = new Vector3(transform.position.x,
                player.transform.position.y,
                player.transform.position.z);
    }
}
