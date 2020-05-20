using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Rigidbody mRB;
    public GameObject mPrefab;
    public Vector3[] mPickUpPositions;
    public int mIdxPickUp;
    public float mSpeed;
    public Text mCountText;
    public Text mWinText;
    public int mCount;

    // Start is called before the first frame update
    void Start()
    {
        mRB = GetComponent<Rigidbody>();
        // SimplePool.Preload(mPrefab, 20);
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
                    // hit.collider.gameObject.Kill();
                    hit.collider.gameObject.GetComponent<Animator>().SetInteger("IdxAnim", 3);
                }
                else
                {
                    Vector3 postion = hit.point;
                    postion.y = 0.5f;
                    // Instantiate(mPrefab, postion, Quaternion.identity);
                    // SimplePool.Spawn(mPrefab, postion, Quaternion.identity);
                    mPrefab.Spawn(postion).GetComponent<Animator>().SetInteger("IdxAnim", Random.Range(0,3));
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Reset();
            SetCountText();
            mPickUpPositions = GetObjectPositions(GameObject.FindGameObjectsWithTag("Pick Up"));
            this.gameObject.GetComponent<Animator>().SetInteger("IdxAnim", 1);
        }

        if (mPickUpPositions != null && mIdxPickUp < mPickUpPositions.Length)
        {
            if (mPickUpPositions[mIdxPickUp] == Vector3.zero)
            {
                mIdxPickUp++;
            }
            else
            {
                this.transform.LookAt(mPickUpPositions[mIdxPickUp]);
                this.transform.parent.position = Vector3.MoveTowards(this.transform.parent.position, mPickUpPositions[mIdxPickUp], mSpeed * Time.deltaTime );
            }
        }
        else
        {
            this.gameObject.GetComponent<Animator>().SetInteger("IdxAnim", 0);
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
            if (mPickUpPositions != null && mIdxPickUp < mPickUpPositions.Length && mPickUpPositions[mIdxPickUp] == other.transform.position)
            {
                mIdxPickUp++;
            }
            else
            {
                UpdateObjectPositions(other.transform.position);
            }

            // other.gameObject.SetActive(false);
            // SimplePool.Despawn(other.gameObject);
            // other.gameObject.Kill();
            other.gameObject.GetComponent<Animator>().SetInteger("IdxAnim", 3);
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
        mPickUpPositions = null;
        mWinText.text = "";
        mCountText.text = "";
    }

    Vector3[] GetObjectPositions(GameObject[] objects)
    {
        List<Vector3> tmp = new List<Vector3>();
        for (int i = 0; i < objects.Length; i++)
        {
            tmp.Add(objects[i].transform.position);
        }

        return tmp.ToArray();
    }

    void UpdateObjectPositions(Vector3 position)
    {
        for (int i = 0; i < mPickUpPositions.Length; i++)
        {
            if (mPickUpPositions[i] == position)
            {
                mPickUpPositions[i] = Vector3.zero;
            }
        }
    }
}
