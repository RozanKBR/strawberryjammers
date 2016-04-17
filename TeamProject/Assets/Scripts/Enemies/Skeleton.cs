using UnityEngine;
using System.Collections;
using System;

public class Skeleton : Enemy
{
    // Weapon Type
    public Weapon bow;

    protected override void Attack()
    {
        bow.Attack((destPos - transform.position).normalized, transform.position);
    }

    protected override void FindPlayer()
    {
        
    }

    // Use this for initialization
    void Start()
    {
        BaseStart();
    }
}
