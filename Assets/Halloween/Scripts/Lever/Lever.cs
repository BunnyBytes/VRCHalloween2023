
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

public class Lever : UdonSharpBehaviour
{
    [SerializeField] GameStateManager GameStateManager;
    [SerializeField] GameObject LeverRespawn;
    [SerializeField] GameObject gate;

    // The lever handle that the player will pick up and bring to this object to trigger the event
    [SerializeField] GameObject leverHandlePickup;
    // The lever handle which will be enabled to show the puzzle has been solved
    [SerializeField] GameObject leverHandleLocal;

    private void Start()
    {
        DisableInteractive = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            Debug.Log($"{gameObject.name} colliding with: {other.name}");
            if (leverHandlePickup != null && leverHandlePickup == other.gameObject)
            {
                Debug.Log("Colliding with the handle!");
                leverHandlePickup.SetActive(false);
                leverHandleLocal.SetActive(true);

                // Start animation
                
                // Change spawn point
                GameStateManager.SpawnPoint = LeverRespawn.transform.position;
                SendCustomNetworkEvent(NetworkEventTarget.All, "SetEnableInteractive");
            }
        }
    }

    public override void Interact()
    {
        SendCustomNetworkEvent(NetworkEventTarget.All, "DisableGate");
    }

    public void SetEnableInteractive()
    {
        DisableInteractive = false;
    }

    public void DisableGate()
    {
        DisableInteractive = true;
        gate.SetActive(false);
    }
}
