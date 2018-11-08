using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour {

    //Character Speed
    [SerializeField]
    private float speed;

    //Character Animation
    private Animator myAnimator;

    //Character Direction
    protected Vector2 direction;

    //Character RigidBody
    private Rigidbody2D myRigidBody;

    public bool IsMoving
    {
        get
        {
            return direction.x != 0 || direction.y != 0;
        }

    }

    // Use this for initialization
    protected virtual void Start ()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	protected virtual void Update ()
    {
        HandleLayers();
	}

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        //Move the Character
        myRigidBody.velocity = direction.normalized * speed;
    }

    public void HandleLayers()
    {
        //Checks if we are Idle or walking.
        if (IsMoving)
        {
            ActivateLayer("WalkLayer");

            //Set the right animation depending on which direction he is facing
            myAnimator.SetFloat("x", direction.x);
            myAnimator.SetFloat("y", direction.y);
        }
        else
        {
            //Change the layer back to Idle when we are not pressing Keys.
            ActivateLayer("IdleLayer");
        }
    }

    public void ActivateLayer(string layerName)
    {
        for (int i = 0; i < myAnimator.layerCount; i++)
        {
            myAnimator.SetLayerWeight(i, 0);
        }

        myAnimator.SetLayerWeight(myAnimator.GetLayerIndex(layerName),1);
    }

}
