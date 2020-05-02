using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showMap : MonoBehaviour
{
    public GameObject hideMap;
    public float cdShowMap = 6f;

    // Start is called before the first frame update
    void Start()
    {
        hideMap.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (cdShowMap>0)
        {
            cdShowMap -= Time.deltaTime;
        }
       
        if (cdShowMap<=0)
        {
            StartCoroutine(timeShowMap());
        }
    }   

    IEnumerator timeShowMap()
    {
        hideMap.SetActive(false);

        yield return new WaitForSeconds(2f);

        hideMap.SetActive(true);
        cdShowMap = 6f;
    }
}
