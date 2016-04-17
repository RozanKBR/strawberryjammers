#region Using Statements
using UnityEngine;
using System.Collections.Generic;
#endregion


public class PlayerController : MonoBehaviour
{
    private const float COOLDOWN_DAMAGE = 0.2f;

    private new Transform transform = null;
    //private new SpriteRenderer renderer = null;
    //private Sprite m_sprite = null;

    [Header("Character Varaibles")]
    [SerializeField] private float m_buttonPressureOffset = 0.2f;
    [SerializeField] private float m_characterSpeed = 5f;
    [SerializeField] private float m_rollingDuration = 1f;
    [SerializeField] [Range(0, 100)] private int m_rollingSpeedModifier = 20;
    [SerializeField] private float m_rollingCooldown = 0.15f;
    [SerializeField] private float m_startHealth = 100f;
    [SerializeField] private float m_startAttack = 2f;
    [SerializeField] private float m_startDefense = 2f;
    [SerializeField] private GameObject m_gameOverScreen = null;
    [SerializeField] private GameObject m_weaponSlot = null;
    [SerializeField] private Transform m_spawnPoint = null;

    private bool m_is_rolling = false;
    private bool m_is_movement_button_pressed = false;
    private bool m_is_dead = false;
    private float m_current_roll_duration = 0f;
    private float m_current_roll_cooldown = 0f;
    private float m_current_damage_cooldown = 0f;
    private Vector3 m_rolling_direction = Vector3.zero;
    private Vector3 m_current_direction = Vector3.up;

    private PlayerStats m_stats = new PlayerStats();
    private Weapon m_weapon = null;
    private Transform m_weapon_slot_transform = null;
    private SpriteRenderer m_weapon_slot_sprite_renderer = null;

    void Awake()
    {
        transform = GetComponent<Transform>();

        if (m_weaponSlot)
        {
            m_weapon_slot_transform = m_weaponSlot.GetComponent<Transform>();
            m_weapon_slot_sprite_renderer = m_weaponSlot.GetComponent<SpriteRenderer>();
        }
    }

    void Start()
    {
        SetupPlayerStats();

        m_gameOverScreen.SetActive(false);

        if (m_spawnPoint == null)
            m_spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").GetComponent<Transform>();

    }

    void Update()
    {
        if (GameManager._Instance.MGameState == GameState.Paused ||
            m_is_dead)
            return;

        // grab input
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        m_is_movement_button_pressed =  Mathf.Abs(h) >= m_buttonPressureOffset ||
                                        Mathf.Abs(v) >= m_buttonPressureOffset;

        if (m_is_movement_button_pressed)
        {
            m_current_direction = ((Vector3.right * h) + (Vector3.forward * v));

            // getting the right direction
            DirectionToOne(ref m_current_direction, m_buttonPressureOffset);
            AdjustWeaponRotation();
        }

        PlayerMovement(h, v);
        PlayerRolling(h, v);
        PlayerAttack();
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

    private void DirectionToOne (ref Vector3 direction, float min_value)
    {
        FloatToOne(ref direction.x, min_value);
        FloatToOne(ref direction.y, min_value);
        FloatToOne(ref direction.z, min_value);
    }

    private void FloatToOne(ref float value, float min_value)
    {
        if (value > min_value)
            value = 1f;
        else if (value < -min_value)
            value = -1;
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
        m_stats.Attack = m_startAttack;
        m_stats.Defense = m_startDefense;

        m_stats.MaxHealth = m_startHealth;
        m_stats.CurrentHealth = m_stats.MaxHealth;
    }

    public void AssignWeapon (Weapon w)
    {
        m_weapon = w;
        m_weapon_slot_sprite_renderer.sprite = w.weaponSprite;
    }

    public void ApplyDamage(float damage)
    {
        damage = Mathf.Clamp(   (damage - (m_stats.Defense * Mathf.Log10(m_stats.Defense))), 
                                0.0f, damage);

        m_stats.CurrentHealth -= damage;
        CheckForHealth();
    }

    private void CheckForHealth()
    {
        if (m_stats.CurrentHealth <= 0f)
        {
            m_is_dead = true;
            m_gameOverScreen.SetActive(true);

            GameManager._Instance.PauseGame();
        }
    }

    private void AdjustWeaponRotation()
    {
        if (m_current_direction.x > 0.1f)
        {
            if (m_current_direction.z > 0.1f)
            {
                m_weapon_slot_transform.rotation = 
                    Quaternion.Euler(new Vector3(90f, 0f, 45f));
            }
            else if (m_current_direction.z < -0.1f)
            {
                m_weapon_slot_transform.rotation =
                   Quaternion.Euler(new Vector3(90f, 0f, -45f));
            }
            else
            {
                m_weapon_slot_transform.rotation =
                   Quaternion.Euler(new Vector3(90f, 0f, 0f));
            }
        }
        else if (m_current_direction.x < -0.1f)
        {
            if (m_current_direction.z > 0.1f)
            {
                m_weapon_slot_transform.rotation =
                   Quaternion.Euler(new Vector3(90f, 0f, -45 + 180f));
            }
            else if (m_current_direction.z < -0.1f)
            {
                m_weapon_slot_transform.rotation =
                   Quaternion.Euler(new Vector3(90f, 0f, 45 + 180f));
            }
            else
            {
                m_weapon_slot_transform.rotation =
                   Quaternion.Euler(new Vector3(90f, 0f, 180f));
            }
        }
        else
        {
            if (m_current_direction.z > 0.1f)
            {
                m_weapon_slot_transform.rotation =
                   Quaternion.Euler(new Vector3(90f, 0f, 90f));
            }
            else if (m_current_direction.z < 0.1f)
            {
                m_weapon_slot_transform.rotation =
                   Quaternion.Euler(new Vector3(90f, 0f, -90f));
            }
        }
    }

    public void ResetPlayerGame()
    {
        SetupPlayerStats();
        m_gameOverScreen.SetActive(false);

        m_weapon = null;
        m_is_dead = false;
        m_is_rolling = false;
        transform.position = m_spawnPoint.position;

        GameManager._Instance.ResetGame();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            ApplyDamage(2f);//col.gameObject.GetComponent<Enemy>().EnemyAttack);
        }
    }

    void OnCollisionStay(Collision col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            m_current_damage_cooldown += GameManager._Instance.MGameTime;
            if (m_current_damage_cooldown >= COOLDOWN_DAMAGE)
            {
                ApplyDamage(2f);//col.gameObject.GetComponent<Enemy>().EnemyAttack);
                m_current_damage_cooldown = 0f;
            }
        }
    }
}
