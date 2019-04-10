using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonTargetSkills : MonoBehaviour
{
    private Rigidbody2D myRigidBody;

    // Use this for initialization
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();

        if (Player.MyInstance.upD)
        {
            myRigidBody.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
        }

        else if (Player.MyInstance.downD)
        {
            myRigidBody.AddForce(new Vector2(0, -10), ForceMode2D.Impulse);
        }

        else if (Player.MyInstance.leftD)
        {
            myRigidBody.AddForce(new Vector2(-10, 0), ForceMode2D.Impulse);
        }

        else if (Player.MyInstance.rightD)
        {
            myRigidBody.AddForce(new Vector2(10, 0), ForceMode2D.Impulse);
        }
    }
}
