#region Using Statements
using UnityEngine;
using System.Collections;
using System;
#endregion


public class Slime : Boss
{
    protected override void Attack()
    {
        float distance = Vector3.Distance(transform.position, _target.position);
        if (distance < 2.0f)
        {
            // spawn small slime
            _target_controller.ApplyDamage(m_attack);
        }
    }

    protected override void FindPlayer()
    {

    }
}
