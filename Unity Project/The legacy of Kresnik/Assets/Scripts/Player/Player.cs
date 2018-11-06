using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

    //Movement
    public float maxSpeed = 3;
    public float speed = 50f;

    //Management
    private Rigidbody2D myBody;
    private Animator myAnim;

	// Use this for initialization
	void Start () {

        myBody = gameObject.GetComponent<Rigidbody2D>();
        myAnim = gameObject.GetComponent<Animator>();
        int playerLayer = LayerMask.NameToLayer("Player");

        myAnim.SetBool("FacingL", false);
        myAnim.SetBool("FacingR", false);

        Time.timeScale = 1;
    }
	
	// Update is called once per frame
	void Update () {

        myAnim.SetFloat("SpeedX", Mathf.Abs(myBody.velocity.x));
        myAnim.SetFloat("SpeedY", Mathf.Abs(myBody.velocity.y));

        //Movement
        if (Input.GetAxis("Horizontal") < -0.1f)
        {
            transform.localScale = new Vector3(-4, 4, 1);
        }

    if (Input.GetAxis("Horizontal") < 0.1f)
        {
            transform.localScale = new Vector3(4, 4, 1);
        }

        if (Input.GetAxis("Vertical") < -0.1f)
        {
            transform.localScale = new Vector3(4, -4, 1);
        }

    if (Input.GetAxis("Vertical") < 0.1f)
        {
            transform.localScale = new Vector3(4, 4, 1);
        }

    /*if (Input.GetAxis("Vertical") == 0)
        {
                myAnim.SetBool("FacingR", false);
                myAnim.SetBool("FacingU", false);
                myAnim.SetBool("FacingD", false);
                myAnim.SetBool("FacingL", false);
        }

    if(Input.GetAxis("Horizontal") == 0)
       {
            myAnim.SetBool("FacingR", false);
            myAnim.SetBool("FacingU", false);
            myAnim.SetBool("FacingD", false);
            myAnim.SetBool("FacingL", false);
       }*/
    }

    void FixedUpdate()
    {
        //Velocity
        Vector3 easeVelocity = myBody.velocity;
        easeVelocity.y = myBody.velocity.y;
        easeVelocity.z = 0;
        easeVelocity.x *= 0.75f;

        float moveX = Input.GetAxis("Horizontal");

        //Movement X
        myBody.AddForce((Vector2.right * speed)*moveX);
        if(myBody.velocity.x > maxSpeed)
        {
            myBody.velocity = new Vector2(maxSpeed, myBody.velocity.y);
            myAnim.SetBool("FacingL", false);
            myAnim.SetBool("FacingU", false);
            myAnim.SetBool("FacingD", false);
            myAnim.SetBool("FacingR", true);
        }

        if (myBody.velocity.x < -maxSpeed)
        {
            myBody.velocity = new Vector2(-maxSpeed, myBody.velocity.y);
            myAnim.SetBool("FacingR", false);
            myAnim.SetBool("FacingU", false);
            myAnim.SetBool("FacingD", false);
            myAnim.SetBool("FacingL", true);
        }

        //Movement Y
        float moveY = Input.GetAxis("Vertical");
                        
        myBody.AddForce((Vector2.up * speed) * moveY);
        if (myBody.velocity.y > maxSpeed)
        {
            myBody.velocity = new Vector2(maxSpeed, myBody.velocity.x);
            myAnim.SetBool("FacingR", false);
            myAnim.SetBool("FacingL", false);
            myAnim.SetBool("FacingD", false);
            myAnim.SetBool("FacingU", true);
        }

        if (myBody.velocity.y < -maxSpeed)
        {
            myBody.velocity = new Vector2(-maxSpeed, myBody.velocity.x);
            myAnim.SetBool("FacingR", false);
            myAnim.SetBool("FacingL", false);
            myAnim.SetBool("FacingU", false);
            myAnim.SetBool("FacingD", true);
        }
    }
}
