using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField] GameObject looseStart = null;
    [SerializeField] GameObject looseEnd = null;
    [SerializeField] bool swingToRight = true;
    [SerializeField] float maxSwingEulerAngle = 28.0f;
    [SerializeField] float swingSpeed = 35.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Swing();
    }

    void Swing()
    {
        looseStart.transform.rotation *= Quaternion.AngleAxis(swingSpeed * (swingToRight ? 1 : -1) * Time.deltaTime, Vector3.forward);

        float angle = Quaternion.Angle(looseStart.transform.rotation, Quaternion.AngleAxis(0, Vector3.forward));

        if (angle >= maxSwingEulerAngle ||
            angle <= -maxSwingEulerAngle)
        {
            looseStart.transform.rotation = looseStart.transform.rotation.z > 0 ? Quaternion.Euler(0, 0, maxSwingEulerAngle) : Quaternion.Euler(0, 0, -maxSwingEulerAngle);
            swingToRight = !swingToRight;
        }
    }

    public Vector3 GetLooseEndPosition()
    {
        return looseEnd.transform.position;
    }
}
