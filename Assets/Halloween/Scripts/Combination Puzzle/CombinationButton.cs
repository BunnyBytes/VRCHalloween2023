
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class CombinationButton : UdonSharpBehaviour
{
    [SerializeField] CombinationPuzzle combination;
    [SerializeField] bool increase;
    [SerializeField] int index;

    public override void Interact()
    {
        combination.ChangeCombinationDigit(index, increase);
    }
}
