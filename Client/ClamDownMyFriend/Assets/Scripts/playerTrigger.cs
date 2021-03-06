﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerTrigger : MonoBehaviour
{

    public GameObject respawnPoint;
    private Vector3 respawnPVec;
    public static int statusWinner = 0;
    public static int resetPosition = 0;

    public GameObject winnerSound;

    // Start is called before the first frame update
    void Start()
    {
        respawnPoint = GameObject.Find("respawnPoint");
        respawnPVec = new Vector3(respawnPoint.transform.position.x, respawnPoint.transform.position.y, respawnPoint.transform.position.z);

        //winnerSound = GameObject.Find("winnerSound");
        winnerSound.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (resetPosition==1)
        {
            transform.position = respawnPVec;
            resetPosition = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="dead")
        {
            StartCoroutine(waitForDead());
           // transform.position = respawnPVec;
        }

        if (other.tag == "goal")
        {
            if (ConnectionManager.statusGame=="start")
            {
                statusWinner = 1;

                Debug.Log("is win.....");

                StartCoroutine(waitForWinnerSound());
            }

            if (ConnectionManager.statusGame == "stop")
            {
                statusWinner = 0;
                Debug.Log("is not win.....");
            }
                
            
            
        }
    }

    //When the Primitive exits the collision, it will change Color
    private void OnTriggerExit(Collider other)
    {
       
    }

    IEnumerator waitForDead()
    {
        yield return new WaitForSeconds(1f);
        transform.position = respawnPVec;
    }

    IEnumerator waitForWinnerSound()
    {
        winnerSound.SetActive(true);
        yield return new WaitForSeconds(3f);
        winnerSound.SetActive(false);
    }
}
