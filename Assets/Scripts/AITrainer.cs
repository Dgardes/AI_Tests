using UnityEngine;
using System.Collections.Generic;
public class AITrainer : MonoBehaviour
{
    public static AITrainer Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }

    public List<AgentAI> agents;

    /*
    private float episodeTime = 60f;
    private float timer;
    private int generationCount = 0;

    void Start()
    {
        ResetAllAgents();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= episodeTime)
        {
            ResetAllAgents();
        }
    }

    void ResetAllAgents()
    {
        timer = 0;
        generationCount++;

        foreach (AgentAI agent in agents)
        {
            agent.EndEpisode();
        }
    }

    */
}