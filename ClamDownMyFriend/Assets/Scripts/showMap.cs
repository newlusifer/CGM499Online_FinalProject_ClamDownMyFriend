using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showMap : MonoBehaviour
{
    public GameObject hideMap;
    public float timer = 6f;

    // Start is called before the first frame update
    void Start()
    {
        hideMap = GameObject.Find("hideMap");
        hideMap.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer<=0)
        {
            StartCoroutine(waitForShowMap());
        }
    }

    IEnumerator waitForShowMap()
    {
        hideMap.SetActive(false);

        yield return new WaitForSeconds(2);

        hideMap.SetActive(true);
        timer = 6.0f;
    }
}
