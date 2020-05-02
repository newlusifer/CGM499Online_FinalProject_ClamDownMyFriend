using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour
{

    private Animator animator;
    private bool isAnimating = false;
    public GameObject camera;
    public float speed = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {       
        camera.transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z-1);

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * Time.deltaTime*speed);
            transform.rotation= Quaternion.Euler(0, 0, 0);
            animator.SetBool("Walk",true);

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
            transform.rotation = Quaternion.Euler(0,-90, 0);
            animator.SetBool("Walk", true);
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            animator.SetBool("Walk", false);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            transform.Translate(Vector3.up * Time.deltaTime * 5f);
            animator.SetBool("Run", true);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            animator.SetBool("Run", false);
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
