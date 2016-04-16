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

    IEnumerator Shoot()
    {
        direction.Normalize();
        float travelledDistance = 0;
        while (travelledDistance < m_shootDistance)
        {
            travelledDistance += m_shootSpeed * Time.deltaTime;
            transform.position += direction * (m_shootSpeed * Time.deltaTime);

            // check for collision
            RaycastHit hit;
            Ray ray = new Ray(transform.position, direction);
            if (Physics.Raycast(ray, out hit, 5f))
            {
                // collision

            }

            yield return 0;
        }

        //explosionPrefab.Spawn(transform.position);
        this.Recycle();
    }

    public void SetDamage(float damage)
    {
        m_damage = damage;
    }

    public void Reset(Vector3 pos)
    {
        transform.position = pos;

        direction = Vector2.zero;
        m_damage = 0f;

        if (transform == null)
            transform = GetComponent<Transform>();
    }
}
