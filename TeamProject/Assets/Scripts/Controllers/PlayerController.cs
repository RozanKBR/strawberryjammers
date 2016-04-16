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

    private bool m_is_rolling = false;
    private float m_current_roll_duration = 0f;

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
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        transform.position +=   (Vector3.right * h * m_characterSpeed) + 
                                (Vector3.forward * v * m_characterSpeed);
    }

    private void PlayerRolling()
    {
        if (m_is_rolling)
        {

        }
        else
        {

        }
    }
}
