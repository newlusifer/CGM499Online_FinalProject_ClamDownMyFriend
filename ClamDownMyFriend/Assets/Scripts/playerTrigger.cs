using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerTrigger : MonoBehaviour
{

    public GameObject respawnPoint;
    private Vector3 respawnPVec;
    public static int statusWinner = 0;

    // Start is called before the first frame update
    void Start()
    {
        respawnPoint = GameObject.Find("respawnPoint");
        respawnPVec = new Vector3(respawnPoint.transform.position.x, respawnPoint.transform.position.y, respawnPoint.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        
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
            }

            if (ConnectionManager.statusGame == "stop")
            {
                statusWinner = 0;
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
}
