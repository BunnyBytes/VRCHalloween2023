
using UdonSharp;
using UnityEngine;
using UnityEngine.AI;
using VRC.SDKBase;
using VRC.Udon;

public class Snail : UdonSharpBehaviour
{
    [SerializeField] NavMeshAgent agent;

    private void Update()
    {
        VRCPlayerApi target = Networking.LocalPlayer;

        agent.SetDestination(target.GetPosition());
    }
}
