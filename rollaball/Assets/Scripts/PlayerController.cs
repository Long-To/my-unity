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
        // SimplePool.Preload(mPrefab, 20);
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
                    // hit.collider.gameObject.SetActive(false);
                    // SimplePool.Despawn(hit.collider.gameObject);
                    hit.collider.gameObject.Kill();
                }
                else
                {
                    Vector3 postion = hit.point;
                    postion.y = 0.5f;
                    // Instantiate(mPrefab, postion, Quaternion.identity);
                    // SimplePool.Spawn(mPrefab, postion, Quaternion.identity);
                    mPrefab.Spawn(postion);
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Reset();
            SetCountText();
            mPickUps = GameObject.FindGameObjectsWithTag("Pick Up");
        }

        if (mPickUps != null && mIdxPickUp < mPickUps.Length)
        {
            if (mPickUps[mIdxPickUp].transform.position == Vector3.zero)
            {
                mIdxPickUp++;
            }
            else
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, mPickUps[mIdxPickUp].transform.position, mSpeed * Time.deltaTime );
            }
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
            if (other.transform.position == mPickUps[mIdxPickUp].transform.position)
            {
                mIdxPickUp++;
            }
            else
            {
                for (int i = 0; i < mPickUps.Length; i++)
                {
                    if (other.transform.position == mPickUps[i].transform.position)
                    {
                        mPickUps[i].transform.position = Vector3.zero;
                    }
                }
            }

            // other.gameObject.SetActive(false);
            // SimplePool.Despawn(other.gameObject);
            other.gameObject.Kill();
            mCount++;
            SetCountText();
        }    
    }

    void SetCountText()
    {
        mCountText.text = "Count: " + mCount.ToString();
        if (mCount >= 15)
        {
            mWinText.text = "You Win!";
        }
    }

    void Reset()
    {
        mCount = 0;
        mIdxPickUp = 0;
        mPickUps = null;
        mWinText.text = "";
    }
}
