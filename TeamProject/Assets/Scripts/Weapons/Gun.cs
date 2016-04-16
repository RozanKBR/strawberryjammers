using UnityEngine;
using System.Collections;

public class Gun : Weapon
{
   

	// Use this for initialization
	
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButton("Fire1") && Time.time > nextAttack)
        {
            attackMove(Vector3.forward);
        }
	}
}
