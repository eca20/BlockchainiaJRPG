using UnityEngine;
using System.Collections;

namespace FPSRetroKit
{
    public class DynamicCrosshair : MonoBehaviour
    {
        //Crosshair For shooting

        [Header("Crosshair settings")]
        static public float spread = 0;

        public const int PISTOL_SHOOTING_SPREAD = 20;
        public const int JUMP_SPREAD = 50;
        public const int WALK_SPREAD = 10;
        public const int RUN_SPREAD = 25;

        [Header("Crosshair GameObject (Canvas)")]
        public GameObject crosshair;

        GameObject topPart;
        GameObject bottomPart;
        GameObject leftPart;
        GameObject rightPart;

        float initialPosition;

        //Checking all parts of the crosshair (they are separate to freely manipulate them)
        void Start()
        {
            topPart = crosshair.transform.Find("TopPart").gameObject;
            bottomPart = crosshair.transform.Find("BottomPart").gameObject;
            leftPart = crosshair.transform.Find("LeftPart").gameObject;
            rightPart = crosshair.transform.Find("RightPart").gameObject;

            initialPosition = topPart.GetComponent<RectTransform>().localPosition.y;
        }

        void Update()
        {
            // Changing crosshair location depends on "spread" value 
            // If "spread" is other than 0, then spread should be slowly decreased 
            if (spread != 0)
            {
                topPart.GetComponent<RectTransform>().localPosition = new Vector3(0, initialPosition + spread, 0);
                bottomPart.GetComponent<RectTransform>().localPosition = new Vector3(0, -(initialPosition + spread), 0);
                leftPart.GetComponent<RectTransform>().localPosition = new Vector3(-(initialPosition + spread), 0, 0);
                rightPart.GetComponent<RectTransform>().localPosition = new Vector3(initialPosition + spread, 0, 0);
                spread -= 1;
            }
        }
    }
}