#region Using Statements
using UnityEngine;
using System.Collections;
#endregion


public abstract class Enemy : MonoBehaviour
{
    [Header ("Enemy Varaibles")]
    [SerializeField] protected float m_maxHealth = 100.0f;
    [SerializeField] protected float m_defense = 2.0f;
    [SerializeField] protected float m_attack = 2.0f;
    [SerializeField] protected float m_moveSpeed = 5.0f;

    public float EnemyAttack { get { return m_attack; } }

    public float EnemyDefece { get { return m_attack; } }

    protected static Transform _target = null;
    protected static PlayerController _target_controller = null;
    protected new Transform transform = null;
    protected Animator animator = null;

    protected float m_current_health = 0f;
    protected bool m_is_dead = false;

    public bool IsActive { get; set; }

    //next destination for the npc
    protected Vector3 destPos;

    //List of points for patrolling
    protected GameObject[] pointList;

    //Shooting Rate
    protected float shootRate;
    protected float elapsedTime;

    //Weapon
    public Transform weapon { get; set; }
    public Transform ProjectileSpawnPoint { get; set; }

    protected virtual void Initialize()
    {
        curState = EnemyState.Patrol;
        curSpeed = m_moveSpeed;
        curRotSpeed = 2.0f;
        bDead = false;
        elapsedTime = 0.0f;
        shootRate = 3.0f;
        health = m_maxHealth;

        //get the list of waypoints
        pointList = GameObject.FindGameObjectsWithTag("WayPoint");

        //set random destination point first
        FindNextPoint();

        //get the target enemy (Player)
        GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
        _target = objPlayer.transform;

        _target_controller = objPlayer.GetComponent<PlayerController>();

        if (!_target)
            print("Player non existant" + "Add one with tag 'Player'");
        //get the weapon of the enemy
        weapon = gameObject.transform.GetChild(0).transform;
        ProjectileSpawnPoint = weapon.GetChild(0).transform;
    }
    protected virtual void EnemyUpdate()
    {
        switch (curState)
        {
            case EnemyState.Patrol: UpdatePatrolState(); break;
            case EnemyState.Chase:  UpdateChaseState(); break;
            case EnemyState.Attack: UpdateAttackState(); break;
            case EnemyState.Dead:   UpdateDeadState(); break;
        }

        //update the time
        elapsedTime += Time.deltaTime;
        //Go to dead state if no health left
        if (health <= 0)
            curState = EnemyState.Dead;
    }

    protected void UpdatePatrolState()
    {
        //find anaother random patrol point if the current pont is reached
        if (Vector3.Distance(transform.position, destPos) <= 100.0f)
        {
            FindNextPoint();
        }

        //check the Distance with the player character
        //When the Distance is near, change to chase state
        else if (Vector3.Distance(transform.position, _target.position) <= 300.0f)
        {
            curState = EnemyState.Chase;
        }

        //Rotate to the Target Point
        Quaternion targetRotation = Quaternion.LookRotation(destPos - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * curRotSpeed);
        //Go Forward
        transform.Translate(Vector3.forward * Time.deltaTime * curSpeed);
    }

    protected void FindNextPoint()
    {
        print("Finding next point");
        int rndIndex = Random.Range(0, pointList.Length);
        float rndRadius = 10.0f;
        Vector3 rndPosition = Vector3.zero;
        destPos = pointList[rndIndex].transform.position +
        rndPosition;
        //Check Range to decide the random point 
        //as the same as before
        if (IsInCurrentRange(destPos))
        {
            rndPosition = new Vector3(Random.Range(-rndRadius,
            rndRadius), 0.0f, Random.Range(-rndRadius,
            rndRadius));
            destPos = pointList[rndIndex].transform.position +
            rndPosition;
        }
    }

    protected bool IsInCurrentRange(Vector3 pos)
    {
        float xPos = Mathf.Abs(pos.x - transform.position.x);
        float zPos = Mathf.Abs(pos.z - transform.position.z);
        if (xPos <= 50 && zPos <= 50)
            return true;
        return false;
    }

    protected void UpdateChaseState()
    {
        //Set the target position as the player position
        destPos = _target.position;

        //Check the distance with player when the distance is near, transition to attack state
        float dist = Vector3.Distance(transform.position, _target.position);
        if (dist <= 200.0f)
        {
            curState = EnemyState.Attack;
        }
        //Go back to patrol if it become too far
        else if (dist >= 300.0f)
        {
            curState = EnemyState.Patrol;
        }
        //go forward
        transform.Translate(Vector3.forward * Time.deltaTime * curSpeed);
    }

    protected void UpdateAttackState()
    {
        //Set the target posisition as the player position
        destPos = _target.position;
        //check the distance with the player
        float dist = Vector3.Distance(transform.position, _target.position);
        if (dist >= 200.0f && dist < 300.0f)
        {
            //rotate to target point
            Quaternion targetRotation = Quaternion.LookRotation(destPos - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * curRotSpeed);
            // go forward
            transform.Translate(Vector3.forward * Time.deltaTime * curSpeed);
            curState = EnemyState.Attack;
        }
        else if (dist >= 300.0f)
        {
            curState = EnemyState.Patrol;
        }

        //Always Turn the turret towards the player
        Quaternion turretRotation = Quaternion.LookRotation(destPos - weapon.position);
        weapon.rotation = Quaternion.Slerp(weapon.rotation, turretRotation, Time.deltaTime * curRotSpeed);
        //shoot the projectiles
        ShootProjectile();
    }
    private void ShootProjectile()
    {
        if (elapsedTime >= shootRate)
        {
            //Shoot
            Attack();
            elapsedTime = 0.0f;
        }
    }

    protected void UpdateDeadState()
    {
        if (!bDead)
        {
            bDead = true;
        }
    }

    protected virtual void EnemyFixedUpdate()
    {
    }

    public enum EnemyState
    {
        None,
        Patrol,
        Chase,
        Attack,
        Dead,
    }

    //Current State of the Enemy
    public EnemyState curState;

    //Speed of the Enemy
    private float curSpeed;

    //Enemy Rotation Speed
    private float curRotSpeed;

    //Whether the NPC is destroyed or not
    protected bool bDead;
    protected float health;

    protected void BaseStart()
    {
        Initialize();
    }

    void Update()
    {
        EnemyUpdate();
    }

    void FixedUpdate()
    {
        EnemyFixedUpdate();
    }
    void Awake()
    {
        transform = GetComponent<Transform>();
        animator = GetComponent<Animator>();

        m_current_health = m_maxHealth;

        if (_target == null)
            _target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    public void ApplyDamage(float damage)
    {
        damage = Mathf.Clamp((damage - (m_defense * Mathf.Log10(m_defense))), 0.0f, damage);

        m_current_health -= damage;
        CheckForHealth();
    }

    private void CheckForHealth()
    {
        if (m_current_health <= 0.0f)
        {
            m_is_dead = true;
            Destroy(this.gameObject);
        }
    }


    #region Abstract Functions
    protected abstract void Attack();
    protected abstract void FindPlayer();
    #endregion
}
