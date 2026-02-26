using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class AITrainer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txt_timer;
    [SerializeField] TextMeshProUGUI txt_aliveCount;
    [SerializeField] TextMeshProUGUI txt_generation;

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
        
        txt_timer.text = "Time: " + timer.ToString("F2") + "s";
        if (timer >= episodeTime)
        {
            ResetAllAgents();
        }
        UpdateAliveCount();
    }

    void ResetAllAgents()
    {
        timer = 0;
        generationCount++;
        txt_generation.text = "Generation: " + generationCount;

        foreach (AgentAI agent in agents)
        {
            agent.EndEpisode();
        }
    }

    public void UpdateAliveCount() 
    {
        int aliveCount = 0;
        foreach (AgentAI agent in agents)
        {
            if (agent.gameObject.activeSelf)
                aliveCount++;
        }
        txt_aliveCount.text = "Alive: " + aliveCount;
    }


}