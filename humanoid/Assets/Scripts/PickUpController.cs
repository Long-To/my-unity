using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    public ParticleSystem mPickUpFireBall;
    
    // Update is called once per frame
    void Update()
    {
        if (this.GetComponent<Animator>().GetInteger("IdxAnim") == 1 && !mPickUpFireBall.isPlaying)
        {
            mPickUpFireBall.Play();
        }
    }
}
