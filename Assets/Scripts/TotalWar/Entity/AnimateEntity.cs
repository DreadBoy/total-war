using UnityEngine;

namespace TotalWar.Entity
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Entity))]
    public class AnimateEntity : MonoBehaviour
    {
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void Idle()
        {
            _animator.SetBool("run", false);
            _animator.SetBool("ready", false);
        }

        public void Run()
        {
            _animator.SetBool("run", true);
            _animator.SetBool("ready", false);
        }

        public void Aim()
        {
            _animator.SetBool("run", false);
            _animator.SetBool("ready", true);
        }

        public void Shoot()
        {
            _animator.SetBool("run", false);
            _animator.SetTrigger("shoot");
        }
    }
}