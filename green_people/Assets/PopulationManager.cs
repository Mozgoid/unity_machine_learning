using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopulationManager : MonoBehaviour
{
    public static float elapsed = 0;

    public GameObject personPrefab;

    private const int populationSize = 10;
    private List<DNA> population = new List<DNA>();
    private int generation = 0;
    private const int trialTime = 10;

    private GUIStyle guiStyle = new GUIStyle();

    private void OnGUI()
    {
        guiStyle.fontSize = 50;
        guiStyle.normal.textColor = Color.white;
        GUI.Label(new Rect(10, 10, 100, 20), string.Format("Generation: {0}", generation), guiStyle);
        GUI.Label(new Rect(10, 65, 100, 20), string.Format("Trial time {0}", elapsed),  guiStyle);
    }


    // Use this for initialization
    void Start ()
    {
        for (int i = 0; i < populationSize; i++)
        {
            var randomScreenPosition = new Vector3(Random.Range(-8, 8), Random.Range(-4, 5), 0);
            var dna = Instantiate(personPrefab, randomScreenPosition, Quaternion.identity).GetComponent<DNA>();
            dna.color = new Color(Random.value, Random.value, Random.value);
            dna.scale = Random.Range(0.5f, 1.5f);
            population.Add(dna);
        }
    }

    // Update is called once per frame
    void Update ()
    {
        elapsed += Time.deltaTime;
        if (elapsed > trialTime)
        {
            BreadNewPopulation();
        }
    }

    void BreadNewPopulation()
    {
        elapsed = 0;
        generation++;

        var sortedPopulation = population.OrderByDescending(dna => dna.timeToDeath).ToList();
        population.Clear();

        for (int i = sortedPopulation.Count / 2 - 1; i < sortedPopulation.Count - 1; i++)
        {
            population.Add(Bread(sortedPopulation[i], sortedPopulation[i + 1]));
            population.Add(Bread(sortedPopulation[i + 1], sortedPopulation[i]));
        }

        foreach (var dna in sortedPopulation)
        {
            Destroy(dna.gameObject);
        }
    }

    DNA Bread(DNA a, DNA b)
    {
        var randomScreenPosition = new Vector3(Random.Range(-8, 8), Random.Range(-4, 5), 0);
        var dna = Instantiate(personPrefab, randomScreenPosition, Quaternion.identity).GetComponent<DNA>();

        var newColor = Color.white;
        newColor.r = Random.value > 0.5f ? a.color.r : b.color.r;
        newColor.g = Random.value > 0.5f ? a.color.g : b.color.g;
        newColor.b = Random.value > 0.5f ? a.color.b : b.color.b;

        var newScale = Random.value > 0.5f ? a.scale : b.scale;

        //mutations
        if (Random.value < 0.05)
        {
            newColor.r = Random.value;
        }
        if (Random.value < 0.05)
        {
            newColor.g = Random.value;
        }
        if (Random.value < 0.05)
        {
            newColor.b = Random.value;
        }
        if (Random.value < 0.05)
        {
            newScale = Random.Range(0.5f, 1.5f);
        }

        dna.color = newColor;
        dna.scale = newScale;
 
        return dna;
    }
}
