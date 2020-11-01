﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class PopulationManager : MonoBehaviour
{
    public GameObject botPrefab;
    public int populationSize = 50;
    List<GameObject> population = new List<GameObject>();
    public static float elapsed = 0;
    public float trialTime = 5;
    int generation = 1;

    GUIStyle guiStyle = new GUIStyle();
    private void OnGUI()
    {
        guiStyle.fontSize = 25;
        guiStyle.normal.textColor = Color.white;
        GUI.BeginGroup(new Rect(10, 10, 250, 150));
        GUI.Box(new Rect(0, 0, 140, 140), "Stats", guiStyle);
        GUI.Label(new Rect(10, 25, 200, 30), "Generation: " + generation, guiStyle);
        GUI.Label(new Rect(10, 50, 200, 30), string.Format("Time: {0:0.00}", elapsed), guiStyle);
        GUI.Label(new Rect(10, 75, 200, 30), "Population: " + population.Count, guiStyle);
        GUI.EndGroup();
    }

    void Start()
    {
        for (int i = 0; i < populationSize; i++)
        {
            Vector3 pos = new Vector3(this.transform.position.x + Random.Range(-2, 2), this.transform.position.y,
                this.transform.position.z + Random.Range(-2, 2));
            GameObject obj = Instantiate(botPrefab, pos, this.transform.rotation);
            obj.GetComponent<Brain>().Init();
            population.Add(obj);

        }
    }
    GameObject Breed(GameObject mom, GameObject dad)
    {
        Vector3 pos = new Vector3(this.transform.position.x + Random.Range(-2, 2), this.transform.position.y,
                this.transform.position.z + Random.Range(-2, 2));
        GameObject obj = Instantiate(botPrefab, pos, this.transform.rotation);
        Brain b = obj.GetComponent<Brain>();
        if (Random.Range(0, 100) == 1)
        {
            b.Init();
            b.dna.Mutate();
        }
        else
        {
            b.Init();
            b.dna.Combine(mom.GetComponent<Brain>().dna, dad.GetComponent<Brain>().dna);
        }
        return obj;
    }
    void BreedNewPopulation()
    {
        List<GameObject> sortedList = population.OrderBy(o => (o.GetComponent<Brain>().timeWalking*5+ o.GetComponent<Brain>().timeAlive)).ToList();
        population.Clear();
        for (int i = (int)(sortedList.Count / 2.0f) - 1; i < sortedList.Count - 1; i++)
        {
            population.Add(Breed(sortedList[i], sortedList[i + 1]));
            population.Add(Breed(sortedList[i + 1], sortedList[i]));
        }
        for (int i = 0; i < sortedList.Count; i++)
        {
            Destroy(sortedList[i]);
        }
        generation++;
    }

    void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= trialTime)
        {
            BreedNewPopulation();
            elapsed = 0;
        }

    }
}
