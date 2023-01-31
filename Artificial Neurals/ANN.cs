using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANN : MonoBehaviour
{
    //layers /neurons/ neurons input 3 layers
    public int numInputs;
    public int numOutputs;//you might have a more than one output
    public int numHidden;//layers between input and output layers
    public int numPerHidden;//hidden layers count
    public double alpha;//determines how much any particular training sample comes in is going to affect
    //neurondan her seferinde eðitim setinin yalnýzca belirli bir yüzdesini entegre etmesini isteyebilirsiniz.
    //you may only want to adjust the w a little bit each time, not a full amount based an h value that's coming in
    List<Layer> layers = new List<Layer>();

    public ANN(int nI,int nO,int nH,int nPH,double a)
    {
        numInputs = nI;
        numOutputs = nO;
        numHidden = nH;
        numPerHidden = nPH;
        alpha = a;

        if (numHidden > 0)
        {
            layers.Add(new Layer(numPerHidden, numInputs));//number of neuron

            for(int i = 0; i < numHidden - 1; i++)
            {
                layers.Add(new Layer(numPerHidden, numPerHidden));
            }
            layers.Add(new Layer(numOutputs, numInputs));
        }
        else
        {
            layers.Add(new Layer(numOutputs, numInputs));
        }
    }
    public List<double>Go(List<double> inputValues,List<double> desiredOutput)
    {
        List<double> inputs = new List<double>();
        List<double> outputs = new List<double>();

        if (inputValues.Count != numInputs)
        {
            Debug.Log("ERROR: Number of inputs must be " + numInputs);
            return outputs;
        }
        inputs = new List<double>(inputValues);
        for(int i = 0; i < numHidden + 1; i++)//i is layer
        {
            if (i > 0)
            {
                inputs = new List<double>(outputs);
            }
            outputs.Clear();
            for (int j = 0; j < layers[i].numNeurons; j++)
            {
                double N = 0;//how long
                layers[i].neurons[j].inputs.Clear();
                for(int k = 0; k < layers[i].neurons[j].numInputs; k++)
                {
                    layers[i].neurons[j].inputs.Add(inputs[k]);
                    N += layers[i].neurons[j].weights[k] * inputs[k];
                }
                N -= layers[i].neurons[j].bias;
                layers[i].neurons[j].output = ActivationFunction(N);
                outputs.Add(layers[i].neurons[j].output);
            }
        }
        UpdateWeights(outputs, desiredOutput);
        return outputs;
    }
    void UpdateWeights(List<double> output,List<double> desiredOutput)
    {
        double error;
        for(int i = numHidden; i >= 0; i--)
        {
            for(int j = 0; j < layers[i].numNeurons; j++)
            {
                if ( i == numHidden)//if we are at the and aware in the output layer
                {
                    error = desiredOutput[j] - output[j];//istenen çýktý- bulunan çýktý
                    layers[i].neurons[j].errorGradient = output[j] * (1 - output[j]) * error;
                    //her nöronun sebep olduðu error miktarý hesaplanýr
                    //errorGradient calculated with Delta Rule
                }
                else
                {
                    layers[i].neurons[j].errorGradient = layers[i].neurons[j].output * (1 - layers[i].neurons[j].output);
                    double errorGradSum=0;//error in th
                    for(int p = 0; p < layers[i + 1].numNeurons; p++)
                    {
                        errorGradSum += layers[i + 1].neurons[p].errorGradient * layers[i + 1].neurons[p].weights[j];
                    }
                    layers[i].neurons[j].errorGradient *= errorGradSum;
                }
                for(int k = 0; k < layers[i].neurons[j].numInputs; k++)
                {
                    if (i == numHidden)
                    {
                        error = desiredOutput[j] - output[j];
                        layers[i].neurons[j].weights[k] += alpha * layers[i].neurons[j].inputs[k] * error;
                    }
                    else
                    {
                        layers[i].neurons[j].weights[k] += alpha * layers[i].neurons[j].inputs[k] * layers[i].neurons[j].errorGradient;
                    }
                }
                layers[i].neurons[j].bias += alpha * -1 * layers[i].neurons[j].errorGradient;
            }
        }
    }
    double ActivationFunction(double value)
    {
        return Sigmoid(value);
    }
    double Step(double value)//also call a binary step
    {
        if (value < 0) return 0;
        else return 1;
    }

    double Sigmoid(double value)//soft step exponential value
    {
        double k = (double)System.Math.Exp(value);
        return k / (1.0f + k);
    }
}
