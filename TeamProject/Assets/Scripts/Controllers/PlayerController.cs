#region Using Statements
using UnityEngine;
using System.Collections;
#endregion


public class PlayerController : MonoBehaviour
{
    private new Transform transform;
    private new SpriteRenderer renderer;
    private Sprite m_sprite;

    [Header ("Character Movement")]
    [SerializeField] private float m_characterSpeed = 5f;
    [SerializeField] private float m_rollingDuration = 1f;
    [SerializeField] [Range (0, 100)] private int m_rollingSpeedModifier = 20;
    [SerializeField] private float m_rollingCooldown = 0.15f;

    private bool m_is_rolling = false;
    private float m_current_roll_duration = 0f;
    private float m_current_roll_cooldown = 0f;
    private Vector3 m_rolling_direction = Vector3.zero;

    void Awake()
    {
        transform = GetComponent<Transform>();
        renderer = GetComponent<SpriteRenderer>();

        m_sprite = renderer.sprite;
    }

    void Update()
    {
        if (GameManager._Instance.MGameState == GameState.Paused)
            return;

        // the game is running excute the code

        // grab input
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        PlayerMovement(h, v);
        PlayerRolling(h, v);
    }

    private void PlayerMovement(float h, float v)
    {
        if (m_is_rolling)
            return;

        transform.position +=   ((Vector3.right * h * m_characterSpeed) + 
                                (Vector3.forward * v * m_characterSpeed)) * 
                                GameManager._Instance.MGameTime;
    }

    private void PlayerRolling(float h, float v)
    {
        if (m_current_roll_cooldown >= 0.1f)
        {
            m_current_roll_cooldown += GameManager._Instance.MGameTime;

            if (m_current_roll_cooldown >= m_rollingCooldown)
            {
                m_current_roll_cooldown = 0f;
            }
        }
        else
        {
            if (m_is_rolling)
            {
                transform.position += m_rolling_direction * (m_characterSpeed *
                                        (1 + ((float)m_rollingSpeedModifier / 100)))
                                        * GameManager._Instance.MGameTime;

                m_current_roll_duration += GameManager._Instance.MGameTime;

                if (m_current_roll_duration >= m_rollingDuration)
                {
                    m_current_roll_cooldown += GameManager._Instance.MGameTime;
                    m_current_roll_duration = 0f;
                    m_is_rolling = false;
                }
            }
            else
            {
                if (Input.GetButton("RollActive"))
                {
                    if (h != 0 || v != 0)
                    {
                        m_rolling_direction = (Vector3.right * h) +
                                                (Vector3.forward * v);

                        m_is_rolling = true;
                        Debug.Log("Roll");
                    }
                }
            }
        }        
    }
}
