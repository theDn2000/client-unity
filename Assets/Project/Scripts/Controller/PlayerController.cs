using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;


public class PlayerController : MonoBehaviour
{
    const string IDLE = "Idle";
    const string RUN_FORWARD = "RunForward";


CustomActions input;
NavMeshAgent agent;
Animator animator;

    [Header("Movement")]
    [SerializeField] ParticleSystem clickEffect;
    [SerializeField] LayerMask clickableLayer;

    float lookRotaionSpeed = 25f;

    // Click effect para el clic del ratón
    [Space][SerializeField] private Renderer ClickIcon;
    private static readonly int CLICK_TIME_PROPERTY = Shader.PropertyToID("_ClickTime");

    void Awake()
    {
        input = new CustomActions();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Configurar rotación del NavMeshAgent
        agent.updateRotation = true;
        agent.angularSpeed = 720f; // Velocidad de rotación en grados por segundo
        agent.autoBraking = false;

        AssignInputs();
    }

    void AssignInputs()
    {
        input.Main.Move.performed += ctx => ClickToMove();
    }

    void ClickToMove()
    {
        RaycastHit hit;
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        
        if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePosition), out hit, 100f, clickableLayer))
        {
            // Verificar si el punto está en una zona navegable
            NavMeshHit navHit;
            if (NavMesh.SamplePosition(hit.point, out navHit, 1.0f, NavMesh.AllAreas))
            {
                agent.destination = navHit.position;

                if (clickEffect != null)
                {
                    Instantiate(clickEffect, navHit.position + new Vector3(0, 0.1f, 0), clickEffect.transform.rotation);
                }
            }
        }
    }

    void OnEnable()
    {
        input.Enable();
    }

    void OnDisable()
    {
        input.Disable();
    }

    void Update()
    {
        // Permitir movimiento manteniendo el botón derecho del ratón
        if (Mouse.current.rightButton.isPressed)
        {
            MoveToMousePosition();
        }

        // Activar animación de click solo al soltar el botón derecho
        if (Mouse.current.rightButton.wasReleasedThisFrame)
        {
            ShowClickEffectAtMousePosition();
        }

        FaceTarget();
        SetAnimations();
    }

    // Nuevo método para mover al agente hacia la posición bajo el cursor
    void MoveToMousePosition()
    {
        RaycastHit hit;
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        
        if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePosition), out hit, 100f, clickableLayer))
        {
            // Verificar si el punto está en una zona navegable
            NavMeshHit navHit;
            if (NavMesh.SamplePosition(hit.point, out navHit, 1.0f, NavMesh.AllAreas))
            {
                agent.destination = navHit.position;
            }
        }
    }

    // Click effect function
    void ShowClickEffectAtMousePosition()
    {
        RaycastHit hit;
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        
        if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePosition), out hit, 100f, clickableLayer))
        {
            // Verificar si el punto está en una zona navegable
            NavMeshHit navHit;
            if (NavMesh.SamplePosition(hit.point, out navHit, 1.0f, NavMesh.AllAreas))
            {
                ClickIcon.transform.position = navHit.position + new Vector3(0, 0.1f, 0);
                ClickIcon.material.SetFloat(CLICK_TIME_PROPERTY, Time.time);
            }
        }
    }

    void FaceTarget()
    {
        if (agent.hasPath && agent.remainingDistance > agent.stoppingDistance)
        {
            Vector3 direction = agent.steeringTarget - transform.position;
            direction.y = 0; // Ignorar la altura
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lookRotaionSpeed * Time.deltaTime);
        }
    }

    void SetAnimations()
    {
        if (agent.velocity == Vector3.zero)
        {
            animator.SetBool("RunForward", false);
            animator.SetBool("Idle", true);
        }
        else
        {
            animator.SetBool("Idle", false);
            animator.SetBool("RunForward", true);
        }
    }
}
