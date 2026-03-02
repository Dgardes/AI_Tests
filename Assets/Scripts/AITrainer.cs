using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class AITrainer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txt_timer;
    [SerializeField] TextMeshProUGUI txt_aliveCount;
    [SerializeField] TextMeshProUGUI txt_generation;

    private int aliveCount;
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

    
    private float episodeTime = 30f;
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
        if (timer >= episodeTime || aliveCount == 0)
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
            if (agent.gameObject.activeSelf)
            { 
                agent.AddReward(-1.0f); 
                agent.EndEpisode();
    }
            agent.gameObject.SetActive(true);
        }
    }

    public void UpdateAliveCount() 
    {
        int alive = 0;
        foreach (AgentAI agent in agents)
        {
            if (agent.gameObject.activeSelf)
                alive++;
        }
        aliveCount = alive;
        txt_aliveCount.text = "Alive: " + aliveCount;
    }


}