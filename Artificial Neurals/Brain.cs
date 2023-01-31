using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Brain : MonoBehaviour//using a MonoBehaviour because we use start and update func
{
    ANN ann;
    double sumSquareError = 0;
    //this value calculated in statistics to work out how closely the model that you've constructed fits the data you fed into it 

    void Start()
    {
        ann = new ANN(2, 1, 1, 2, 0.8);
        //alpha can not take 0 because thats mean it wouldn't learn anything
        //being a high value for alpha run our network quicker bbut it will also couse issue
        List<double> result;

        for(int i = 0; i < 1000; i++)//if do you high this value the result get closer the true result
        {
            sumSquareError = 0;
            result = Train(1, 1, 0);
            sumSquareError += Mathf.Pow((float)result[0] - 0,2);//0,2 0'ý outputun 0 ý
            result = Train(1, 0, 1);
            sumSquareError += Mathf.Pow((float)result[0] - 1, 2);
            result = Train(0, 1, 1);
            sumSquareError += Mathf.Pow((float)result[0] - 1, 2);
            result = Train(0, 0, 0);
            sumSquareError += Mathf.Pow((float)result[0] - 0, 2);
            //XOR operation

        }
     Debug.Log("SSE: " + sumSquareError);

        result = Train(1, 1, 0);
        Debug.Log(" 1 1 " + result[0]);
        result = Train(1, 0, 1);
        Debug.Log(" 1 0 " + result[0]);
        result = Train(0, 1, 1);
        Debug.Log(" 0 1 " + result[0]);
        result = Train(0, 0, 0);
        Debug.Log(" 0 0 " + result[0]);
    }
    List<double>Train(double i1,double i2,double o)
    {
        List<double> inputs = new List<double>();
        List<double> outputs = new List<double>();
        inputs.Add(i1);
        inputs.Add(i2);
        outputs.Add(o);
        return (ann.Go(inputs, outputs));
    }
    
}
