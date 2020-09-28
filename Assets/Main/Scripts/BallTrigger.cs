using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTrigger : MonoBehaviour
{
    BallManager ballManager;

    // Start is called before the first frame update
    void Start()
    {
        ballManager = GameObject.Find("BallManager").GetComponent<BallManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ballManager.ToggleKeepReleasingBall();
        }
    }
}
