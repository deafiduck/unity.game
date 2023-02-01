using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour
{

    public GameObject paddle;
    //public GameObject paddle2;
    public GameObject ball;
    Rigidbody2D brb;
    float yvel;//this is the y velocity the value we want outputted from an neural network
    float yvel2;
    float paddleMinY = 8.8f;//alanýn aþaðý uzunluðu
    float paddleMaxY = 17.4f;//alanýn yukarý uzunluðu
    float paddleMinY2 = 8.8f;
    float paddleMaxY2 = 17.4f;
    //how far we want to paddle to be able to travel
    float paddleMaxSpeed = 15;
    float paddleMaxSpeed2 = 15;
    public float numSaved = 0;//how many balls we actually hit
    public float numSaved2 = 0;
    public float numMissed = 0;//hw many balls we actually miss
    public float numMissed2 = 0;
   

    ANN ann;
    

   //  inputs value for neurons: ball direction & speed, ball position,racket position,racket velocit
   //ball x,ball y,ball velocity x,ball velocity y,paddle x,paddle y
   //output; paddle y
    void Start()
    {
        ann = new ANN(6, 1, 1, 4, 0.005);
        
        //one hidden layer and 4 hidden neurons
        brb = ball.GetComponent<Rigidbody2D>();
    }

   List<double>Run(double bx,double by,double bvx,double bvy,double px,double py,double pv,bool train)
    {
        List<double> inputs = new List<double>();
        List<double> outputs = new List<double>();
        inputs.Add(bx);
        inputs.Add(by);
        inputs.Add(bvx);
        inputs.Add(bvy);//velocity
        inputs.Add(px);
        inputs.Add(py);
        outputs.Add(pv);
        if (train)
        {
            return (ann.Train(inputs, outputs));
        }
        else
        {
            return (ann.CalcOutput(inputs, outputs));//calculate what output should be
        }
    }
    void Update()
    {
        /* float posy2 = Mathf.Clamp(paddle2.transform.position.y + (yvel2 * Time.deltaTime * paddleMaxSpeed2),
              paddleMinY2, paddleMaxY2);*/

        /* paddle2.transform.position = new Vector3(paddle2.transform.position.x, posy2, paddle2.transform.position.z);
         List<double> output2 = new List<double>();*/

       
        

        float posy = Mathf.Clamp(paddle.transform.position.y + (yvel * Time.deltaTime * paddleMaxSpeed),
            paddleMinY, paddleMaxY);//clamp burda max ve min value vermemizi saðlar
        paddle.transform.position = new Vector3(paddle.transform.position.x, posy, paddle.transform.position.z);

        List<double> output = new List<double>();
        int layerMask = 1 << 9;//this is essential making sure that collisions still happen with all sorts of layers without separating out the physics.
        RaycastHit2D hit = Physics2D.Raycast(ball.transform.position, brb.velocity, 1000, layerMask);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "tops")
            {
                Vector3 reflection = Vector3.Reflect(brb.velocity, hit.normal);//the normal caoming away from the boundary that we've hit
                /*Reflect (): Bir Vector3 deðiþkenin, ayný herhangi bir nesnenin bir aynada sahip olduðu yansýmasý gibi,
                 * bir normal ekseni kullanýlarak yansýmasý elde edilmesini saðlayan fonksiyondur. 
                 * Ýlk deðiþkeni yanstýlacak orijinal obje, 
                 * diðer deðiþken de yansýtma için kullanýlacak Vektor3 tipindeki deðiþkendir.
                 */
                hit = Physics2D.Raycast(hit.point, reflection, 1000, layerMask);
            }


            if (hit.collider != null && hit.collider.gameObject.tag == "backwall")
            {
                float dy = (hit.point.y - paddle.transform.position.y);//hareket etmesi gereken mesafe
              //  float dy2 = (hit.point.y - paddle2.transform.position.y);

                


                output = Run(ball.transform.position.x,
                    ball.transform.position.y,
                    brb.velocity.x, brb.velocity.y,
                    paddle.transform.position.x,
                    paddle.transform.position.y,
                    dy, true);

                yvel = (float)output[0];

               /* output2 = Run(ball.transform.position.x,
                    ball.transform.position.y,
                    brb.velocity.x, brb.velocity.y,
                    paddle2.transform.position.x,
                    paddle2.transform.position.y,
                    dy2, true);
                yvel2 = (float)output2[0];*/
            }

        }

        else
        {
            yvel = 0;
        }
    }
}
