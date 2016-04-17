#region Using Statements
using UnityEngine;
using System.Collections;
using System;
#endregion


public class Bow : Weapon
{
    [SerializeField] private Bullet m_arrow;

    public override void Attack(Vector3 direction, Vector3 origin)
    {
        if (m_has_attacked)
            return;

        m_arrow.CreatePool();

        if (transform == null)
            transform = GetComponent<Transform>();

        transform.position = origin;

        Bullet b = m_arrow.Spawn(transform.position, transform.rotation);
        b.Reset(origin);
        b.SetDirection(direction);
        b.SetDamage(damage);
    }
}
