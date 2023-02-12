using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSRetroKit
{
    public class WeaponSwitch : MonoBehaviour
    {

        //Switch Weapons (1, 2, 3 keyboard). And add more weapons

        public List<Transform> weapons; //How many weapons (list of weapons)
        public int initialWeapon; //Which weapon player starts with or is main
        public bool autoFill; //Automatically fill ammo
        int selectedWeapon; //which weapon is selected


        private void Awake()
        {
            //Automatically add ammo and weapons if autoFill is selected.
            if (autoFill)
            {
                weapons.Clear();
                foreach (Transform weapon in transform)
                    weapons.Add(weapon);
            }
        }

        void Start()
        {
            selectedWeapon = initialWeapon % weapons.Count;
            UpdateWeapon();
        }

        void Update()
        {
            // Scroll to change the weapon
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
                selectedWeapon = (selectedWeapon + 1) % weapons.Count;
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
                selectedWeapon = Mathf.Abs(selectedWeapon - 1) % weapons.Count;

            // Buttons on Keyboard to change the weapon (1) and (2) and (3)
            if (Input.GetKeyDown(KeyCode.Alpha1))
                selectedWeapon = 0; //if clicked "1" then select weapon under 0 from list inspector
            if (Input.GetKeyDown(KeyCode.Alpha2) && weapons.Count > 1)
                selectedWeapon = 1; //if clicked "2" then select weapon under 1 from list inspector
            if (Input.GetKeyDown(KeyCode.Alpha3) && weapons.Count > 1)
                selectedWeapon = 2; //if clicked "3" then select weapon under 2 from list inspector


            UpdateWeapon();
        }

        void UpdateWeapon()
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                //Change weapon disactive if other weapon is being selected
                if (i == selectedWeapon)
                    weapons[i].gameObject.SetActive(true);
                else
                    weapons[i].gameObject.SetActive(false);
            }
        }
    }
}