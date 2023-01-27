using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pin5 : MonoBehaviour
{
    public static int count5 = 0;

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "cube")
        {

            count5 = 1;

        }
    }
}
