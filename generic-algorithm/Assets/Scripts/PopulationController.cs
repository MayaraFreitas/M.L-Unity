using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationController : MonoBehaviour
{
    List<GeneticPathfinder> population = new List<GeneticPathfinder>();
    public GameObject creaturePrefab;
    public int populationSize = 100;
    public int genomeLenght;
    public float cutoff = 0.3f;
    public Transform spawnPoint;
    public Transform end;

    private void InitPopulation()
    {
        for(int  i= 0;  i < populationSize; i++)
        {
            // Criando particula
            GameObject go = Instantiate(creaturePrefab, spawnPoint.position, Quaternion.identity);
            
            go.GetComponent<GeneticPathfinder>().InitCreature(new DNA(genomeLenght), end.position);
            population.Add(go.GetComponent<GeneticPathfinder>());
        }
    }

    private void NextGeneration()
    {
        int survivorCut = Mathf.RoundToInt(populationSize * cutoff); // Definir quantidade de sobreviventes da geração
        List<GeneticPathfinder> survivors = new List<GeneticPathfinder>();
        for (int i = 0; i < survivorCut; i++)
        {
            // Obter os DNAs que chegaram mais próximos ao destino
            survivors.Add(GetFittest());
        }

        //Limpando DNAs não selecionados
        for (int i = 0; i < population.Count; i++)
        {
            Destroy(population[i].gameObject);
        }
        population.Clear();

        // A partir dos sobreviventes, criar uma nova geração com a mesma quantidade inicial
        while (population.Count < populationSize)
        {
            for (int i = 0; i < survivors.Count; i++)
            {
                GameObject go = Instantiate(creaturePrefab, spawnPoint.position, Quaternion.identity);
                go.GetComponent<GeneticPathfinder>().InitCreature(new DNA(survivors[i].dna, survivors[Random.Range(0, survivors.Count)].dna), end.position);
                population.Add(go.GetComponent<GeneticPathfinder>());
                if (population.Count >= populationSize)
                {
                    break;
                }
            }
        }
        // Limpar lista de sobreviventes
        for (int i = 0; i < survivors.Count; i++)
        {
            Destroy(survivors[i].gameObject);
        }
    }

    private void Start()
    {
        InitPopulation();
    }

    private void Update()
    {
        if (!HasActive()){
            NextGeneration();
        }  
    }

    private GeneticPathfinder GetFittest()
    {
        float maxFitness = float.MinValue;
        int index = 0;
        for (int i = 0; i < population.Count; i++)
        {
            if (population[i].fitness > maxFitness)
            {
                maxFitness = population[i].fitness;
                index = i;
            }
        }
        GeneticPathfinder fittest = population[index];
        population.Remove(fittest);
        return fittest;
    }
    private bool HasActive()
    {
        for (int i = 0; i < population.Count; i++)
        {
            if (!population[i].hasFinished)
            {
                return true;
            }
        }
        return false;
    }
}