using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopulationManager : MonoBehaviour
{
    public GameObject personPrefab;
    public int populationSize = 10;
    List<GameObject> population = new List<GameObject>();//ana pop�lasyon
    public static float elapsed=0;//for test??
    int trialTime = 10;//generation maxs time
    int generation = 1;

    GUIStyle guiStyle = new GUIStyle();
    void OnGUI()
    {

        guiStyle.fontSize = 50;
        guiStyle.normal.textColor=Color.white;
        GUI.Label(new Rect(10, 10, 100, 20), "Generation: " + generation, guiStyle);
        GUI.Label(new Rect(10, 65, 100, 20), "Trial time " + (int)elapsed, guiStyle);
    }
    void Start()
    {

        for(int i = 0; i < populationSize; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-9, 9), Random.Range(-4.5f, 4.5f), 0);
            //random position on the screen
            GameObject go = Instantiate(personPrefab, pos, Quaternion.identity);
            //Instantiate metodu obje olu�turmaya yarar. Yani istedi�imiz bir noktada istedi�imiz bir objeyi olu�turmam�za yarar.
            //Instantiate metodu 3 adet parametre al�r. Bu parametrelerden ilki �retilecek objemizi al�r.
            //�kinci parametre �retilecek objemizin hangi konumda �retilece�ini al�r.
            //���nc� parametre ise objemizin do�rultusunu belirler.
            go.GetComponent<DNA>().r = Random.Range(0.0f, 1.0f);
            go.GetComponent<DNA>().b = Random.Range(0.0f, 1.0f);
            go.GetComponent<DNA>().b = Random.Range(0.0f, 1.0f);
            population.Add(go);
        }

    }

    GameObject Breed(GameObject parent1,GameObject parent2)
    {
        Vector3 pos = new Vector3(Random.Range(-9, 9), Random.Range(-4.5f, 4.5f), 0);
        GameObject offspring = Instantiate(personPrefab, pos, Quaternion.identity);
        //yukar�da a��klamas� var
        DNA dna1 = parent1.GetComponent<DNA>();
        DNA dna2 = parent2.GetComponent<DNA>();
        //bu sefer random de�er atamak yerine parentlerin renklerinden al�yolar

        //swap parent dna
        offspring.GetComponent<DNA>().r = Random.Range(0, 10) < 5 ? dna1.r : dna2.r;
        offspring.GetComponent<DNA>().g = Random.Range(0, 10) < 5 ? dna1.g : dna2.g;
        offspring.GetComponent<DNA>().b = Random.Range(0, 10) < 5 ? dna1.b : dna2.b;
        //yar� yar�ya �ans� var e�er random say� 10dan k���kse dna1in rengini al�r de�ilse dna2 nin rengini al�r
        return offspring;
    }

    void BreedNewPopulation()
    {
        List<GameObject> newPopulation = new List<GameObject>();//yavru generation
        List<GameObject> sortedList = population.OrderBy(o => o.GetComponent<DNA>().timeToDie).ToList();//b�y�kten k����e listeye atar
           population.Clear();

        for (int i = (int)(sortedList.Count / 2.0f) ; i < sortedList.Count - 1; i++)//generationdaki person say�s�n� yar�ya b�ler
        {
            for(int j = (int)(sortedList.Count / 2.0f) + 1; i < sortedList.Count; i++)
            {
                population.Add(Breed(sortedList[i], sortedList[j]));
                population.Add(Breed(sortedList[j], sortedList[i]));
            }
            
        }

        for(int i = 0; i < sortedList.Count; i++)
        {
            Destroy(sortedList[i]);
        }
        generation++;
    }

    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed > trialTime)//game time controll
        {
            BreedNewPopulation();
            elapsed = 0;
        }
    }
}
