using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genetic : MonoBehaviour
{
    public const int NUMBER_OF_GENERATION_SAMPLES = 50;
    public const float MUTATION_PERCENTAGE = 0.5f;

    [SerializeField]
    private GameObject model;

    private GameObject best_sample;
    private GameObject[] samples;


    void Awake()
    {
        samples = new GameObject[NUMBER_OF_GENERATION_SAMPLES];
    }


    public void GenerateRandomGeneration()
    {
        Net net;

        for (int i = 0; i < NUMBER_OF_GENERATION_SAMPLES; i++)
        {
            net = new Net(
                MindConstants.NUMBER_OF_INPUT_NODES,
                MindConstants.NUMBER_OF_OUTPUT_NODES,
                MindConstants.NUMBER_OF_HIDDEN_NODES,
				MindConstants.NUMBER_OF_HIDDEN_LAYERS
				);
            net.InitializeRandom();
            InstantiateWithNet(i, net);
        }
    }

    public void GenerateGenerationFromModel(string file)
    {
        Net net = new Net(
            MindConstants.NUMBER_OF_INPUT_NODES, 
            MindConstants.NUMBER_OF_OUTPUT_NODES, 
            MindConstants.NUMBER_OF_HIDDEN_NODES,
			MindConstants.NUMBER_OF_HIDDEN_LAYERS
			);
        net.LoadFromCSV(file);
        best_sample = Instantiate(model);
        best_sample.GetComponentInChildren<NetInterface>().Mind = net;
        NetInterface best = best_sample.GetComponentInChildren<NetInterface>();
        samples[0] = best_sample;

        net = null;
        for (int i = 1; i < Mathf.FloorToInt(NUMBER_OF_GENERATION_SAMPLES * MUTATION_PERCENTAGE); i++)
        {
            net = best.Mind.Clone();
            net.Mutate();
            InstantiateWithNet(i, net);
        }
        for (int i = Mathf.FloorToInt(NUMBER_OF_GENERATION_SAMPLES * MUTATION_PERCENTAGE); i < NUMBER_OF_GENERATION_SAMPLES; i++)
        {
            net = new Net(
                MindConstants.NUMBER_OF_INPUT_NODES,
                MindConstants.NUMBER_OF_OUTPUT_NODES,
                MindConstants.NUMBER_OF_HIDDEN_NODES,
				MindConstants.NUMBER_OF_HIDDEN_LAYERS
				);
            net.InitializeRandom();
            InstantiateWithNet(i, net);
        }
    }

    public void NextGeneration()
    {
        UpdateBestSample();

        Net best = best_sample.GetComponentInChildren<NetInterface>().Mind.Clone();
        int id = best_sample.GetComponentInChildren<NetInterface>().Id;
        float score = best_sample.GetComponentInChildren<NetInterface>().Score;
        Destroy(samples[0]);
        InstantiateWithNetAndScore(0, best, score, id);

        Net net;
        for (int i = 1; i < Mathf.FloorToInt(NUMBER_OF_GENERATION_SAMPLES * MUTATION_PERCENTAGE); i++)
        {
            net = best.Clone();
            net.Mutate();
            Destroy(samples[i]);
            InstantiateWithNet(i, net);
        }
        for (int i = Mathf.FloorToInt(NUMBER_OF_GENERATION_SAMPLES * MUTATION_PERCENTAGE); i < NUMBER_OF_GENERATION_SAMPLES; i++)
        {
            net = new Net(
                MindConstants.NUMBER_OF_INPUT_NODES,
                MindConstants.NUMBER_OF_OUTPUT_NODES,
                MindConstants.NUMBER_OF_HIDDEN_NODES,
				MindConstants.NUMBER_OF_HIDDEN_LAYERS
				);
            net.InitializeRandom();
            Destroy(samples[i]);
            InstantiateWithNet(i, net);
        }
    }

    public void UpdateBestSample()
    {
        GameObject sample;
        for (int i = 0; i < NUMBER_OF_GENERATION_SAMPLES; i++)
        {
            sample = samples[i];
            if (best_sample == null 
                || best_sample.GetComponentInChildren<NetInterface>().Score < sample.GetComponentInChildren<NetInterface>().Score)
            {
                best_sample = sample;
            }
        }
    }

    public void SaveBestSample(string file)
    {
        best_sample.GetComponentInChildren<NetInterface>().Mind.SaveToCSV(file);
    }


    private void InstantiateWithNet(int index, Net net)
    {
        samples[index] = Instantiate(model);
        samples[index].SetActive(false);
        samples[index].GetComponentInChildren<NetInterface>().Initialize(net);
    }

    private void InstantiateWithNetAndScore(int index, Net net, float score, int id)
    {
        samples[index] = Instantiate(model);
        samples[index].SetActive(false);
        samples[index].GetComponentInChildren<NetInterface>().Initialize(net, score, id);
    }

    public GameObject Best_sample { get => best_sample; set => best_sample = value; }
    public GameObject[] Samples { get => samples; set => samples = value; }
}
