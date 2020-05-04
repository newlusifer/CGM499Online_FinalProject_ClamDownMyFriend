using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool canControl = false;
    public float speedMove = 2.5f;

    public GameObject camera;
    public float speed = 1f;
    private Animator animator;

    void Start()
    {
        camera = GameObject.Find("Main Camera");
        animator = GetComponent<Animator>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (!canControl)
            return;

        camera.transform.position = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z -1);

        /* Vector3 axis = (Vector3.right * Input.GetAxis("Horizontal")) +
                         (Vector3.up * Input.GetAxis("Vertical"));
         axis.Normalize();
         this.transform.position += axis * speedMove * Time.deltaTime;*/

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
            transform.rotation = Quaternion.Euler(0, 0, 0);
            animator.SetBool("Walk", true);

            Debug.Log(transform.position);
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            animator.SetBool("Walk", false);
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
            transform.rotation = Quaternion.Euler(0, 180, 0);
            animator.SetBool("Walk", true);
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            animator.SetBool("Walk", false);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
            transform.rotation = Quaternion.Euler(0, 90, 0);
            animator.SetBool("Walk", true);
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            animator.SetBool("Walk", false);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
            transform.rotation = Quaternion.Euler(0, -90, 0);
            animator.SetBool("Walk", true);
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            animator.SetBool("Walk", false);
        }
        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 3f;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 1f;
        }

    }
}
