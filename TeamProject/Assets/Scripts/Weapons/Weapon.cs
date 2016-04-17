using UnityEngine;
using System.Collections;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected float coolDown = 0.2f;
    [SerializeField] protected float nextAttack = 0.0f;
    [SerializeField] protected float damage = 5.0f;

    public string weaponName = "weapon";
    public Sprite weaponSprite = null;

    protected bool m_has_attacked = false;
    protected float m_accumulated_time = 0f;

    protected new Transform transform;

    void Awake()
    {
        transform = GetComponent<Transform>();
    }

    void Update()
    {
        if (m_has_attacked)
        {
            m_accumulated_time += GameManager._Instance.MGameTime;
            if (m_accumulated_time >= coolDown)
            {
                m_has_attacked = false;
                m_accumulated_time = 0f;
            }
        }
    }
    
    public abstract void Attack(Vector3 direction, Vector3 origin);
}
