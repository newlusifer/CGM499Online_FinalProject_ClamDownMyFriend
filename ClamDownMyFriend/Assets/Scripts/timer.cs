using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timer : MonoBehaviour
{
    private float time = 0;
    public Text minute;
    public Text sec;

    private int m = 0;
    private int s = 0;

    private string statusClock = "";

    public static string minuteShare = "";
    public static string secondShare ="";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Status Game Is.... "+ ConnectionManager.statusGame);

        statusClock=ConnectionManager.statusGame;
        Debug.Log("Status clock Is.... " + statusClock);

        if (statusClock == "start")
        {
           
            time += Time.deltaTime;

            if (time >= 60)
            {
                m++;
                time = 0;
            }

            minute.text = m.ToString();
            s = Mathf.RoundToInt(time);
            sec.text = s.ToString();

            minuteShare = m.ToString();
            secondShare = s.ToString();
        }

        


        

    }
}
