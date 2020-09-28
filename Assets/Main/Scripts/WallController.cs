using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
    GameObject startWall;
    BallManager ballManager;

    // Start is called before the first frame update
    void Start()
    {
        startWall = GameObject.Find("StartWall");
        ballManager = GameObject.Find("BallManager")?.GetComponent<BallManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            ObstacleController obstacleCollider = other.gameObject.GetComponent<ObstacleController>();
            obstacleCollider.BackToStartPoint(startWall.transform.position);
        }
        else if (other.CompareTag("JumpingMonkey"))
        {
            JumpingMonkey jumpingMonkey = other.gameObject.GetComponent<JumpingMonkey>();
            jumpingMonkey.BackToStartPoint();
        }
        else if (other.CompareTag("Sphere"))
        {
            GameObject ball = other.gameObject.transform.parent.gameObject;
            ball.SetActive(false);

            ballManager.AddToPendingList(ball);
        }
    }
}
