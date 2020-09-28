using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{

    GameObject startWall;
    GameObject balls;
    BallTrigger ballTrigger;

    List<GameObject> pendingBalls;
    bool isLongRangeBall = true;
    bool keepReleasingBall = true;


    // Start is called before the first frame update
    void Start()
    {
        startWall = GameObject.Find("StartWall");
        balls = GameObject.Find("Balls");
        ballTrigger = GameObject.Find("BallTrigger").GetComponent<BallTrigger>();

        pendingBalls = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pendingBalls.Count > 0 && keepReleasingBall)
        {
            float lastBallPos = 0.0f;

            foreach (Transform child in balls.transform)
            {
                if (child.gameObject.activeInHierarchy && child.position.x > lastBallPos)
                {
                    lastBallPos = child.position.x;
                }
            }

            if (startWall.transform.position.x - lastBallPos > (isLongRangeBall ? 20.0f : 10.0f))
            {
                isLongRangeBall = !isLongRangeBall;

                GameObject ball = pendingBalls[0];
                ball.transform.position = new Vector3(startWall.transform.position.x, ball.transform.position.y, ball.transform.position.z);
                ball.GetComponent<StandableBall>().OnReset();
                ball.SetActive(true);

                pendingBalls.Remove(ball);
            }
        }
    }

    public void AddToPendingList(GameObject ball)
    {
        pendingBalls.Add(ball);
    }

    public void ToggleKeepReleasingBall()
    {
        keepReleasingBall = !keepReleasingBall;
    }
}
