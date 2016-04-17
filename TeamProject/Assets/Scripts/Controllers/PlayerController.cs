#region Using Statements
using UnityEngine;
using System.Collections;
#endregion


public class PlayerController : MonoBehaviour
{
    private new Transform transform = null;
    //private new SpriteRenderer renderer = null;
    //private Sprite m_sprite = null;

    [Header("Character Movement")]
    [SerializeField] private float m_buttonPressureOffset = 0.2f;
    [SerializeField] private float m_characterSpeed = 5f;
    [SerializeField] private float m_rollingDuration = 1f;
    [SerializeField] [Range(0, 100)] private int m_rollingSpeedModifier = 20;
    [SerializeField] private float m_rollingCooldown = 0.15f;
    //[SerializeField] private GameObject m_weaponSlot = null;

    private bool m_is_rolling = false;
    private bool m_is_movement_button_pressed = false;
    private float m_current_roll_duration = 0f;
    private float m_current_roll_cooldown = 0f;
    private Vector3 m_rolling_direction = Vector3.zero;
    private Vector3 m_current_direction = Vector3.up;

    private PlayerStats m_stats = new PlayerStats();
    private Weapon m_weapon = null;

    void Awake()
    {
        transform = GetComponent<Transform>();
        //renderer = GetComponent<SpriteRenderer>();

        //m_sprite = renderer.sprite;

        //temp
        //if (m_weaponSlot == null)
        //    Debug.LogError("No Weapon Assigned!");

        //m_weapon = m_weaponSlot.GetComponent<Weapon>();
        //m_weapon = GameManager._Instance.AvailableWeapons[0];
    }

    void Start()
    {
        SetupPlayerStats();
    }

    void Update()
    {
        if (GameManager._Instance.MGameState == GameState.Paused)
            return;

        // grab input
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        m_is_movement_button_pressed =  Mathf.Abs(h) >= m_buttonPressureOffset ||
                                        Mathf.Abs(v) >= m_buttonPressureOffset;

        if (m_is_movement_button_pressed)
            m_current_direction = ((Vector3.right * h) + (Vector3.forward * v));

        PlayerMovement(h, v);
        PlayerRolling(h, v);
        PlayerAttack();

        //Debug.Log("Direction: " + m_current_direction);
    }

    private void PlayerMovement(float h, float v)
    {
        if (m_is_rolling)
            return;

        transform.position +=   ((Vector3.right * h) + (Vector3.forward * v)) * 
                                m_characterSpeed * GameManager._Instance.MGameTime;
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
                if (m_is_movement_button_pressed)
                {
                    if (Input.GetButton("RollActive"))
                    {
                        m_rolling_direction =   (Vector3.right * h) +
                                                (Vector3.forward * v);

                        m_is_rolling = true;
                        //Debug.Log("Is Rolling");
                    }
                }
            }
        }
    }

    private void PlayerAttack()
    {
        if (m_is_rolling || m_weapon == null)
            return;

        if (Input.GetButtonDown("Fire1"))
        {
            m_weapon.Attack(m_current_direction, transform.position);
        }
    }


    private void SetupPlayerStats ()
    {
        m_stats.Attack = 2.0f;
        m_stats.Defense = 2.0f;

        m_stats.MaxHealth = 100f;
        m_stats.CurrentHealth = m_stats.MaxHealth;
    }

    public void AssignWeapon (Weapon w)
    {
        m_weapon = w;
    }
}
