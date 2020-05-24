using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    public ParticleSystem mPickUpFireBall;
    // Start is called before the first frame update
    void Start()
    {
        if (this.GetComponent<Animator>().GetInteger("IdxAnim") == 1)
        {
            mPickUpFireBall.Play();
        }
    }
}
