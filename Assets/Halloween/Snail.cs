
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
        //target = GameStateManager.playerIds[Random.Range(0, GameStateManager.playerIds.Count)];
        int randomTargetId = GameStateManager.playerIds[Random.Range(0, GameStateManager.playerIds.Count - 1)];
        target = VRCPlayerApi.GetPlayerById(randomTargetId);
        if (target != null)
        {
            Debug.Log($"Setting snail target to player {target.displayName} with id of {randomTargetId}");
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
