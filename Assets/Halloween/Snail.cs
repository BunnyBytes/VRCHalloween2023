
using UdonSharp;
using UnityEngine;
using UnityEngine.AI;
using VRC.SDKBase;

public class Snail : UdonSharpBehaviour
{
    [SerializeField] GameStateManager GameStateManager;
    [SerializeField] NavMeshAgent agent;
    VRCPlayerApi target;

    private void Start()
    {
        if (Networking.IsOwner(gameObject) && target == null)
        {
            GetNewTarget();
        }
    }

    void GetNewTarget()
    {
        target = GameStateManager.players[Random.Range(0, GameStateManager.players.Count)];
        if (target != null)
        {
            agent.SetDestination(target.GetPosition());
        }
    }

    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        if (player == target)
        {
            GetNewTarget();
        }
    }
}
