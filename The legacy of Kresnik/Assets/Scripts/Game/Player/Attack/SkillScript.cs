using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillScript : MonoBehaviour {

    private static SkillScript instance;

    public static SkillScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SkillScript>();
            }
            return instance;
        }
    }

    public Effect myEffect;

    private Rigidbody2D myRigidBody;

    private Transform target;
    public Transform MyTarget { get; private set; }

    private Transform source;

    [SerializeField]
    private float speed;

    [SerializeField]
    private int manaCost;
    public int MyManaCost
    {
        get
        {
            return manaCost;
        }

        set
        {
            manaCost = value;
        }
    }

    private int damage;

	// Use this for initialization
	void Start ()
    {
        myRigidBody = GetComponent<Rigidbody2D>();

	}

    public void Initialize(Transform target, int damage, int manaCost, float speed, Transform source)
    {
        this.MyTarget = target;
        this.damage = damage;
        this.MyManaCost = manaCost;
        this.speed = speed;
        this.source = source;
    }

    private void FixedUpdate()
    {
        if (MyTarget != null)
        {
            //Calculates skill direction
            Vector2 direction = MyTarget.position - transform.position;

            //Move the skill 
            myRigidBody.velocity = direction.normalized * speed;

            // calculate angle
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            //rotate toward the target
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Obstacle")
        {
            speed = 0;
            GetComponent<Animator>().SetTrigger("Impact");
        }

        if (myEffect == Effect.Damage)
        {
            if (collision.tag == "Hitbox" && collision.transform == MyTarget)
            {
                Character c = collision.GetComponentInParent<Character>();
                speed = 0;

                damage += (Player.MyInstance.MyIntellect / 3);

                c.DamageTaken(damage, source);
                GetComponent<Animator>().SetTrigger("Impact");
                myRigidBody.velocity = Vector2.zero;
                MyTarget = null;
            }
        }

        if (myEffect == Effect.Dps)
        {
            if (collision.tag == "Hitbox" && collision.transform == MyTarget)
            {
                Character c = collision.GetComponentInParent<Character>();
                speed = 0;

                damage += (Player.MyInstance.MyIntellect / 3);

                c.DamageTakenDPS(damage, source);
                GetComponent<Animator>().SetTrigger("Impact");
                myRigidBody.velocity = Vector2.zero;
                MyTarget = null;
            }
        }

        if (myEffect == Effect.Stun)
        {
            if (collision.tag == "Hitbox" && collision.transform == MyTarget)
            {
                Character c = collision.GetComponentInParent<Character>();
                SkillBook sb = GetComponent<SkillBook>();
                speed = 0;

                damage += (Player.MyInstance.MyIntellect / 3);

                c.DamageTakenStun(damage, source);
                GetComponent<Animator>().SetTrigger("Impact");
                myRigidBody.velocity = Vector2.zero;
                MyTarget = null;
            }
        }

        if (myEffect == Effect.Heal)
        {
            if (collision.tag == "HealHitbox")
            {
                Player p = collision.GetComponentInParent<Player>();

                damage += (Player.MyInstance.MyIntellect / 3);

                p.DamageHealed(damage);
                GetComponent<Animator>().SetTrigger("Impact");
                myRigidBody.velocity = Vector2.zero;
                MyTarget = null;
            }
        }

        if (myEffect == Effect.Regen)
        {
            if (collision.tag == "HealHitbox")
            {
                Player p = collision.GetComponentInParent<Player>();

                damage += (Player.MyInstance.MyIntellect / 3);

                p.DamageHealedRegen(damage);
                GetComponent<Animator>().SetTrigger("Impact");
                myRigidBody.velocity = Vector2.zero;
                MyTarget = null;
            }
        }
    }
}
