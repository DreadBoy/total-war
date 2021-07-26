using System.Collections;
using TotalWar.Unit;
using UnityEngine;
using UnityEngine.AI;

namespace TotalWar.Entity
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Entity))]
    [RequireComponent(typeof(AnimateEntity))]
    public class MoveEntity : MonoBehaviour
    {
        private AnimateEntity _animateEntity;
        private NavMeshAgent _navMeshAgent;

        private IEnumerator _goTo;

        private Unit.Unit Unit => GetComponentInParent<Unit.Unit>();

        private void Awake()
        {
            _animateEntity = GetComponent<AnimateEntity>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        public void GoTo(Vector3 globalPosition)
        {
            if (_goTo != null)
                StopCoroutine(_goTo);
            _goTo = GoToAnimated(globalPosition);
            StartCoroutine(_goTo);
        }

        private IEnumerator GoToAnimated(Vector3 globalPosition)
        {
            _navMeshAgent.destination = globalPosition;

            _animateEntity.Run();

            while (true)
            {
                yield return new WaitForSeconds(0.5f);
                if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
                    break;
            }

            WarpTo(globalPosition);

            _animateEntity.Idle();
            LookForward();
            _goTo = null;
        }

        private void WarpTo(Vector3 globalPosition)
        {
            _navMeshAgent.Warp(globalPosition);
        }

        private void LookForward()
        {
            transform.forward = Unit.GetComponent<MoveUnit>().Forward;
        }

        private void Update()
        {
            Debug.DrawLine(transform.position, _navMeshAgent.destination, Color.red);
        }
    }
}