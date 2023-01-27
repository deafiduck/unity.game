using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pin3 : MonoBehaviour
{
    public static int count3 = 0;

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "cube")
        {

            count3 = 1;

        }
    }
}
