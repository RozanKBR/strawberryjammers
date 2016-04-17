using UnityEngine;
using System.Collections;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected float coolDown = 0.2f;
    [SerializeField] protected float nextAttack = 0.0f;
    [SerializeField] protected float damage = 5.0f;

    public string weaponName = "weapon";
    public Sprite weaponSprite = null;



    protected new Transform transform;

    void Awake()
    {
        transform = GetComponent<Transform>();
    }
    
    public abstract void Attack(Vector3 direction, Vector3 origin);
}
