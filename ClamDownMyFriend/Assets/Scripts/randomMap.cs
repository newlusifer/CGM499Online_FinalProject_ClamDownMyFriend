using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomMap : MonoBehaviour
{
    public GameObject map1;
    public GameObject map2;
    public GameObject map3;

    public static string randomMapValue = "1";

    // Start is called before the first frame update
    void Start()
    {
        map1 = GameObject.Find("map1");
        map2 = GameObject.Find("map2");
        map3 = GameObject.Find("map3");

        //randomMapValue=Random.Range(1, 9);

        
    }

    // Update is called once per frame
    void Update()
    {
        //ConnectionManager.randomMapFromServer = 0;

        randomMapValue = ConnectionManager.randomMapFromServer;

        if (randomMapValue == "1" || randomMapValue == "3" || randomMapValue == "7")
        {
            map1.SetActive(true);
            map2.SetActive(false);
            map3.SetActive(false);
        }

        if (randomMapValue == "2" || randomMapValue == "9" || randomMapValue == "5")
        {
            map1.SetActive(false);
            map2.SetActive(true);
            map3.SetActive(false);
        }

        if (randomMapValue == "8" || randomMapValue == "6" || randomMapValue == "4")
        {
            map1.SetActive(false);
            map2.SetActive(false);
            map3.SetActive(true);
        }

    }
}
