using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    [SerializeField] GameObject root = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BackToStartPoint(Vector3 startPos)
    {
        if (root != null)
            root.transform.position = new Vector3(startPos.x, root.transform.position.y, root.transform.position.z);
        else
            transform.position = new Vector3(startPos.x, transform.position.y, transform.position.z);
    }
}
