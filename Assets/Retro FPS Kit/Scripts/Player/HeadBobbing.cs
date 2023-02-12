
//This scrip allows our head bobbing on gameplay

using UnityEngine;
using System.Collections;

namespace FPSRetroKit
{

    public class HeadBobbing : MonoBehaviour
    {
        public float bobbingSpeed = 0.18f; //How often head bobbing should occur while walking
        public float bobbingHeight = 0.2f; //How strong the bobbing is
        public float midpoint = 1.8f; //What is the height of bobbing (according to player height = 2)
        public bool isHeadBobbing = true; //Is headbobbing active or disactive?

        private float timer = 0.0f; //Headbobbing goes up/down on time to reset functionality

        void Update()
        {
            float waveslice = 0.0f;
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 cSharpConversion = transform.localPosition; //Transform position of this object on bobbing 


            //Maths for headbobbing (will be updated for easier method in the update)
            if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
            {
                timer = 0.0f;
            }
            else
            {
                waveslice = Mathf.Sin(timer);
                timer = timer + bobbingSpeed;
                if (timer > Mathf.PI * 2)
                {
                    timer = timer - (Mathf.PI * 2);
                }
            }
            if (waveslice != 0)
            {
                float translateChange = waveslice * bobbingHeight;
                float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
                totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
                translateChange = totalAxes * translateChange;
                if (isHeadBobbing == true)
                    cSharpConversion.y = midpoint + translateChange;
                else if (isHeadBobbing == false)
                    cSharpConversion.x = translateChange;
            }
            else
            {
                if (isHeadBobbing == true)
                    cSharpConversion.y = midpoint;
                else if (isHeadBobbing == false)
                    cSharpConversion.x = 0;
            }

            transform.localPosition = cSharpConversion;
        }
    }
}