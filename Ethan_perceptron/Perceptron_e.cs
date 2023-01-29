using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
//[Serializable] özniteliði, Unity'nin sýnýfýn
//üyelerini incelemesine izin vererek inspectorde görünmelerine neden olur.
public class TrainingSet_
{
    public double[] input;
    public double output;
}

public class Perceptron_e : MonoBehaviour
{
    List<TrainingSet_>ts=new List<TrainingSet_>();
    double[] weights = { 0, 0 };//there are two inputs and every input has a own weight
    double bias = 0;
    double totalError = 0;
    //that's the difference between our desired output and the actual output from the perception
    public GameObject npc;//controlling

    public void SendInput(double i1,double i2,double o)//
    {
        /* input2 | input1 | desired output
         * red      sphere      0
         * red      cube        1
         * green    sphere      1
         * green    cube        1
         */
        double result = CalcOutput(i1, i2);
        Debug.Log(result);//this print result between 0 - 1 
        if (result == 0)//duck and cover /eðil ve siper al
        {
            npc.GetComponent<Animator>().SetTrigger("Crouching");
            npc.GetComponent<Rigidbody>().isKinematic = false;
        }
        else
        {
            npc.GetComponent<Rigidbody>().isKinematic = true;
        }
        //learn from it for next time
        TrainingSet_ s = new TrainingSet_();
        s.input = new double[2] { i1, i2 };
        s.output = 0;
        ts.Add(s);
        Train();

    }

    void InitialiseWeights()
    {
        for (int i = 0; i < weights.Length; i++)
        {
            weights[i] = Random.Range(-1.0f, 1.0f);
        }
        bias = Random.Range(-1.0f, 1.0f);
    }

    void Train()
    {//how many times we wnat to pass the training set throught the perception
     //TrainingSeti perceptrondan kaç kez geçirmek istiyoruz
       

        
            for (int t = 0; t < ts.Count; t++)
            {
                UpdateWeights(t);
               
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
    double ActivationFunction(double dp)
    {
        if (dp > 0) return (1);
        return (0);
    }
    void LoadWeights()
    {
        string path = Application.dataPath + "/weights.txt";
        if (File.Exists(path))
        {
            var sr = File.OpenText(path);
            string line = sr.ReadLine();
            string[] w = line.Split(',');
            weights[0] = System.Convert.ToDouble(w[0]);
            weights[1] = System.Convert.ToDouble(w[1]);
            bias = System.Convert.ToDouble(w[2]);
            Debug.Log("loading");
        }
    }

    void SaveWeights()
    {
        string path = Application.dataPath + "/weights.txt";
        var sr = File.CreateText(path);
        sr.WriteLine(weights[0] + "," + weights[1] + "," + bias);
        sr.Close();
    }
    void Start()
    {
        InitialiseWeights();
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
        return(ActivationFunction(DotProductBias(weights, ts[i].input)));
       
    }
    double CalcOutput(double i1, double i2)
    {
        double[] inp = new double[] { i1, i2 };
        return (ActivationFunction(DotProductBias(weights,inp)));
        //DotProductBias binary result and ActivationFunction give a desicion if its bigger 1 or not
    }
   
    void DrawAllPoints()
    {
        for (int t = 0; t < ts.Count; t++)
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
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            InitialiseWeights();
            ts.Clear();
        }
        else if (Input.GetKeyDown("s"))
        {
            SaveWeights();

        }
        else if (Input.GetKeyDown("l"))
        {
            LoadWeights();

        }
    }
}
