
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;

public class Cube : UdonSharpBehaviour
{
    [SerializeField]
    AudioSource audio;
    [SerializeField]
    Text text;
    [SerializeField]
    Animator animator;
    [UdonSynced] int timesClicked = 0;

    public override void Interact()
    {
        // Transfer ownership to the interactor so the variable can be synced and RequestSerialization called
        Networking.SetOwner(Networking.LocalPlayer, gameObject);

        timesClicked++;
        RequestSerialization();
        DoTestStuff();
    }

    public void DoTestStuff()
    {
        text.text = timesClicked.ToString();
        animator.SetTrigger("Move");
        audio.Play();
    }
    public override void OnDeserialization()
    {
        DoTestStuff();
        Debug.Log("OnDeserialised called");
    }
}