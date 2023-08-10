using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    public float range;
    public Transform centrePoint;

    private Animator animator;
    private bool isMoving = false;
    private Vector3 lastPosition;
    private float idleTimer = 0f;
    private float idleThreshold = 2f;
    private bool canGenerateDestination = true;
    private float destinationCooldown = 9f;
    private float destinationTimer = 0f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        lastPosition = transform.position;
    }

    private void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (canGenerateDestination)
            {
                Vector3 point;
                if (RandomPoint(centrePoint.position, range, out point))
                {
		    isMoving = true;
		    animator.SetBool("IsMoving", isMoving);
                    Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                    agent.SetDestination(point);
                    canGenerateDestination = false;
                    destinationTimer = 0f;
                }
            }
        }

        if (Mathf.Abs(agent.velocity.magnitude) > 0.1f)
        {
            isMoving = true;
            idleTimer = 0f;
	    animator.SetBool("IsMoving", isMoving);

            // Calcula la dirección hacia la cual se mueve el personaje
            Vector3 direction = agent.velocity.normalized;
            direction.y = 0f; // Asegura que la rotación solo se realice en el plano horizontal

            // Rota el personaje hacia la dirección del movimiento
            if (direction != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, agent.angularSpeed * Time.deltaTime);
            }
        }
        else
        {
            isMoving = false;
	    animator.SetBool("IsMoving", isMoving);
            idleTimer += Time.deltaTime;
        }

        

        if (!canGenerateDestination)
        {
            destinationTimer += Time.deltaTime;
            if (destinationTimer >= destinationCooldown)
            {
                canGenerateDestination = true;
            }
        }
        lastPosition = transform.position;
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
}


