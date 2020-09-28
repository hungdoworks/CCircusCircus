using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingStar : MonoBehaviour
{
    [SerializeField] List<Material> materials;
    [SerializeField] float blinkingSpeed = 1.0f;

    Material currentMat;
    int matIndex;

    // Start is called before the first frame update
    void Start()
    {
        currentMat = GetComponent<Renderer>().material;
        matIndex = 0;

        InvokeRepeating("Blinking", 0, blinkingSpeed);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Blinking()
    {
        if (matIndex < materials.Count - 1)
        {
            matIndex++;
        }
        else
        {
            matIndex = 0;
        }

        currentMat.mainTexture = materials[matIndex].mainTexture;
    }
}
