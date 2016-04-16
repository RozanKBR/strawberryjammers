using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RouletteWheel : MonoBehaviour
{
    private List<Weapon> WeaponList = new List<Weapon>();
    public float DistanceFromWheel = 5.0f;

    public new Animation animation;

    public GameObject []rouletteWheel;
    public Weapon currentWeapon;
    int index;

    private Transform target;
    public AudioSource audioSource;

    
	// Use this for initialization
	void Start ()
    {
        animation = this.GetComponent<Animation>();

        rouletteWheel = GameObject.FindGameObjectsWithTag("RouletteWheel");

        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        foreach (GameObject RouletteWheel in rouletteWheel)
        {
            float dist = Vector3.Distance(target.position, RouletteWheel.transform.position);

            if (dist < DistanceFromWheel)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("SpinWheel");
                    //animation.Play();
                    audioSource.Play();
                }
            }
        }
	
	}

    public void RecieveWeapon()
    {
        Weapon[] randomWeapons = GameManager._Instance.AvailableWeapons;
        index = Random.Range(0, randomWeapons.Length);
        currentWeapon = randomWeapons[index];
        print(currentWeapon.name);
    }

    private void AddWeaponToList ()
    {

    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, DistanceFromWheel);
    }

    

}
