
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
                Debug.Log("Snail target is null. Getting a new target");
                GetNewTarget();
            }
            else
            {
                Debug.Log("Setting new agent location");
                SetNewAgentLocation();
            }
        }
    }

    void GetNewTarget()
    {
        DataList playerIds = GameStateManager.GetAllPlayerIds();
        int randomRange = playerIds.Count - 1;


        // From the DataList of playerIds, choose one at random
        DataToken newTargetToken = playerIds[Random.Range(0, randomRange)];

        // Check if the types are appropriate
        if (!newTargetToken.IsNull && newTargetToken.TokenType == TokenType.Int)
        {
            VRCPlayerApi newPlayerTarget = VRCPlayerApi.GetPlayerById(newTargetToken.Int);

            if (newPlayerTarget != null)
            {
                target = newPlayerTarget;
            }
        }
    }

    void SetNewAgentLocation()
    {
        Vector3 newTargetLocation = target.GetPosition() != null ? target.GetPosition() : Vector3.zero;

        if (newTargetLocation != null && newTargetLocation != Vector3.zero)
        {
            Debug.Log($"Setting new target destination at {newTargetLocation}");
            agent.SetDestination(newTargetLocation);
        }
        else
        {
            Debug.Log("Unable to set agent position to new target position");
        }
    }
}
