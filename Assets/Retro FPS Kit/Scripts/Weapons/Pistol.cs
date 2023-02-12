using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace FPSRetroKit
{

	[RequireComponent(typeof(AudioSource))]
	public class Pistol : MonoBehaviour
	{
		//WEAPON: Pistol TEMPLATE

		[Header("Prefabs and Sprites")]
		public GameObject bloodSplat; //GameObject of blood after projectile hits enemy
		public Sprite idlePistol; //Pistol sprite when idle
		public Sprite shotPistol; //Pistol sprite when shooting

		[Header("Pistol Settings")]
		public float pistolDamage; //How much damage pistol deals
		public float pistolRange; //How far can we shoot with Pistol
		public float howLongReload;

		[Header("Pistol Audio Settings")]
		public AudioClip shotSound; //Shot sound on shooting
		public AudioClip reloadSound; //Reloading sound
		public AudioClip emptyGunSound; //No Ammo sound

		[Header("Pistol Specification")]
		public Text ammoText; //Text Ammo on UI Canvas

		public int ammoAmount; //How much ammo we have in pistol loaded
		public int ammoClipSize; //How much ammo we have in the bag

		public GameObject bulletHole; //Game Object for holes that projectiles make once they hit environment

		public int ammoLeft; //For scripting: how many ammo left
		public int ammoClipLeft; //How many clips we have left

		bool isShot; //Check if we're shooting/shot
		bool isReloading; //Check if we're reloading

		AudioSource source; //Audiosource to handle sounds from this gameobject

		void Awake()
		{
			source = GetComponent<AudioSource>();
			ammoLeft = ammoAmount; //Set ammo left as starting amount
			ammoClipLeft = ammoClipSize; //Set ammo clip left as starting amount
		}

		//Do not reload on start game or on selecting the weapon
		void OnEnable()
		{
			isReloading = false;
		}


		void Update()
		{
			ammoText.text = "Ammo" + "\n" + ammoClipLeft + " / " + ammoLeft; //change UI Text of how much ammo left

			//If mouse button is clicked and pistol is not reloading then shoot
			if (Input.GetButtonDown("Fire1") && isReloading == false)
				isShot = true;

			//If clicked R and already not reloading and we have enough ammo clips - then reload
			if (Input.GetKeyDown(KeyCode.R) && isReloading == false && ammoClipLeft != ammoClipSize)
			{
				Reload();
			}
		}

		void FixedUpdate()
		{
			// Calculating spread of the weapon (pattern of where the projectile shoots around crosshair)
			Vector2 bulletOffset = Random.insideUnitCircle * DynamicCrosshair.spread;

			// Creating projectile area from our camera to the center of the screen
			Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2 + bulletOffset.x, Screen.height / 2 + bulletOffset.y, 0));
			RaycastHit hit;

			//If we have shot and we have anough ammo and are not reloading weapon
			if (isShot == true && ammoClipLeft > 0 && isReloading == false)
			{
				isShot = false; //After shot, let's set "not shooting" state, as before next shot we have a break
				DynamicCrosshair.spread += DynamicCrosshair.PISTOL_SHOOTING_SPREAD; //Get spread
				ammoClipLeft--; //Remove ammo from pistol
				source.PlayOneShot(shotSound); //Play sound of shooting once each shot
				StartCoroutine("shot"); //Start function of shooting

				//Once we click Mouse button (shooting) and our projectile is in collision with other object
				//Let's make following functions
				if (Physics.Raycast(ray, out hit, pistolRange))
				{
					Debug.Log("This projectile is in collision with " + hit.collider.gameObject.name);

					// Sending information to the object that we have hit it
					// Hit trigger(object) should get information that it's being hit with pistol and take dealt damage 
					hit.collider.gameObject.SendMessage("AddDamage", pistolDamage, SendMessageOptions.DontRequireReceiver);

					//If we've set object to AntiBulletHole, then bullethole won't show up

					//If Projectile hits enemy then:
					if (hit.transform.CompareTag("Enemy"))
					{
						Instantiate(bloodSplat, hit.point, Quaternion.identity); //Make blood on hit 
						if (hit.collider.gameObject.GetComponent<EnemyStates>().currentState == hit.collider.gameObject.GetComponent<EnemyStates>().patrolState ||
							hit.collider.gameObject.GetComponent<EnemyStates>().currentState == hit.collider.gameObject.GetComponent<EnemyStates>().alertState)

							// If enemy is in partolling or alarm states - let's send our shooting location to the Enemy (EnemyState.script)
							hit.collider.gameObject.SendMessage("HiddenShot", transform.parent.transform.position, SendMessageOptions.DontRequireReceiver);
					}
					else
					{
						//Making Bullet prefab change size to 0.05 before spawning
						bulletHole.transform.GetChild(0).gameObject.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

						// Let's create hole (bullethole) on object that has been hit by our projectile
						// Now we are changing projectile parent in hierarchy for the bullethole. Bullethole is now child of object it did hit.
						Instantiate(bulletHole, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)).transform.parent =
							hit.collider.gameObject.transform;
						


					}

				}
			}
			else if (isShot == true && ammoClipLeft <= 0 && isReloading == false)
			{
				//If we shoot without having any ammo - reload weapon
				isShot = false;
				Reload();
			}
		}

		// Reloading Function
		void Reload()
		{
			//Count how many bullets should we reload
			int bulletsToReload = ammoClipSize - ammoClipLeft;
			//If we have some ammo then fill up only needed ammo
			if (ammoLeft >= bulletsToReload)
			{
				StartCoroutine("ReloadWeapon");
				ammoLeft -= bulletsToReload;
				ammoClipLeft = ammoClipSize;
			}

			//Reload lasting amount of ammo that we have left in bag and not full magazine
			else if (ammoLeft < bulletsToReload && ammoLeft > 0)
			{
				StartCoroutine("ReloadWeapon");
				ammoClipLeft += ammoLeft;
				ammoLeft = 0;
			}
			//If we have no ammo play empty gun sound
			else if (ammoLeft <= 0)
			{
				source.PlayOneShot(emptyGunSound);
			}
		}

		// This function plays the reload sound on reload
		IEnumerator ReloadWeapon()
		{
			isReloading = true; //Now we're reloading
			source.PlayOneShot(reloadSound); //Play reload sound once
			yield return new WaitForSeconds(howLongReload); //Wait X seconds for reload
			isReloading = false; //stop reloading at the end
		}

		// Function during shooting changes weapon graphic for 0.1seconds. "Shooting Sprite"
		IEnumerator shot()
		{
			GetComponent<SpriteRenderer>().sprite = shotPistol; //Show Sprite of Shooting (Change graphic on screen)
			yield return new WaitForSeconds(0.1f); //Wait 0.1second
			GetComponent<SpriteRenderer>().sprite = idlePistol; //Change back to idle weapon after we shot
		}

		//If we find ammo bonus or fillup - add ammo with value set in the bonus prefab
		public void AddAmmo(int value)
		{
			ammoLeft += value;
		}

	}
}