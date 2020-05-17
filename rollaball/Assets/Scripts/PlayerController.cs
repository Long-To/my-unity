using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Rigidbody mRB;
    public GameObject mPrefab;
    public GameObject[] mPickUps;
    private int mIdxPickUp;
    public float mSpeed;
    public Text mCountText;
    public Text mWinText;
    private int mCount;

    // Start is called before the first frame update
    void Start()
    {
        mRB = GetComponent<Rigidbody>();
        mCount = 0;
        mIdxPickUp = 0;
        mWinText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Pick Up"))
                {
                    hit.collider.gameObject.SetActive(false);
                }
                else
                {
                    Vector3 postion = hit.point;
                    postion.y = 0.5f;
                    Instantiate(mPrefab, postion, Quaternion.identity);
                }
            }
        }
        else if(Input.GetKeyDown(KeyCode.Space))
        {
            mPickUps = GameObject.FindGameObjectsWithTag("Pick Up");
        }

        if (mPickUps != null && mIdxPickUp < mPickUps.Length)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, mPickUps[mIdxPickUp].transform.position, mSpeed * Time.deltaTime );
        }
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
            if (other.transform.position == mPickUps[mIdxPickUp].transform.position)
            {
                mIdxPickUp = mIdxPickUp + 1;
            }
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
