using UnityEngine;
using System.Collections;

public abstract class Weapon : MonoBehaviour
{
    protected float coolDown = 0.2f;
    protected float nextAttack = 0.0f;

    protected float damage = 5.0f;
    //protected Collider2D b_Collider;

    protected new Transform transform;
    protected GameObject attackShot;
    // Use this for initialization

    void Awake()
    {
        transform = GetComponent<Transform>();
    }

    //public void attackMove(Vector3 direction)
    //{              
    //        nextAttack = Time.time + coolDown;
    //        GameObject clone = Instantiate(attackShot, transform.position, transform.rotation) as GameObject;      
    //}
    
    public abstract void Attack(Vector3 direction, Vector3 origin);
}
