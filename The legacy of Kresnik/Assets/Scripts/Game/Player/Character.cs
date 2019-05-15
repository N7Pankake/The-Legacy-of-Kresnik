using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    protected bool isUsingHeal = false;

    public bool IsAttacking { get; set; }

    protected Coroutine actionRoutine;

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

    private bool audioIsPlaying = false;

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

                MyAnimator.SetFloat("x", direction.x);
                MyAnimator.SetFloat("y", direction.y);
            }
            else if (isUsingSkill)
            {
                ActivateLayer("SkillLayer");
            }
            else if (isUsingHeal)
            {
                ActivateLayer("HealLayer");
            }
            else if (IsAttacking)
            {
                ActivateLayer("AttackLayer");
            }
            else
            {
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
            if (gameObject.tag == "Player" && !audioIsPlaying)
            {
                audioIsPlaying = true;
                AudioSource audio = GetComponent<AudioSource>();
                audio.PlayOneShot(Player.MyInstance.MyGameOver);
            }

            Direction = Vector2.zero;
            myRigidBody.velocity = Direction;
            GameManager.MyInstance.OnKillConfirmed(this);
            MyAnimator.SetTrigger("die");
        }
    }

    public virtual void DamageTakenDPS(float damage, Transform source)
    {
        float myDamage = damage;
        Transform mySource = source;

        health.MyCurrentValue -= damage;
        CombatTextManager.MyInstance.CreateText(transform.position, damage.ToString(), SCTType.Damage, false);

        if (health.MyCurrentValue <= 0)
        {
            Direction = Vector2.zero;
            myRigidBody.velocity = Direction;
            GameManager.MyInstance.OnKillConfirmed(this);
            MyAnimator.SetTrigger("die");
        }
        StartCoroutine(DPS(myDamage, mySource));
    }

    public IEnumerator DPS(float damage, Transform source)
    {
        Enemy enemy = GetComponentInParent<Enemy>();
        yield return new WaitForSeconds(1);

        if(IsAlive)
        {
            for (int i = 0; i < (damage-1); i++)
            {

            health.MyCurrentValue -= damage;

                if (IsAlive)
                {
                    CombatTextManager.MyInstance.CreateText(transform.position, damage.ToString(), SCTType.Damage, false);
                    enemy.OnHealthChanged(health.MyCurrentValue);
                    yield return new WaitForSeconds(1);
                }

                else
                {
                    enemy.OnHealthChanged(health.MyCurrentValue);
                    Direction = Vector2.zero;
                    myRigidBody.velocity = Direction;
                    GameManager.MyInstance.OnKillConfirmed(this);
                    MyAnimator.SetTrigger("die");
                }
            }
        }
        
    }

    public virtual void DamageTakenStun(float damage, Transform source)
    {
        float myDamage = damage;
        Transform mySource = source;

        health.MyCurrentValue -= damage;
        CombatTextManager.MyInstance.CreateText(transform.position, damage.ToString(), SCTType.Damage, false);

        if (health.MyCurrentValue <= 0)
        {
            Direction = Vector2.zero;
            myRigidBody.velocity = Direction;
            GameManager.MyInstance.OnKillConfirmed(this);
            MyAnimator.SetTrigger("die");
        }

        StartCoroutine(Stun(myDamage, mySource));
    }

    public IEnumerator Stun(float damage, Transform source)
    {
        Enemy enemy = GetComponentInParent<Enemy>();
        float speed = enemy.MySpeed;
        enemy.MySpeed = 0;
        yield return new WaitForSeconds(damage);
        enemy.MySpeed = speed;
    }

    public void GetHealth(int health)
    {
        MyHealth.MyCurrentValue += health;
        CombatTextManager.MyInstance.CreateText(transform.position, health.ToString(), SCTType.Heal, true);
    }
}