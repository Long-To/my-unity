using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform mTransform;
    private MeshRenderer mCubeRender;
    private Color[] mColors = {Color.red, Color.yellow, Color.blue};
    private int mColorIdx = 0;
    void Start()
    {
        mCubeRender = this.GetComponent<MeshRenderer>();
        mCubeRender.material.SetColor("_Color", mColors[mColorIdx]);
        mTransform = this.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            mColorIdx = (mColorIdx + 1) % mColors.Length;
            mCubeRender.material.SetColor("_Color", mColors[mColorIdx]);
            mTransform.Rotate(new Vector3(25f, 30f, 45f) * Time.deltaTime);
        }
    }
}
