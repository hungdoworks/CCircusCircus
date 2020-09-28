using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeyController : MonoBehaviour
{
    [SerializeField] bool isRunningMode = false;

    Animator monkeyAnimator;

    // Start is called before the first frame update
    void Start()
    {
        monkeyAnimator = GetComponentInChildren<Animator>();

        SetDefaultMode();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable()
    {
        // This block code to handle animation is not correct after return from inactive state.
        if (monkeyAnimator != null)
        {
            SetDefaultMode();
        }
    }

    void SetDefaultMode()
    {
        if (isRunningMode)
        {
            Run();
        }
        else
        {
            Walk();
        }
    }

    public void Walk()
    {
        monkeyAnimator.SetBool("Walk", true);
    }

    public void Run()
    {
        monkeyAnimator.SetBool("Run", true);
    }
}
