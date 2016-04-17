using UnityEngine;
using System.Collections;
using System;

public class RocketLauncher : Weapon
{
    [SerializeField]private Bomb m_rocket;


    public override void Attack(Vector3 direction, Vector3 origin, Target t)
    {
        if (m_has_attacked)
            return;

        m_rocket.CreatePool();

        if (transform == null)
            transform = GetComponent<Transform>();

        transform.position = origin;

        Bomb b = m_rocket.Spawn(transform.position, transform.rotation);
        b.Reset(origin);
        b.SetDirection(direction);
        b.SetDamage(damage);
        b.SetTarget(t);
    }

}
