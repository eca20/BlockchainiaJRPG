using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FPSRetroKit
{

    public class RocketLauncher : MonoBehaviour
    {
        //WEAPON: RocketLauncher TEMPLATE

        [Header("Prefabs for the Launcher")]
        public GameObject rocket; //missile that is being shot from this weapon
        public GameObject explosion; //What shows when it interacts with any trigger/target (explosion how looks)
        public GameObject spawnPoint; //Where projectile is being spawned from the weapon (at front of player, around weapon)

        [Header("Sounds to This Weapon")]
        public AudioClip shotSound; //Sound Shot
        public AudioClip reloadSound; //Sound Reload
        public AudioClip emptyGunSound; //Sound of weapon without ammo
        public AudioClip explosionSound; //Sound of explosion once projectile hits something

        [Header("Projectile Settings")]
        public float howLongReload; //How long reloading lasts
        public float rocketForce; //How far/strong is the shot
        public float explosionRadius; //Explotion radius - how huge a hit is
        public float explosionDamage; //How much damage explotion deals
        public LayerMask explosionLayerMask; //With what TAG it explodes (On touch with enemy)

        [Header("UI For Weapon")]
        public Text ammoText; //Ammo Text (Canvas displayed on screen)
        public int rocketsAmount; //Ammo Amount available

        public int rocketsLeft; //How many ammo left (integer)
        AudioSource source; //Audio Source for sounds

        bool isReloading; //Check if weapon is reloading now
        bool isCharged = true; //Check if weapon has ammo 
        bool isShot; //Check if weapon shoots now

        int rocketInChamber;
        GameObject crosshair; //Change crosshair on shoot etc (manipulate crosshair to action of weapons)

        private void Awake()
        {
            source = GetComponent<AudioSource>();
            rocketsLeft = rocketsAmount; //Set rockets left as starting amount
        }

        //When game starts or when we choose this weapon
        void OnEnable()
        {
            isReloading = false; //Do not reload on start, be pre-loaded
            crosshair = GameObject.Find("Crosshair"); //Take crosshair object
            crosshair.SetActive(false); // Disable CrossHair for this weapon
        }

        void OnDisable()
        {
            crosshair.SetActive(true); //Enable crosshair if this weapon is not chosen
        }

        void Update()
        {
            rocketInChamber = isCharged ? 1 : 0;
            ammoText.text = "Ammo" + "\n" + rocketInChamber + " / " + rocketsLeft; //Show text of how many ammo left

            //Shooting using mouse button if: weapon can shoot and is not reloading
            if (Input.GetButtonDown("Fire1") && isCharged && !isReloading)
            {
                isCharged = false; //After we shoot - we cannot shoot again straight away, we need to charge/reload
                source.PlayOneShot(shotSound); //Play one sound of shooting
                GameObject rocketInstantiated = (GameObject)Instantiate(rocket, spawnPoint.transform.position, Quaternion.identity); //Create projectile
                rocketInstantiated.GetComponent<Rocket>().damage = explosionDamage; //Create damage from rocket's explotion
                rocketInstantiated.GetComponent<Rocket>().radius = explosionRadius; //Make explotion radius
                rocketInstantiated.GetComponent<Rocket>().explosionSound = explosionSound; // Play explotion sound on explotion
                rocketInstantiated.GetComponent<Rocket>().layerMask = explosionLayerMask; //Take layer mask from Enemy
                rocketInstantiated.GetComponent<Rocket>().explosion = explosion; //Make explotion
                Rigidbody rocketRb = rocketInstantiated.GetComponent<Rigidbody>(); //Get rocket's rigidbody. Projectile should fall after some time
                rocketRb.AddForce(Camera.main.transform.forward * rocketForce, ForceMode.Impulse); //Add force to the projectile so it "shoots"
                Reload(); //Finally Reload at the end (charge)
            }
            else if (Input.GetButtonDown("Fire1") && !isCharged && !isReloading)
                Reload(); //if weapon is not charged and is not reloading - then we click mouse to reload weapon
        }

        void Reload()
        {
            //If there is no ammo - play once empty ammo sounds
            if (rocketsLeft <= 0)
            {
                source.PlayOneShot(emptyGunSound);
            }
            else //otherwise if we have ammo, reload the weapon and remove ammo from backpack
            {
                StartCoroutine("ReloadWeapon");
                rocketsLeft--;
                isCharged = true; //if reloaded weapon then we have charged weapon
            }
        }

        //Reload Weapon Function
        IEnumerator ReloadWeapon()
        {
            isReloading = true; //We activate reloading, so we cannot shoot while reloading
            source.PlayOneShot(reloadSound); //Play once reload sound
            yield return new WaitForSeconds(howLongReload); //Wait X seconds for reloading (set in inspector)
            isReloading = false; //change reloading state to false. Now player can shoot
        }
    }
}