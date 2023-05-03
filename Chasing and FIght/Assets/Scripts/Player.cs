using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Player : Character
{
    Coroutine AttackCoroutine;
    Vector3 m_Destination;

    bool isWalk;

    // Update is called once per frame
    private void Update()
    {
        // Left Click on Enemy
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_EnemyLayer))
            {
                m_Target = hit.collider.transform;
                isWalk = true;
                m_Anim.Play(WalkAnim);
                m_Destination = m_Target.position;
                m_Agent.isStopped = false;
                m_Agent.SetDestination(m_Destination);
            }
        }

        // Right Click on Ground
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_GroundLayer))
            {
                m_Target = null;
                isWalk = true;
                m_Anim.Play(WalkAnim);
                m_Destination = hit.point;
                m_Agent.isStopped = false;
                m_Agent.SetDestination(hit.point);
            }
        }

        if (Vector3.Distance(transform.position, m_Destination) <= 0.5f && isWalk)
        {
            m_Anim.Play(IdleAnim);
            m_Agent.isStopped = true;
            isWalk = false;
        }

        if (m_Target)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, AtkRange, m_EnemyLayer);
            if (hitColliders != null && hitColliders.Length > 0)
            {
                isWalk = false;
                m_Agent.isStopped = true;
                AttackCoroutine ??= StartCoroutine(OnAttack(hitColliders));
            }
        }
    }

    IEnumerator OnAttack(Collider[] targets)
    {
        if (!targets.All(x => x.GetComponent<Character>().IsDead))
        {
            m_Anim.Play(AttackAnim);
            for (int i = 0; i < targets.Length; i++)
            {
                if (!targets[i].GetComponent<Character>().IsDead)
                {
                    targets[i].GetComponent<Character>().ApplyDamage(this);
                }
            }
            yield return new WaitForSeconds(1f);
            AttackCoroutine = null;
        }
    }


}