using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]

public abstract class Character : MonoBehaviour {

    [SerializeField]
    private string type;
    public string MyType
    {
        get
        {
            return type;
        }
    }

    //Character Health
    [SerializeField]
    protected Stat health;

    public Stat MyHealth
    {
        get {return health;}
    }

    [SerializeField]
    protected float initHealth;

    //Character Speed
    [SerializeField]
    private float speed;

    [SerializeField]
    private int level;
    public int MyLevel
    {
        get
        {
            return level;
        }

        set
        {
            level = value;
        }
    }

    //Character Animation
    public Animator MyAnimator { get; set; }

    //Character Direction
    private Vector2 direction;

    //Character RigidBody
    private Rigidbody2D myRigidBody;

    public Transform MyTarget { get; set; }

    //Character Skills&Attacks
    protected bool isUsingSkill = false;
    
    public bool IsAttacking { get; set; }

    protected Coroutine skillRoutine;

    [SerializeField]
    protected Transform hitBox;

    public bool IsMoving
    {
        get
        {
            return direction.x != 0 || direction.y != 0;
        }

    }

    public Vector2 Direction
    {
        get
        {
            return direction;
        }

        set
        {
            direction = value;
        }
    }

    public float MySpeed
    {
        get
        {
            return speed;
        }

        set
        {
            speed = value;
        }
    }

    public bool IsAlive
    {
        get
        {
            return health.MyCurrentValue > 0;
        }
    }

    // Use this for initialization
    protected virtual void Start ()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        MyAnimator = GetComponent<Animator>();
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
        if(IsAlive)
        {
            //Move the Character
            myRigidBody.velocity = direction.normalized * speed;
        }
    }

    public void HandleLayers()
    {
        if (IsAlive)
        {
            if (IsMoving)
            {
                ActivateLayer("WalkLayer");

                //Set the right animation depending on which direction he is facing
                MyAnimator.SetFloat("x", direction.x);
                MyAnimator.SetFloat("y", direction.y);
            }
            else if (isUsingSkill)
            {
                ActivateLayer("SkillLayer");
            }
            else if (IsAttacking)
            {
                ActivateLayer("AttackLayer");
            }
            else
            {
                //Change the layer back to Idle when we are not pressing Keys.
                ActivateLayer("IdleLayer");
            }
        }
        else
        {
            ActivateLayer("DeathLayer");
        }
    }

    public void ActivateLayer(string layerName)
    {
        for (int i = 0; i < MyAnimator.layerCount; i++)
        {
            MyAnimator.SetLayerWeight(i, 0);
        }
        MyAnimator.SetLayerWeight(MyAnimator.GetLayerIndex(layerName), (1));
    }
    
    public virtual void DamageTaken(float damage, Transform source)
    {
        health.MyCurrentValue -= damage;

        CombatTextManager.MyInstance.CreateText(transform.position, damage.ToString(), SCTType.Damage, false);

        if (health.MyCurrentValue <= 0)
        {
            Direction = Vector2.zero;
            myRigidBody.velocity = Direction;
            GameManager.MyInstance.OnKillConfirmed(this);
            MyAnimator.SetTrigger("die");
        }
    }

    public void GetHealth(int health)
    {
        MyHealth.MyCurrentValue += health;
        CombatTextManager.MyInstance.CreateText(transform.position, health.ToString(), SCTType.Heal, true);
    }
}
