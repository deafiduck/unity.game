using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain2 : MonoBehaviour
{
    public GameObject paddle_;
    float paddleMinY_ = 8.8f;
    float paddleMaxY_ = 17.4f;
    float paddleMaxSpeed_ = 15;
    



    void Start()
    {
        
    }

    void Update()
    {
        if (paddle_.GetComponent<Collider2D>().gameObject.tag != "top")
        {
            if (Input.GetKey("w"))
            {
                paddle_.transform.Translate(0, paddleMaxSpeed_ * Time.deltaTime, 0);
            }
            else if (Input.GetKey("s"))
            {
                paddle_.transform.Translate(0, -paddleMaxSpeed_ * Time.deltaTime, 0);
            }


        }
        
        
    }
    
}
