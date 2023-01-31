using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layer : MonoBehaviour
{
    public int numNeurons;//this is telling the layer how many neurons it needs to create
    public List<Neuron> neurons = new List<Neuron>();

    public Layer(int nNeurons,int numNeuronInputs)
    {
        //numNeuronInputs(number of neuron) is the number of neurons in the prvious layer
        numNeurons = nNeurons;
        for(int i = 0; i < nNeurons; i++)
        {
            neurons.Add(new Neuron(numNeuronInputs));
        }
    }
}
