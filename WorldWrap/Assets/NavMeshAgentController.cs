using UnityEngine.AI;
using UnityEngine;

public class NavMeshAgentController : MonoBehaviour
{
    [SerializeField] private GameObject lure;
    private UnityEngine.AI.NavMeshAgent navmeshAgent;

    void Start()
    {
        navmeshAgent = gameObject.GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        navmeshAgent.SetDestination(lure.transform.position);
    }
}
