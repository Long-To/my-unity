﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public GameObject mPrefab;
    public Vector3[] mPickUpPositions;
    public int mIdxPickUp;
    public float mSpeed;
    public Animator mAnimPlayerController;
    private bool mPlayerIsFalling;
    public ParticleSystem mPlayerRespawn;
    public Slider mBlood;
    public TextMeshProUGUI mPlayedTime;
    public TextMeshPro mWinOrLose;
    private AudioSource mSfxSmallExplosion;

    // Start is called before the first frame update
    void Start()
    {
        InGameMenuController.mGameIsPaused = false;
        mSfxSmallExplosion = GetComponent<AudioSource>();
        mSfxSmallExplosion.volume = MainMenuController.mSfxVolume;
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (!InGameMenuController.mGameIsPaused)
        {
            UpdatePlayedTime();
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
                mWinOrLose.text = "";
                Reset();
                mPickUpPositions = GetObjectPositions(GameObject.FindGameObjectsWithTag("Pick Up"));
            }

            if (mPlayerIsFalling && mAnimPlayerController.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                AddBloodValue(-0.2f);
                if (mBlood.value == 0)
                {
                    mWinOrLose.text = "Lose!";
                    mAnimPlayerController.SetInteger("IdxAnim", 6);
                }
                else
                {
                    mPlayerIsFalling = false;
                    mPlayerRespawn.Play();
                }
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
                        if (mAnimPlayerController.GetCurrentAnimatorStateInfo(0).IsName("Death"))
                        {
                            this.transform.parent.position = Vector3.zero;
                            mPlayerRespawn.Play();
                            mAnimPlayerController.SetInteger("IdxAnim", 7);
                        }
                        else if (mAnimPlayerController.GetCurrentAnimatorStateInfo(0).IsName("Bellydancing"))
                        {
                            mAnimPlayerController.SetInteger("IdxAnim", 5);
                        }
                        else
                        {
                            if (mPlayerRespawn.isStopped)
                            {
                                mAnimPlayerController.SetInteger("IdxAnim", 1);
                                Vector3 direction = mPickUpPositions[mIdxPickUp];
                                direction.y = this.transform.position.y/2f;
                                this.transform.LookAt(direction);
                                this.transform.parent.position = Vector3.MoveTowards(this.transform.parent.position, mPickUpPositions[mIdxPickUp], mSpeed * Time.deltaTime );
                            }
                        }
                    }
                }
            }
            else
            {    
                if (!mPlayerIsFalling)
                {
                    mAnimPlayerController.SetInteger("IdxAnim", 3);
                    if (mBlood.value > 0 && mPickUpPositions != null && mPickUpPositions.Length > 0)
                    {
                        mWinOrLose.text = "Win!";
                        if (mAnimPlayerController.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                        {
                            mAnimPlayerController.SetInteger("IdxAnim", 4);
                        }
                    } 
                }
            }
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
                mPlayerIsFalling = true;
                mAnimPlayerController.SetInteger("IdxAnim", 2);
                mSfxSmallExplosion.Play();
                other.gameObject.GetComponentInChildren<ParticleSystem>().Play();
            }
            else
            {
                AddBloodValue(0.1f);
            }

            other.gameObject.GetComponent<Animator>().SetInteger("IdxAnim", 4);
        }    
    }

    void AddBloodValue(float value)
    {
        if (mBlood.value + value  < 0.0f)
        {
            mBlood.value = 0.0f;
        }
        else if (mBlood.value + value  > 1.0f)
        {
            mBlood.value = 1.0f;
        }
        else
        {
            mBlood.value += value;
        }
    }

    void Reset()
    {
        mBlood.value = 1.0f;
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

    void UpdatePlayedTime()
    {
        float time = Time.time;
        string min = ((int)time / 60).ToString("00");
        string sec = (time % 60).ToString("00");
        mPlayedTime.text = min + ":" + sec;
    }
}
