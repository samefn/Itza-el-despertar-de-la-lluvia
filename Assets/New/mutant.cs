using UnityEngine;

public class mutant : MonoBehaviour
{
   public Animator m_Animator;
    public Transform player;
    float dist;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        dist = Vector3.Distance(transform.position, player.position);
        m_Animator.SetFloat("dist", dist);
    }
}
