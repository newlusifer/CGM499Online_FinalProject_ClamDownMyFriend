using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showMap : MonoBehaviour
{
    public GameObject hideMap1;
    public GameObject hideMap2;
    public GameObject hideMap3;

    public float timer = 6f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            if (randomMap.randomMapValue == "1" || randomMap.randomMapValue == "3" || randomMap.randomMapValue == "7")
            {
                StartCoroutine(waitForShowMap1());
            }

            if (randomMap.randomMapValue == "2" || randomMap.randomMapValue == "9" || randomMap.randomMapValue == "5")
            {
                StartCoroutine(waitForShowMap2());
            }

            if (randomMap.randomMapValue == "8" || randomMap.randomMapValue == "6" || randomMap.randomMapValue == "4")
            {
                StartCoroutine(waitForShowMap3());
            }
        }

       


       
    }

    IEnumerator waitForShowMap1()
    {
        hideMap1.SetActive(false);

        yield return new WaitForSeconds(2);

        hideMap1.SetActive(true);
        timer = 6.0f;
    }

    IEnumerator waitForShowMap2()
    {
        hideMap2.SetActive(false);

        yield return new WaitForSeconds(2);

        hideMap2.SetActive(true);
        timer = 6.0f;
    }

    IEnumerator waitForShowMap3()
    {
        hideMap3.SetActive(false);

        yield return new WaitForSeconds(2);

        hideMap3.SetActive(true);
        timer = 6.0f;
    }
}
