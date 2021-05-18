using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Soldier : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody rigidBody;

    private const float TimeToMove = 1f;

    private void Reset()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
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

    public void CheckYourPosition(Vector3[] localPositions, float unitSize)
    {
        var pressure = LookForOpening(localPositions, unitSize);
        pressure += AreYouOutside(localPositions, unitSize);
        Debug.DrawLine(transform.position, transform.TransformPoint(pressure + Vector3.up), Color.green, 600);
        if (pressure == Vector3.zero) return;
        
        StartCoroutine(Move(transform.localPosition + pressure, TimeToMove));
    }

    private Vector3 LookForOpening(Vector3[] localPositions, float unitSize)
    {
        var localPosition = transform.localPosition;
        var forwardSpotExists = IsIncluded(localPosition + Vector3.forward * unitSize, localPositions);
        var forwardSpotEmpty = !IsSoldierThere(localPosition + Vector3.forward * unitSize);
        if (forwardSpotExists && forwardSpotEmpty)
            return Vector3.forward * unitSize;
        return Vector3.zero;
    }

    private Vector3 AreYouOutside(Vector3[] localPositions, float unitSize)
    {
        var localPosition = transform.localPosition;
        var isInside = IsIncluded(localPosition, localPositions);
        if (isInside) return Vector3.zero;

        var sides = new Vector3[] {Vector3.left * unitSize, Vector3.right * unitSize};
        foreach (var side in sides)
        {
            var isValid = IsIncluded(localPosition + side, localPositions); 
            var isEmpty = !IsSoldierThere(localPosition + side);
            if (isValid && isEmpty) return side;
        }
        return Vector3.zero;
    }

    private bool IsIncluded(Vector3 localPosition, IEnumerable<Vector3> localPositions)
    {
        localPosition = Vector3.ProjectOnPlane(localPosition, Vector3.up);
        return localPositions
            .Select(p => Vector3.ProjectOnPlane(p, Vector3.up))
            .Any(p => Vector3.Distance(p, localPosition) < 0.5);
    }

    private bool IsSoldierThere(Vector3 localPosition)
    {
        localPosition = Vector3.ProjectOnPlane(localPosition, Vector3.up) + Vector3.up;
        var offset = localPosition - transform.localPosition;
        var globalPosition = transform.TransformPoint(offset);
        LayerMask mask = LayerMask.GetMask("Soldier");
        var occupied = Physics.CheckSphere(globalPosition, 0.3f, mask);
        // var color = Color.green;
        // if (occupied) color = Color.red;
        // Debug.DrawLine(transform.position, globalPosition, color, 1);
        return occupied;
    }
    IEnumerator Move(Vector3 localPosition, float targetTime)
    {
        var t = transform;
        var oldForward = t.forward;

        var start = t.localPosition;
        var end = localPosition;

        var currentTime = 0f;

        var forward = Vector3.ProjectOnPlane(end - start, Vector3.up);
        var forwardStep = forward.magnitude / targetTime;
        t.forward = forward;

        if (forward.magnitude > 0.1) Run();

        while (currentTime < targetTime)
        {
            currentTime += Time.deltaTime;
            t.Translate(forwardStep * Time.deltaTime * Vector3.forward, Space.Self);
            yield return new WaitForFixedUpdate();
        }

        t.forward = oldForward;
        t.localPosition = localPosition;

        Idle();
    }
}