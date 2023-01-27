using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pin1 : MonoBehaviour
{
    public static int count1 = 0;

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "cube")
        {

            count1 = 1;

        }
    }
}
