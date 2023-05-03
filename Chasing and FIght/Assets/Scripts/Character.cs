using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
public class Character : MonoBehaviour
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

    // Component
    protected Animator m_Anim;
    protected NavMeshAgent m_Agent;

    // Enemy
    protected Transform m_Target;
    public bool IsDead;

    private void Awake()
    {
        m_Anim = GetComponent<Animator>();
        m_Agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        m_CurrentHp = MaxHp;
        m_Agent.speed = Speed;
    }

    public void ApplyDamage(Character damager)
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

    void OnGUI()
    {
        if (IsDead)
        {
            if (GUI.Button(new Rect(0, 0, 100, 50), "Restart"))
                SceneManager.LoadScene(0);
        }
    }
}
