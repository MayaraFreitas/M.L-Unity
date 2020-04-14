using System.Collections.Generic;
using UnityEngine;

public class DNA
{
    public List<Vector2> genes = new List<Vector2>();

    // Construtor acionado na primeira geração do DNA
    public DNA(int genomeLenght = 50)
    {
        for(int i = 0; i < genomeLenght; i++)
        {
            genes.Add(new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)));
        }
    }

    // Construtor acionado na segunda geração em diante do DNA
    public DNA(DNA parent, DNA partner, float mutationRate=0.01f)
    {
        for (int i = 0; i < parent.genes.Count; i++)
        {
            float mutationChance = Random.Range(0.0f, 1.0f);

            // Se a change de mutação foi 0,01 -> Criar um novo Gene
            if(mutationChance <= mutationRate)
            {
                genes.Add(new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)));
            }
            else
            {
                int chance = Random.Range(0, 2);

                // Se entre os números 0, 1 e 2 o zero for sorteado, utilizar o mesmo gene
                if(chance == 0)
                {
                    genes.Add(parent.genes[i]);
                }
                else // Caso contrário, utilizar um aleatório da listagem de sobreviventes
                {
                    genes.Add(partner.genes[i]);
                }
                
            }
        }
    }  
}
