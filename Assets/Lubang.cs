using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lubang : MonoBehaviour
{
   bool entered = false;
   public bool Entered {get => entered;}

   private void OnTriggerEnter(Collider other)
   {
    Debug.Log("Entered");
    entered = true;
   }
}
