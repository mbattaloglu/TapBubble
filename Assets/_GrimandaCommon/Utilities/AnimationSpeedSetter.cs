using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSpeedSetter : MonoBehaviour
{
    [SerializeField] float animationSpeedMultiplier=1;
    // Start is called before the first frame update

    void Start()
    {
        GetComponent<Animator>().speed *= animationSpeedMultiplier;   
    }
}
