using UnityEngine;
using System.Collections;
using System;

public class Gun : Weapon
{
    [SerializeField] private Bullet m_bullet;

    public override void Attack(Vector3 direction, Vector3 origin)
    {
        m_bullet.CreatePool();

        if (transform == null)
            transform = GetComponent<Transform>();

        transform.position = origin;

        Bullet b = m_bullet.Spawn(transform.position, transform.rotation);
        b.Reset(origin);
        b.direction = direction;
        b.SetDamage(damage);
    }
}
