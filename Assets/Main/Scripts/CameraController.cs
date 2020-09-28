using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 defaultOffset;

    float targetAspectX = 16.0f;
    float targetAspectY = 9.0f;
    float distancePlayerAndStage = 10.0f;

    Vector3 cameraOffset;

    Camera cameraComponent;
    GameObject player;
    GameObject stage;

    // Start is called before the first frame update
    void Start()
    {
        cameraComponent = GetComponent<Camera>();
        player = GameObject.Find("Player");
        stage = GameObject.Find("Stage");

        // Calculate camera position for the first time
        float targetAspect = targetAspectX / targetAspectY;
        cameraOffset = defaultOffset / cameraComponent.aspect * targetAspect; 

        transform.position = player.transform.position + cameraOffset;
    }

    // Update is called once per frame
    void Update()
    {
        if (stage.transform.position.x - player.transform.position.x > distancePlayerAndStage)
        {
            transform.position = new Vector3(player.transform.position.x + cameraOffset.x,
                cameraOffset.y,
                cameraOffset.z);
        }
    }
}
