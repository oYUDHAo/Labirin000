using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gyro : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        Input.gyro.enabled = true;
    }

    // Update is called once per frame
    private void Update()
    {
        this.transform.rotation = Quaternion.Euler(90f, 90f, 0f) 
        * Input.gyro.attitude * new Quaternion(0, 0, 1, 0);
    }
}
