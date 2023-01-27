using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pin6 : MonoBehaviour
{
    public static int count6 = 0;

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "cube")
        {

            count6 = 1;

        }
    }
}
