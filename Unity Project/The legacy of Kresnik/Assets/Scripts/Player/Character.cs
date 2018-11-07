using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour {

    //Character Speed
    [SerializeField]
    private float speed;

    //Character Animation
    private Animator animator;

    //Character Direction
    protected Vector2 direction;

    // Use this for initialization
    protected virtual void Start ()
    {
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	protected virtual void Update ()
    {
        Move();
        AnimateMovement(direction);
	}

    public void Move()
    {
        //Move the Character
        transform.Translate(direction.normalized * speed * Time.deltaTime);

        if (direction.x != 0 || direction.y != 0)
        {
            //Animate the Character movement
            animator.SetLayerWeight(1, 1);
            AnimateMovement(direction);
        }
        else
        {
            animator.SetLayerWeight(1, 0);
        }
    }

    public void AnimateMovement(Vector2 direction)
    {
        //Set the right animation depending on which direction he is facing
        animator.SetFloat("x", direction.x);
        animator.SetFloat("y", direction.y);
    }

}
