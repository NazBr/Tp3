using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour
{
    public Transform player; // À assigner dans l'inspecteur ou dynamiquement
    public float speed = 3f;
    private bool isTouchingPlayer = false;
    private Animator animator;

    public float attackDistance = 5f; // Distance seuil pour attaquer
    public float chaseDistance = 20f; // Distance maximale pour commencer à poursuivre

    

    void Start()
    {
        animator = GetComponentInChildren<Animator>();

    }

    void Update()
    {
        if (player == null || isTouchingPlayer) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < chaseDistance)
        {
            // Direction vers le joueur
            Vector3 direction = (player.position - transform.position).normalized;

            // Rotation vers le joueur
            if (direction.sqrMagnitude > 0.0001f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 0.2f);
            }

            // Déplacement
            transform.position += direction * speed * Time.deltaTime;
            animator.SetBool("IsRunning2", true);

            // Déclenchement de l'attaque si très proche
            if (distanceToPlayer < attackDistance)
            {
                animator.SetTrigger("doAttackEnnemy");
                speed = 10f;
            }
        }
        else
        {
            // Ennemi immobile et animation arrêtée si joueur trop loin
            animator.SetBool("IsRunning2", false);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform == player)
        {
            isTouchingPlayer = true; // Arrête de suivre
            animator.SetBool("IsRunning2", false);
            StartCoroutine(ResumeChaseAfterDelay(4f)); // 2 secondes d'attente
        }
    }

    IEnumerator ResumeChaseAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isTouchingPlayer = false;
        animator.SetBool("IsRunning2", true);
    }
}
