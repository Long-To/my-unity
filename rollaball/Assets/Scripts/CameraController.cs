using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject mPlayer;
    private Vector3 mOffset;
    // Start is called before the first frame update
    void Start()
    {
        mOffset = transform.position - mPlayer.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = mPlayer.transform.position + mOffset;
    }
}
