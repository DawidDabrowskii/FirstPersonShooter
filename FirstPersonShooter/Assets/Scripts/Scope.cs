using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Scope : MonoBehaviour
{
    private bool isScoped = false;
    public Animator animator;
    public GameObject scopeOverlay;

    InputAction scope;
    void Start()
    {
        scope = new InputAction("Scope", binding:"<mouse>/rightButton");
        scope.Enable();
    }

    void Update()
    {
        SniperScope();
    }

    private void SniperScope()
    {
        if (scope.triggered)
        {
            isScoped = !isScoped;
            animator.SetBool("isScoped", isScoped);
            if(isScoped)
            {
                scopeOverlay.SetActive(true);
            }
            else
            {
                scopeOverlay.SetActive(false);
            }

        }
    }
}
