using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDelayer : MonoBehaviour
{
    public float animationStartDelay;

    IEnumerator StartAnimation()
    {
        yield return new WaitForSeconds(animationStartDelay);
        GetComponent<Animator>().enabled = true;
    }
    void Start()
    {
        if (animationStartDelay != 0)
        {
            GetComponent<Animator>().enabled = false;
            StartCoroutine(StartAnimation());
        }
    }
}
