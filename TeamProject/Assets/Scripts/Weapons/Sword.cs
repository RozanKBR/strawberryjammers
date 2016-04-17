#region Using Statements
using UnityEngine;
using System.Collections;
using System;
#endregion


public class Sword : Weapon
{
    [SerializeField] private Bullet m_slash;

    public override void Attack(Vector3 direction, Vector3 origin)
    {
        if (m_has_attacked)
            return;

        m_slash.CreatePool();

        if (transform == null)
            transform = GetComponent<Transform>();

        transform.position = origin;

        Bullet b = m_slash.Spawn(transform.position, transform.rotation);
        b.Reset(origin);
        b.SetDirection(direction);
        b.SetDamage(damage);
    }
}
