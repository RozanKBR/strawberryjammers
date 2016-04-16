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
        m_arrow.CreatePool();

        if (transform == null)
            transform = GetComponent<Transform>();

        transform.position = origin;

        m_arrow.Reset(origin);
        m_arrow.direction = direction;
        m_arrow.Spawn(transform.position, transform.rotation);
    }
}
