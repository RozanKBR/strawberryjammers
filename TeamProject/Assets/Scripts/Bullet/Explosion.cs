using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour
{
    public new SphereCollider collider;
    public float explosionRadius = 7.0f;
    public float explosionDuration = 2.0f;
    private float timeLapse = 0.0f;
    public float explosionDamage = 10.0f;
    public float explosionFactor = 2.0f;
    private new Transform transform;

    // Use this for initialization
    void Awake()
    {
        transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        collider.radius += explosionRadius * GameManager._Instance.MGameTime;
        timeLapse += GameManager._Instance.MGameTime;
        transform.localScale += Vector3.one * GameManager._Instance.MGameTime * explosionFactor;

        if (timeLapse >= explosionDuration)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider co)
    {
        Enemy e = co.GetComponent<Enemy>();
        if (e)
        {
            e.ApplyDamage(explosionDamage);

        }
        else
        {
            PlayerController pc = co.GetComponent<PlayerController>();
            if (pc)
            {
                pc.ApplyDamage(explosionDamage);
            }
        }
    }
}
