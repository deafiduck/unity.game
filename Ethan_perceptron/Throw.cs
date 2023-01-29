using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{
    public GameObject spherePrefab;
    public GameObject cubePrefab;
    public Material green;
    public Material red;

    Perceptron_e p;

    void Start()
    {
        p = GetComponent<Perceptron_e>(); 
    }
    void Update()
    {
        /* input1 | input2 | desired output
        * red 0     sphere 0     0
        * red 0     cube   1     1
        * green 1   sphere 0     1
        * green 1   cube   1     1
         */
        if (Input.GetKeyDown("1")){
            GameObject g = Instantiate(spherePrefab, Camera.main.transform.position, Camera.main.transform.rotation);
            g.GetComponent<Renderer>().material = red;
            g.GetComponent<Rigidbody>().AddForce(0, 0, 500);
            p.SendInput(0, 0, 0);//this func. give a desicion for crouch or not
            //input2,in0put1,output

        }
        else if (Input.GetKeyDown("2"))
        {
            GameObject g = Instantiate(spherePrefab, Camera.main.transform.position, Camera.main.transform.rotation);
            g.GetComponent<Renderer>().material = green;
            g.GetComponent<Rigidbody>().AddForce(0, 0, 500);
            p.SendInput(0, 1, 1);//this func. give a desicion for crouch or not
        }
        else if (Input.GetKeyDown("3"))
        {
            GameObject g = Instantiate(cubePrefab, Camera.main.transform.position, Camera.main.transform.rotation);
            g.GetComponent<Renderer>().material = red;
            g.GetComponent<Rigidbody>().AddForce(0, 0, 500);
            p.SendInput(1, 0, 1);//this func. give a desicion for crouch or not
        }
        else if (Input.GetKeyDown("4"))
        {
            GameObject g = Instantiate(cubePrefab, Camera.main.transform.position, Camera.main.transform.rotation);
            g.GetComponent<Renderer>().material = green;
            g.GetComponent<Rigidbody>().AddForce(0, 0, 500);
            p.SendInput(1, 1, 1);//this func. give a desicion for crouch or not
        }
    }
}
