using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//[Serializable] özniteliði, Unity'nin sýnýfýn
//üyelerini incelemesine izin vererek inspectorde görünmelerine neden olur.
public class TrainingSet
{
    public double[] input;
    public double output;
}

public class Perceptron : MonoBehaviour
{
    public TrainingSet[] ts;
    double[] weights = { 0, 0 };//there are two inputs and every input has a own weight
    double bias = 0;
    double totalError = 0;
    //that's the difference between our desired output and the actual output from the perception

    public SimpleGrapher sg;
    void InitialiseWeights()
    {
        for (int i = 0; i < weights.Length; i++)
        {
            weights[i] = Random.Range(-1.0f, 1.0f);
        }
        bias = Random.Range(-1.0f, 1.0f);
    }

    void Train(int epochs)
    {//how many times we wnat to pass the training set throught the perception
     //TrainingSeti perceptrondan kaç kez geçirmek istiyoruz
        InitialiseWeights();

        for (int e = 0; e < epochs; e++)
        {
            totalError = 0;
            for (int t = 0; t < ts.Length; t++)
            {
                UpdateWeights(t);
                Debug.Log("W1: " + (weights[0]) + " W2: " + (weights[1]) + " B: " + bias);
            }
            Debug.Log("TOTAL ERROR: " + totalError);
        }
    }
    void UpdateWeights(int j)//taking a number of the rule of the line in the training set
    {
        double error = ts[j].output - CalcOutput(j);
        totalError += Mathf.Abs((float)error);
        for (int i = 0; i < weights.Length; i++)
        {
            weights[i] = weights[i] + error * ts[j].input[i];
        }
        bias += error;
    }

    void Start()
    {
        DrawAllPoints();
        Train(8);
       /* Debug.Log("Test 0 0: " + CalcOutput(0, 0));
        Debug.Log("Test 0 1: " + CalcOutput(0, 1));
        Debug.Log("Test 1 0: " + CalcOutput(1, 0));
        Debug.Log("Test 1 1: " + CalcOutput(1, 1));*/
    }
    double DotProductBias(double[] v1, double[] v2)//weights,inputs
    {
        //input_0 * weight_0 + input_1 * weight_1 + bias
        if (v1 == null || v2 == null)
        {
            return -1;
        }
        if (v1.Length != v2.Length)
        {
            return -1;
        }
        double d = 0;
        for (int x = 0; x < v1.Length; x++)//it doesnt matter which one
        {
            d += v1[x] * v2[x];
        }
        d += bias;

        return d;
    }

    double CalcOutput(int i)
    {
        double dp = DotProductBias(weights, ts[i].input);
        if (dp > 0) return (1);
        return (0);
    }
    double CalcOutput(double i1,double i2)
    {
        double[] inp = new double[] { i1, i2 };
        double dp = DotProductBias(weights, inp);
        if (dp > 0) return (1);
        return (0);
    }
    void DrawAllPoints()
    {
        for(int t = 0; t < ts.Length; t++)
        {
            if (ts[t].output == 0)
            {
                //sg.DrawPoint((float)ts[t].input[0], (float)ts[t].input[1], Color.magenta);
                //when output closes to zero that means we're dealing with weapon
                
            }
            else
            {
                //sg.DrawPoint((float)ts[t].input[0], (float)ts[t].input[1], Color.green);
                //food
            }
        }
    }
}
