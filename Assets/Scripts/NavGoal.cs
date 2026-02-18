using UnityEngine;
using UnityEngine.AI; //для работы с навигацией

public class NavGoal :MonoBehaviour
{
    [Header("Настройки цели")]
    public Transform target;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        //отключаем вращение по осям X и Y, чтобы спрайт не падал 
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        //если цель назначена, приказываем агенту идти к ней
        if(target != null)
        {
            agent.SetDestination(target.position);
        }
    }
}