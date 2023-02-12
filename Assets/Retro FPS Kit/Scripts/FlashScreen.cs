using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FPSRetroKit
{

    public class FlashScreen : MonoBehaviour
    {

        //FADEOUT, FADEIN, FLASH EFFECT ON SCREEN FOR: ammos, death, extra effects

        Image flashScreen; //Image taken from this object. 
        [SerializeField] float howLongFlash = 5; //How long flash should last

        void Start()
        {
            flashScreen = GetComponent<Image>();
        }

        void Update()
        {

            if (flashScreen.color.a > 0) //if flash is active (image/colours are visible - flash is on screen)
            {
                //Make flash screen only for X seconds (set in inspector) and  gradually make it invisible
                Color invisible = new Color(flashScreen.color.r, flashScreen.color.g, flashScreen.color.b, 0);
                flashScreen.color = Color.Lerp(flashScreen.color, invisible, howLongFlash * Time.deltaTime);
            }
        }

        //If Player takes Damage, activate flash then "Update" function slowly makes it transparent
        public void TookDamage()
        {
            //RGB = IF Player receives damage then make screen RED (Red = 1, Green = 0, Blue = 0, transparency 0.8)
            flashScreen.color = new Color(1, 0, 0, 0.8f);
        }

        //If Player pickedup Bonus, activate flash then "Update" function slowly makes it transparent
        public void PickedUpBonus()
        {
            //RGB = IF Player receive bonus, pickup something then make screen Blue (Red = 0, Green = 0, Blue = 1, transparency 0.8)
            flashScreen.color = new Color(0, 0, 1, 0.8f);
        }
    }
}