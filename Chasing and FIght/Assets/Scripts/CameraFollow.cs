using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 m_Offset;

    private Transform m_Player;

    private void Start()
    {
        m_Player = FindObjectOfType<Player>().transform;
    }

    private void LateUpdate()
    {
        transform.position = m_Player.transform.position + m_Offset;
    }
}
