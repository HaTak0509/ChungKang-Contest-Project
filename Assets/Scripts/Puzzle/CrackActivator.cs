using UnityEngine;

public class CrackActivator : MonoBehaviour, IInteractable
{
    [SerializeField] private Crack Crack;

    public void Interact()
    {
        Debug.Log(123);
        Crack.SetCrack(!Crack.isActivated);
    }
}
