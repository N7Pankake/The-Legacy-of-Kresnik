using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonTargetSkills : MonoBehaviour
{
    [SerializeField]
    private float speed;
    
    private Rigidbody2D myRigidBody;

    // Use this for initialization
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {

        myRigidBody.velocity = transform.forward * speed;

    }
}
