using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pin4 : MonoBehaviour
{
    public static int count4 = 0;

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "cube")
        {

            count4 = 1;

        }
    }
}
