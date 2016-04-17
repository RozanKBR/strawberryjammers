#region Using Statements
using UnityEngine;
using System.Collections;
#endregion


public abstract class Enemy : MonoBehaviour
{
    [Header ("Enemy Varaibles")]
    [SerializeField] protected float m_maxHealth = 100f;
    [SerializeField] protected float m_defense = 2.0f;
    [SerializeField] protected float m_attack = 2.0f;

    protected static Transform _target = null;
    protected new Transform transform = null;
    protected Animator animator = null;

    protected float m_current_health = 0f;
    protected bool m_is_dead = false;

    public bool IsActive { get; set; }

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
