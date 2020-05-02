using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerTrigger : MonoBehaviour
{
    public Vector3 respawnPoint = new Vector3(0,4,-5);
    public GameObject winner;

    // Start is called before the first frame update
    void Start()
    {
        winner.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="dead")
        {
            StartCoroutine(waitForRespwan());
        }

        if (other.tag == "goal")
        {
            winner.SetActive(true);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
       
    }

    IEnumerator waitForRespwan()
    {
        yield return new WaitForSeconds(1f);
        transform.position = respawnPoint;
    }
}
