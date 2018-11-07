using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

    //Player Speed
    [SerializeField]
    private float speed;

    //Player Direction
    private Vector2 direction;

    //Player Animation
    private Animator animator;
    
	// Use this for initialization
	void Start () {

        direction = Vector2.down;
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {

        {
            GetInpu();
            Move();
        }
    }

    public void Move()
    {
        //Move the player
        transform.Translate(direction * speed * Time.deltaTime);

        if(direction.x != 0 || direction.y !=0)
        {
          //Animate the player movement
          AnimateMovement(direction);
        }        
        else
        {
            animator.SetLayerWeight(1, 0);
        }
    }

    private void GetInpu()
    {
        direction = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector2.up;
        }

        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector2.left;
        }

        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector2.down;
        }

        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector2.right;
        }
    }


    public void AnimateMovement(Vector2 direction)
    {
        animator.SetLayerWeight(1, 1);

        //Set the right animation depending on which direction he is facing
        animator.SetFloat("x", direction.x);
        animator.SetFloat("y", direction.y);
    }
}
