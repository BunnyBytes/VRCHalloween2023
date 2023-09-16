
using UdonSharp;
using UnityEngine;
using UnityEngine.AI;
using VRC.SDK3.Data;
using VRC.SDKBase;

public class Snail : UdonSharpBehaviour
{
    [SerializeField] GameStateManager GameStateManager;
    [SerializeField] NavMeshAgent agent;
    VRCPlayerApi target;

    private void Update()
    {
        if (Networking.IsOwner(gameObject))
        {
            if (target == null)
            {
                GetNewTarget();
            }
            else
            {
                agent.SetDestination(target.GetPosition());
            }
        }

    }

    void GetNewTarget()
    {
        if (GameStateManager.playerIds.Count > 0)
        {
            DataToken randomTargetId = GameStateManager.playerIds[Random.Range(0, GameStateManager.playerIds.Count - 1)];
            if (randomTargetId.TokenType == TokenType.Int)
            {
                target = VRCPlayerApi.GetPlayerById(randomTargetId.Int);
                if (target != null)
                {
                    Debug.Log($"Setting snail target to player {target.displayName} with id of {randomTargetId}");
                    
                }
            }
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
