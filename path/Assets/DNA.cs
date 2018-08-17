using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA
{
    public List<int> genes = new List<int>();
    private int dnaLength = 0;
    private int maxValue = 0;


    public DNA(int length, int max)
    {
        dnaLength = length;
        maxValue = max;
        SetRandom();
    }

    public void SetRandom()
    {
        genes.Clear();
        for (int i = 0; i < dnaLength; i++)
        {
            genes.Add(Random.Range(0, maxValue));
        }
    }

    public void Combine(DNA a, DNA b)
    {
        for (int i = 0; i < dnaLength; i++)
        {
            genes[i] = (i < dnaLength / 2) ? a.genes[i] : b.genes[i];
        }
    }

    public void Mutate()
    {
        genes[Random.Range(0, dnaLength)] = Random.Range(0, maxValue);
    }


}