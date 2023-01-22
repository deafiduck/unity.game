using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA1 : MonoBehaviour
{
    List<int> genes = new List<int>();
    int dnaLength = 0;
    int maxValues = 0;
    //her gen için max sýnýrý olan random sayýlar belirliycez 
    //ve bu sayýlarýn her birine bir amaç belirliycez

    public DNA1(int l,int v)//constructor
    {
        
        dnaLength = l;
        maxValues = v;
        SetRandom();
    }

    public void SetRandom()
    {
        genes.Clear();//dizideki indeksleri siler 
        for(int i = 0; i < dnaLength; i++)
        {
            genes.Add(Random.Range(0, maxValues));
        }
    }

    public void SetInt(int pos,int value)
    {
        genes[pos] = value;//particular value
    }

    public void Combine(DNA1 d1,DNA1 d2)//diðer jenerasyon için ana jenerasyondan gen alýr
    {
        for(int i = 0; i < dnaLength; i++)
        {
            if (i < dnaLength / 2)//first half use from parent1
            {
                int c = d2.genes[i];
                genes[i] = c;
            }
            else
            {
                int c = d2.genes[i];//second half ue from parent2
                genes[i] = c;
            }
            // 1 1 1 2 2 2 parent1 gens
            // 3 3 3 4 4 4 parent2 gens
            //1 1 1 4 4 4  first child gens
            // 3 3 3 2 2 2
        }
    }
    public void Mutate()
    {
        genes[Random.Range(0, dnaLength)] = Random.Range(0, maxValues);
        //set random value a genes random position
    }
    public int GetGene(int pos)
    {
        return genes[pos];
    }
}
