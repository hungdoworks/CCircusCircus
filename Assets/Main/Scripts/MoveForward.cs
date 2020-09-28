using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float Speed = 5.0f;
    public Vector3 Direction = Vector3.left;

    GameManager gameManager;

    Rigidbody rbSelf;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        rbSelf = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.IsGamePaused || gameManager.IsGameOver || gameManager.IsLevelComplete)
        {
            // Do nothing
        }
        else
        {
            if (rbSelf == null || rbSelf.isKinematic == false)
            {
                transform.position += Direction * Speed * Time.deltaTime;
            }
        }
    }

    void FixedUpdate()
    {
        if (gameManager.IsGamePaused || gameManager.IsGameOver || gameManager.IsLevelComplete)
        {
            // Do nothing
        }
        else if (rbSelf.isKinematic)
        {
            rbSelf.MovePosition(transform.position + (Direction * Speed * Time.fixedDeltaTime));
        }
    }
}
