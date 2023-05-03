using UnityEngine;

public class Enemy : Character
{
    private Vector3 m_originPos;
    protected override void Awake()
    {
        base.Awake();
        m_originPos = transform.position;
    }

    private void Update()
    {
        if (IsDead) return;
        if (m_Target)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, AtkRange, m_EnemyLayer);
            if (hitColliders != null && hitColliders.Length > 0)
            {
                isWalk = false;
                AttackCoroutine ??= StartCoroutine(OnAttack(hitColliders));
            }
            else
            {
                m_Anim.Play(WalkAnim);
                m_Destination = m_Target.position;
                m_Agent.SetDestination(m_Destination);
                isWalk = true;
            }
            transform.LookAt(m_Target);
        }

        if (Vector3.Distance(transform.position, m_originPos) >= 10f)
        {
            m_Target = null;
            m_Anim.Play(WalkAnim);
            m_Destination = m_originPos;
            m_Agent.SetDestination(m_Destination);
            isWalk = true;
        }

        if (Vector3.Distance(transform.position, m_Destination) <= 0.1f && isWalk)
        {
            isWalk = false;
            m_Anim.Play(IdleAnim);
        }
    }

    public override void ApplyDamage(Character damager)
    {
        base.ApplyDamage(damager);
        m_Target = damager.transform;
    }
}