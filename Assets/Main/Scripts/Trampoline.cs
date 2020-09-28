using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] bool prebounceAnimation = true;
    [SerializeField] float bounce = 5.0f;
    [SerializeField] float bounceDelay = 0.1f;

    bool isPreBounce = false;
    float offsetY = 0.2f;

    PlayerController playerController;
    SfxController sfxController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        sfxController = GameObject.Find("SFX").GetComponent<SfxController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (isPreBounce == false)
            {
                Rigidbody rbPlayer = other.rigidbody;

                PreBounce(rbPlayer);
            }
        }
    }

    void PreBounce(Rigidbody rbOther)
    {
        isPreBounce = true;

        if (prebounceAnimation)
            transform.localPosition -= new Vector3(0, offsetY, 0);

        StartCoroutine(BounceUp(rbOther));
    }

    IEnumerator BounceUp(Rigidbody rbOther)
    {
        yield return new WaitForSeconds(bounceDelay);

        rbOther.AddForce(Vector3.up * bounce, ForceMode.Impulse);

        if (prebounceAnimation)
            transform.localPosition += new Vector3(0, offsetY, 0);

        sfxController.PlayTrampolineSfx();

        yield return new WaitForSeconds(0.2f);

        isPreBounce = false;
    }
}
