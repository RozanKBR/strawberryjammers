#region Using Statements
using UnityEngine;
using System.Collections;
#endregion


public class Bullet : MonoBehaviour 
{
    [SerializeField] private float m_shootDistance;
    [SerializeField] private float m_shootSpeed;

    public Vector3 direction;
    private float m_damage = 0f;
    private new Transform transform;
    private string m_tag_to_kill;
    //private Types m_type_to_kill;

    void Awake()
    {
        if (transform == null)
            transform = GetComponent<Transform>();
    }

    void OnEnable()
    {
        StartCoroutine(Shoot());
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    //public void StartShoot(Vector3 direction)
    //{
    //    StartCoroutine(Shoot(direction));
    //}

    public IEnumerator Shoot()
    {
        direction.Normalize();
        float travelledDistance = 0;
        
        while (travelledDistance < m_shootDistance)
        {
            travelledDistance += m_shootSpeed * GameManager._Instance.MGameTime;
            transform.position += direction * (m_shootSpeed * GameManager._Instance.MGameTime);

            // check for collision
            RaycastHit hit;
            Ray ray = new Ray(transform.position, direction);
            if (Physics.Raycast(ray, out hit, 2f))
            {
                // collision
                Enemy e = hit.collider.gameObject.GetComponent<Enemy>();
                if (e)
                {
                    //Debug.Log(m_damage);
                    e.ApplyDamage(m_damage);
                    this.Recycle();
                }
            }

            yield return 0;
        }

        //explosionPrefab.Spawn(transform.position);
        this.Recycle();
    }

    public void DestroyIt()
    {
        StopAllCoroutines();
        this.Recycle();
    }

    public void SetDamage(float damage)
    {
        m_damage = damage;
    }

    public void Reset(Vector3 pos)
    {
        if (transform == null)
            transform = GetComponent<Transform>();

        transform.position = pos;

        direction = Vector3.zero;
        m_damage = 0f;
    }

    public void SetDirection (Vector3 Ndirection)
    {
        direction = Ndirection;

        if (transform == null)
            transform = GetComponent<Transform>();

        if (direction.x > 0.1f)
        {
            if (direction.z > 0.1f)
            {
                transform.Rotate(new Vector3(0f, 0f, 45f));
            }
            else if (direction.z < -0.1f)
            {
                transform.Rotate(new Vector3(0f, 0f, -45f));
            }
            else
            {
                transform.Rotate(new Vector3(0f, 0f, 0f));
            }
        }
        else if (direction.x < -0.1f)
        {
            if (direction.z > 0.1f)
            {
                transform.Rotate(new Vector3(0f, 0f, -45 + 180f));
            }
            else if (direction.z < -0.1f)
            {
                transform.Rotate(new Vector3(0f, 0f, 45 + 180f));
            }
            else
            {
                transform.Rotate(new Vector3(0f, 0f, 180f));
            }
        }
        else
        {
            if (direction.z > 0.1f)
            {
                transform.Rotate(new Vector3(0f, 0f, 90f));
            }
            else if (direction.z < 0.1f)
            {
                transform.Rotate(new Vector3(0f, 0f, -90f));
            }
        }
    }
}
