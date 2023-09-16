
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
                Debug.Log($"Setting new target destination at {target.GetPosition()}");
                agent.SetDestination(target.GetPosition());
            }
        }
    }

    void GetNewTarget()
    {
        // Target is randomly selected from all players currently in the room
        VRCPlayerApi[] players = GameStateManager.GetAllPlayers();

        // Count the number of non-null elements in the array for the purpose of defining the range of random numbers
        int nonNullCount = -1;
        for (int i = 0; i < players.Length; i++)
        {
            Debug.Log($"Getting new target. On index {i}. Null player at this index: {players[i] == null}");
            if (players[i] != null)
            {
                nonNullCount++;
            }
            else
            {
                break;
            }
        }
        Debug.Log($"Non-null count is: {nonNullCount}");

        // Check for non-null elements
        if (nonNullCount != -1)
        {
            // Randomly select a player in the array of all players in room
            VRCPlayerApi randomPlayer = players[Random.Range(0, nonNullCount)];

            // Set the target to the randomly selected target
            target = randomPlayer;
        } 
        else
        {
            Debug.Log("Could not find player to assign to a new target");
        }
        
    }
}
