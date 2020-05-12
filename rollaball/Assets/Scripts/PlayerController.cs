using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Rigidbody mRB;
    public float mSpeed;
    public Text mCountText;
    public Text mWinText;

    private int mCount;

    // Start is called before the first frame update
    void Start()
    {
        mRB = GetComponent<Rigidbody>();
        mCount = 0;
        mWinText.text = "";
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        mRB.AddForce(movement * mSpeed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            mCount =  mCount + 1;
            SetCountText();
        }    
    }

    void SetCountText()
    {
        mCountText.text = "Count: " + mCount.ToString();
        if (mCount >= 5)
        {
            mWinText.text = "You Win!";
        }
    }
}
