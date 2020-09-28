using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeyTrapController : MonoBehaviour
{
    [SerializeField] GameObject jumpingMonkey = null;

    JumpingMonkey jumpingMonkeyCS = null;

    // Start is called before the first frame update
    void Start()
    {
        jumpingMonkeyCS = jumpingMonkey.GetComponent<JumpingMonkey>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject player = other.gameObject;

            float monkeyPosX = jumpingMonkeyCS.transform.position.x;
            float playerPosX = player.transform.position.x;
            float distanceBetweenMonkeyAndPlayer = Mathf.Abs(monkeyPosX - playerPosX);

            if (distanceBetweenMonkeyAndPlayer > 20)
            {
                jumpingMonkeyCS.BackToStartPoint();
                jumpingMonkeyCS.Attack();
            }
        }
    }
}
