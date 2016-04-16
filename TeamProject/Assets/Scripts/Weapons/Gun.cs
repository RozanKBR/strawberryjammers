using UnityEngine;
using System.Collections;
using System;

public class Gun : Weapon
{
    public override void Attack(Vector3 direction, Vector3 origin)
    {
        throw new NotImplementedException();
    }


    // Use this for initialization


    // Update is called once per frame
    void Update ()
    {
        //if (Input.GetButton("Fire1") && Time.time > nextAttack)
        //{
        //    attackMove(Vector3.forward);
        //}
	}
}
