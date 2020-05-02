using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool canControl = false;
    public float speedMove = 2.5f;

    public GameObject camera;
   
    void Start()
    {
        camera = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        if (!canControl)
            return;

        camera.transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z - 1);
        Vector3 axis = (Vector3.right * Input.GetAxis("Horizontal")) +
                        (Vector3.up * Input.GetAxis("Vertical"));
        axis.Normalize();
        this.transform.position += axis * speedMove * Time.deltaTime;
    }
}
