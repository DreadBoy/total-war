using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class Soldier : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private EntityHighlight entityHighlight;
    [SerializeField] private Unit unit;

    private IEnumerator _goTo;

    private void Reset()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        entityHighlight = GetComponentInChildren<EntityHighlight>();
    }

    private void Awake()
    {
        unit = GetComponentInParent<Unit>();
    }

    public void Idle()
    {
        animator.SetBool("run", false);
        animator.SetBool("ready", false);
    }

    public void Run()
    {
        animator.SetBool("run", true);
        animator.SetBool("ready", false);
    }

    public void Aim()
    {
        animator.SetBool("run", false);
        animator.SetBool("ready", true);
    }

    public void Shoot()
    {
        animator.SetBool("run", false);
        animator.SetTrigger("shoot");
    }

    private void LookForward()
    {
        transform.forward = unit.transform.forward;
    }

    private IEnumerator GoToAnimated(Vector3 globalPosition)
    {
        navMeshAgent.destination = globalPosition;

        Run();

        while (true)
        {
            yield return new WaitForFixedUpdate();
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
                break;
        }

        WarpTo(globalPosition);

        Idle();
        LookForward();
        _goTo = null;
    }

    public void WarpTo(Vector3 globalPosition)
    {
        navMeshAgent.Warp(globalPosition);
    }

    public void GoTo(Vector3 globalPosition)
    {
        if (_goTo != null)
            StopCoroutine(_goTo);
        _goTo = GoToAnimated(globalPosition);
        StartCoroutine(_goTo);
    }

    public bool IsHighlighted
    {
        get => entityHighlight.IsHighlighted;
        set => entityHighlight.IsHighlighted = value;
    }
}