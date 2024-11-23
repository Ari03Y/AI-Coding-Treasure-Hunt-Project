// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// public class UpdateHealthBarPosition : MonoBehaviour
// {
//     // Start is called before the first frame update
//     public Transform target;

//     public Vector3 offSet = new Vector3(0,2,0);
//     public Canvas canvas;
//     // Update is called once per frame
//     public void LateUpdate()
//     {
//         transform.position = Camera.main.WorldToScreenPoint(target.position+offSet);
//         Vector3 screenPosition = Camera.main.WorldToScreenPoint(target.position + offSet);
//         bool isOnScreen = screenPosition.z < 0 && screenPosition.x > 0 && screenPosition.x <Screen.width && screenPosition.y > 0 && screenPosition.y <Screen.width;
//         canvas.alpha = isOnScreen ? 1:0;
//         if(isOnScreen){
//             transform.position = screenPosition;
//         }
//     }
// }
