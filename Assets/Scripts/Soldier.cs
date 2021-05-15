using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Soldier : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private void Reset()
    {
        animator = GetComponent<Animator>();
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

}