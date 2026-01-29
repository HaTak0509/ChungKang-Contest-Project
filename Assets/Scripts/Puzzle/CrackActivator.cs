using UnityEngine;

public class CrackActivator : MonoBehaviour, IInteractable
{
    [SerializeField] private Crack Crack;

    public void Interact()
    {
        Crack.SetCrack(!Crack.isActivated);
    }
}
