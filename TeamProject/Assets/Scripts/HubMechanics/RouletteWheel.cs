using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RouletteWheel : MonoBehaviour
{
    //private List<Weapon> WeaponList = new List<Weapon>();
    public float DistanceFromWheel = 5.0f;

    public Animator animator;

    public GameObject[] rouletteWheel;
    public Weapon currentWeapon;
    int index;

    private Transform target;
    private PlayerController target_controller;
    public AudioSource audioSource;

    private bool m_has_started = false;


    public GameObject ShowWeaponPanel = null;
    public Image ShowWeaponSprite = null;
    public Text ShowWeaponName = null;

    
	// Use this for initialization
	void Start ()
    {
        animator = this.GetComponent<Animator>();

        rouletteWheel = GameObject.FindGameObjectsWithTag("RouletteWheel");

        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        target_controller = target.gameObject.GetComponent<PlayerController>();

        ShowWeaponPanel.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (m_has_started)
            return;

        foreach (GameObject RouletteWheel in rouletteWheel)
        {
            float dist = Vector3.Distance(target.position, RouletteWheel.transform.position);

            if (dist < DistanceFromWheel)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    animator.SetBool("ActivateRouletteWheel", true);
                    StartCoroutine(Spinning(audioSource.clip.length));
                    audioSource.Play();

                    m_has_started = true;
                }
            }
        }
	
	}

    private IEnumerator Spinning(float sec)
    {
        yield return new WaitForSeconds(sec * 0.3f);

        animator.speed = 0.5f;

        yield return new WaitForSeconds(sec * 0.25f);

        animator.speed = 0.25f;

        yield return new WaitForSeconds(sec * 0.05f);

        animator.SetBool("ActivateRouletteWheel", false);

        yield return new WaitForSeconds(1.2f);
        
        ShowWeaponPanel.SetActive(true);
        ShowWeaponSprite.sprite = currentWeapon.weaponSprite;
        ShowWeaponName.text = "The " + currentWeapon.weaponName;
        GameManager._Instance.PauseGame();
    }
    public void GiveWeapon()
    {
        Weapon[] randomWeapons = GameManager._Instance.AvailableWeapons;
        index = Random.Range(0, randomWeapons.Length);
        currentWeapon = randomWeapons[index];
        target_controller.AssignWeapon(currentWeapon);
    }

    public void Continue()
    {
        ShowWeaponPanel.SetActive(false);
        GameManager._Instance.UnPauseGame();
    }

    private void AddWeaponToList ()
    {

    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, DistanceFromWheel);
    }

    public void Reset()
    {
        m_has_started = false;
    }


    

}
