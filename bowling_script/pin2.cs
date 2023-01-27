using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pin2 : MonoBehaviour
{
    public static int count2 = 0;

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "cube")
        {

            count2 = 1;

        }
    }
}
