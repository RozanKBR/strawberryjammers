#region Using Statements
using UnityEngine;
using System.Collections;
using System;
#endregion


public class Bow : Weapon
{
    [SerializeField] private Bullet m_arrow;

    public override void Attack(Vector3 direction)
    {
        if (transform == null)
            transform = GetComponent<Transform>();

        m_arrow.Spawn(transform.position, transform.rotation);
    }
}
