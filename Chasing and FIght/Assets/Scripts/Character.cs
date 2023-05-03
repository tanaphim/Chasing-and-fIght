using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
public abstract class Character : MonoBehaviour
{
    protected static readonly int IdleAnim = Animator.StringToHash("Idle");
    protected static readonly int WalkAnim = Animator.StringToHash("Walk");
    protected static readonly int AttackAnim = Animator.StringToHash("Attack");
    protected static readonly int DieAnim = Animator.StringToHash("Die");

    [SerializeField] protected LayerMask m_EnemyLayer;
    [SerializeField] protected LayerMask m_GroundLayer;

    public float MaxHp;
    public float Atk;
    public float AtkRange;
    public float Speed;

    private float m_CurrentHp;

    protected Animator m_Anim;
    protected NavMeshAgent m_Agent;

    protected Transform m_Target;
    protected Coroutine AttackCoroutine;
    protected Vector3 m_Destination;
    public bool IsDead;
    protected bool isWalk;

    protected virtual void Awake()
    {
        m_Anim = GetComponent<Animator>();
        m_Agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        m_CurrentHp = MaxHp;
        m_Agent.speed = Speed;
    }

    public virtual void ApplyDamage(Character damager)
    {
        m_CurrentHp -= damager.Atk;
        m_CurrentHp = Mathf.Clamp(m_CurrentHp, 0, MaxHp);
        if (m_CurrentHp == 0)
        {
            IsDead = true;
            m_Anim.Play(DieAnim);
            Debug.Log($"{gameObject.name} Downed");
        }
    }

    protected IEnumerator OnAttack(Collider[] targets)
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

    private void OnGUI()
    {
        if (IsDead)
        {
            if (GUI.Button(new Rect(0, 0, 100, 50), "Restart"))
                SceneManager.LoadScene(0);
        }
    }
}
