using UnityEngine;
using System.Collections;

public class ButterflyFlocking : MonoBehaviour
{
    public Butterfly[] m_butterflies;
    //private Vector3[] m_randomness;
    public float m_speed = 5f;
    public float m_timeToChange = 15f;

    private Vector3 m_direction;
    private float m_elapsed_time = 0f;

    void Start()
    {
        SortNewDirection();

        //m_randomness = new Vector3[m_butterflies.Length];
    }

    void Update()
    {
        m_elapsed_time += GameManager._Instance.MGameTime;
        if (m_elapsed_time >= m_timeToChange)
        {
            m_elapsed_time = 0f;
            SortNewDirection();
        }
        else
        {
            foreach (Butterfly b in m_butterflies)
            {
                b.transform.position += (m_direction * GameManager._Instance.MGameTime * 
                                        m_speed) + (new Vector3(Random.Range(-1f, 1f), 0f,
                                        Random.Range(-1f, 1f)) * 0.05f);
            }
        }
    }

    private void SortNewDirection()
    {
        m_direction = new Vector3(  Random.Range(-1f, 1f), 0f,
                                    Random.Range(-1f, 1f));

        //for (int i = 0; i < m_randomness.Length; i++)
        //    m_randomness[i] = new Vector3( Random.Range(-1f, 1f), 0f,
        //                                    Random.Range(-1f, 1f));
    }
}
