using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MazePopulation : MonoBehaviour
{
    public GameObject botPrefab;
    public int populationSize = 20;

    public float elapsed { get; private set; }

    List<Brain> population = new List<Brain>();
    int generation = 0;
    public float trialTime = 10;

    GUIStyle guiStyle = new GUIStyle();

    private void OnGUI()
    {
        guiStyle.fontSize = 25;
        guiStyle.normal.textColor = Color.white;
        GUI.BeginGroup(new Rect(10, 10, 250, 150));
        GUI.Box(new Rect(0, 0, 140, 140), "Stats", guiStyle);
        GUI.Label(new Rect(10, 25, 200, 30), "Gen " + generation, guiStyle);
        GUI.Label(new Rect(10, 50, 200, 30), string.Format("Time {0:0.00}", elapsed), guiStyle);
        GUI.Label(new Rect(10, 75, 200, 30), "Population " + population.Count, guiStyle);
        GUI.EndGroup();
    }

    // Use this for initialization
    void Start()
    {
        BreedNewPopulation();
    }

    Brain Breed(Brain a, Brain b)
    {
        var startPos = new Vector3(this.transform.position.x + Random.Range(-2, 2)
            , this.transform.position.y
            , this.transform.position.z + Random.Range(-2, 2));

        var newBrain = Instantiate(botPrefab, startPos, botPrefab.transform.rotation).GetComponent<Brain>();
        newBrain.Init(2);

        if (Random.Range(0, 100) == 1)
        {
            newBrain.dna.Mutate();
        }
        else
        {
            newBrain.dna.Combine(a.dna, b.dna);
        }

        return newBrain;
    }

    void BreedNewPopulation()
    {
        elapsed = 0;
        generation++;

        if (population.Count < 2)
        {
            BreedRandom();
            return;
        }

        var sortedPopulation = population.OrderByDescending(o => o.maxDistanceFromStart).ToList();
        population.Add(Breed(sortedPopulation[0], sortedPopulation[1]));

        if (population.Count > populationSize)
        {
            var worstBrain = sortedPopulation[sortedPopulation.Count - 1];
            population.Remove(worstBrain);
            Destroy(worstBrain.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= trialTime)
        {
            BreedNewPopulation();
        }
    }

    void BreedRandom()
    {
        var startPos = this.transform.position;

        var newBrain = Instantiate(botPrefab, startPos, botPrefab.transform.rotation).GetComponent<Brain>();
        newBrain.Init(2);
        population.Add(newBrain);
    }
}
