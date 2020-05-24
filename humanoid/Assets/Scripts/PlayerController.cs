using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject mPrefab;
    public Vector3[] mPickUpPositions;
    public int mIdxPickUp;
    public float mSpeed;
    public Text mCountText;
    public Text mWinText;
    public int mCount;
    public Animator mAnimPlayerController;
    private bool mPlayerIsFalling;
    public ParticleSystem mPlayerRespawn;

    // Start is called before the first frame update
    void Start()
    {
        mCount = 0;
        Reset();
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
                    hit.collider.gameObject.GetComponent<Animator>().SetInteger("IdxAnim", 4);
                }
                else
                {
                    int color = Random.Range(1,4);
                    Vector3 postion = hit.point;
                    postion.y = 0.5f;
                    mPrefab.Spawn(postion).GetComponent<Animator>().SetInteger("IdxAnim", color);
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Reset();
            SetCountText();
            mPickUpPositions = GetObjectPositions(GameObject.FindGameObjectsWithTag("Pick Up"));
        }

        if (mPlayerIsFalling && mAnimPlayerController.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            mPlayerIsFalling = false;
            mPlayerRespawn.Play();
        }
        else if (mPickUpPositions != null && mIdxPickUp < mPickUpPositions.Length)
        {
            if (mPickUpPositions[mIdxPickUp] == Vector3.zero)
            {
                mIdxPickUp++;
            }
            else
            {
                if (!mPlayerIsFalling)
                {
                    mAnimPlayerController.SetInteger("IdxAnim", 1);
                    Vector3 direction = mPickUpPositions[mIdxPickUp];
                    direction.y = this.transform.position.y;
                    this.transform.LookAt(direction);
                    this.transform.parent.position = Vector3.MoveTowards(this.transform.parent.position, mPickUpPositions[mIdxPickUp], mSpeed * Time.deltaTime );
                }
            }
        }
        else if (!mPlayerIsFalling)
        {
            mAnimPlayerController.SetInteger("IdxAnim", 3);
        }
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

            if (other.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("RedAnimPickUp"))
            {
                mCount--;
                mPlayerIsFalling = true;
                mAnimPlayerController.SetInteger("IdxAnim", 2);
                other.gameObject.GetComponentInChildren<ParticleSystem>().Play();
            }
            else
            {
                mCount++;
            }

            other.gameObject.GetComponent<Animator>().SetInteger("IdxAnim", 4);
            SetCountText();
        }    
    }

    void SetCountText()
    {
        mCountText.text = "Count: " + mCount.ToString();
        if (mCount >= 30)
        {
            mWinText.text = "You Win!";
        }
    }

    void Reset()
    {
        mIdxPickUp = 0;
        mPlayerIsFalling = false;
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
