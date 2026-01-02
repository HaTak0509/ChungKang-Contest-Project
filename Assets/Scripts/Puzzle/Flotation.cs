using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flotation : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float flotationForce;

    public bool action;

    private void Update()
    {
        if (action && !player)
        {

        }
    }
}
