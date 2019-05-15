using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void HealthChanged(float health);

public delegate void CharacterRemoved();

public class Enemy : Character, IInteractable
{
    private static Enemy instance;

    public static Enemy MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Enemy>();
            }
            return instance;
        }
    }

    public event HealthChanged healthChanged;

    public event CharacterRemoved characterRemoved;

    [SerializeField]
    public int goldValue;

    [SerializeField]
    private GameObject attack;

    [SerializeField]
    private SpriteRenderer enemySprite;

    [SerializeField]
    private CircleCollider2D enemyCC2D;

    [SerializeField]
    private Sprite portrait;

    public Sprite MyPortrait
    {
        get
        {
            return portrait;
        }
    }

    [SerializeField]
    private CanvasGroup healthGroup;

    [SerializeField]
    private float initAggroRange;

    [SerializeField]
    private LootTable lootTable;

    private IState currentState;
    
    public float MyAttackRange { get; set;}

    public float MyAttackTime { get; set;}

    public float MyAggroRange { get; set;}

    public Vector3 MyStartPosition { get; set; }

    public bool InRange
    {
        get
        {
            return Vector2.Distance(transform.position, MyTarget.position) < MyAggroRange;
        }
    }

    protected void Awake()
    {
        health.Initialize(initHealth, initHealth);
        MyStartPosition = transform.position;
        MyAggroRange = initAggroRange;
        MyAttackRange = 1f;
        ChangeState(new IdleState());
    }

    protected override void Update()
    {
        if(IsAlive)
        {
            if (!IsAttacking)
            {
                MyAttackTime += Time.deltaTime;
            }
            currentState.Update();
        }
        base.Update();
    }

    public Transform Select()
    {
        healthGroup.alpha = 1;
        return hitBox;
    }

    public void DeSelect()
    {
        healthGroup.alpha = 0;
        healthChanged -= new HealthChanged(UIManager.MyInstance.UpdateTargetFrame);
        characterRemoved -= new CharacterRemoved(UIManager.MyInstance.HideTargetFrame);
    }

    public override void DamageTaken(float damage, Transform source)
    {
        if (!(currentState is EvadeState))
        {
            if (IsAlive)
            {
                SetTarget(source);

                base.DamageTaken(damage, source);

                OnHealthChanged(health.MyCurrentValue);

                if (gameObject.tag == "Boss")
                {
                    BossVoice.MyInstance.BossAttackedVoice();
                }

                else if(gameObject.tag == "Enemy")
                {
                    EnemyVoice.MyInstance.EnemyAttacked();
                }

                if (!IsAlive)
                {
                    if (gameObject.tag == "Enemy")
                    {
                        EnemyVoice.MyInstance.EnemyDeath();
                    }

                    if (gameObject.tag == "Boss")
                    {
                        BossVoice.MyInstance.BossDeath();
                    }

                    enemySprite.sortingOrder = -1;
                    enemyCC2D.isTrigger = true;
                    Player.MyInstance.MyAttackers.Remove(this);
                    Player.MyInstance.GainXp(XPManager.CalculateXP(this as Enemy));
                    Player.MyInstance.StartCoroutine(Player.MyInstance.GainGold(goldValue));
                }
            }
        }
    }

    public override void DamageTakenDPS(float damage, Transform source)
    {
        if (!(currentState is EvadeState))
        {
            if (IsAlive)
            {
                SetTarget(source);

                base.DamageTakenDPS(damage, source);

                OnHealthChanged(health.MyCurrentValue);

                if (gameObject.tag == "Boss")
                {
                    BossVoice.MyInstance.BossAttackedVoice();
                }

                else if (gameObject.tag == "Enemy")
                {
                    EnemyVoice.MyInstance.EnemyAttacked();
                }

                if (!IsAlive)
                {
                    if (gameObject.tag == "Enemy")
                    {
                        EnemyVoice.MyInstance.EnemyDeath();
                    }

                    if (gameObject.tag == "Boss")
                    {
                        BossVoice.MyInstance.BossDeath();
                    }

                    enemySprite.sortingOrder = -1;
                    enemyCC2D.isTrigger = true;
                    Player.MyInstance.MyAttackers.Remove(this);
                    Player.MyInstance.GainXp(XPManager.CalculateXP(this as Enemy));
                    Player.MyInstance.StartCoroutine(Player.MyInstance.GainGold(goldValue));
                }
            }
        }
    }

    public override void DamageTakenStun(float damage, Transform source)
    {
        if (!(currentState is EvadeState))
        {
            if (IsAlive)
            {
                SetTarget(source);

                base.DamageTakenStun(damage, source);

                OnHealthChanged(health.MyCurrentValue);

                if (gameObject.tag == "Boss")
                {
                    BossVoice.MyInstance.BossAttackedVoice();
                }

                else if (gameObject.tag == "Enemy")
                {
                    EnemyVoice.MyInstance.EnemyAttacked();
                }

                if (!IsAlive)
                {
                    if (gameObject.tag == "Enemy")
                    {
                        EnemyVoice.MyInstance.EnemyDeath();
                    }

                    if (gameObject.tag == "Boss")
                    {
                        BossVoice.MyInstance.BossDeath();
                    }

                    enemySprite.sortingOrder = -1;
                    enemyCC2D.isTrigger = true;
                    Player.MyInstance.MyAttackers.Remove(this);
                    Player.MyInstance.GainXp(XPManager.CalculateXP(this as Enemy));
                    Player.MyInstance.StartCoroutine(Player.MyInstance.GainGold(goldValue));
                }
            }
        }
    }

    public void ChangeState(IState newState)
    {
        if(currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter(this);
    }

    public void SetTarget(Transform target)
    {
        if(MyTarget == null && !(currentState is EvadeState))
        {
            float distance = Vector2.Distance(transform.position, target.position);
            MyAggroRange = initAggroRange;
            MyAggroRange += distance;
            MyTarget = target;
        }
    }

    public void Reset()
    {
        this.MyTarget = null;
        this.MyAggroRange = initAggroRange;
        this.MyHealth.MyCurrentValue = this.MyHealth.MyMaxValue;
        OnHealthChanged(health.MyCurrentValue);
    }

    public void Interact()
    {
        if (!IsAlive)
        {
            List<Drop> drops = new List<Drop>();
            
            foreach (IInteractable interactable in Player.MyInstance.MyInteractables)
            {
                if(interactable is Enemy && !(interactable as Enemy).IsAlive)
                {
                    drops.AddRange((interactable as Enemy).lootTable.GetLoot());
                }
            }

            if (drops.Count == 0)
            {
                Destroy(gameObject);
            }

            LootWindow.MyInstance.CreatePages(drops);
        }
    }

    public void StopInteract()
    {
        LootWindow.MyInstance.Close();
    }

    public void OnHealthChanged(float health)
    {
        if (healthChanged != null)
        {
            healthChanged(health);
        }
    }

    public void OnCharacterRemoved()
    {
        if (characterRemoved != null)
        {
            characterRemoved();
        }

        Destroy(gameObject);
    }

    public void AttackOn()
    {
        if (gameObject.tag == "Enemy" && IsAlive)
        {
            EnemyVoice.MyInstance.EnemyAttack();
        }

        if (gameObject.tag == "Boss" && IsAlive)
        {
            BossVoice.MyInstance.BossAttack();
        }
        attack.SetActive(true);
    }

    public void AttackOff()
    {
        attack.SetActive(false);
    }
}
