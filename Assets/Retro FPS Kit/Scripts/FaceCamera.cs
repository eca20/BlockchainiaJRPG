using UnityEngine;
using System.Collections;

//Change rotation of the object to the MainCamera (Player's camera in this example) - always.

namespace FPSRetroKit
{
    public class FaceCamera : MonoBehaviour
    {
        Vector3 cameraDirection; // Our MainCamera Position in World

        void Update()
        {
            cameraDirection = Camera.main.transform.forward;
            cameraDirection.y = 0;
            transform.rotation = Quaternion.LookRotation(cameraDirection);
        }
    }
}