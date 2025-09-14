using System.Security.Cryptography;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    [SerializeField] private float damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            HealthBehavior enemy = other.gameObject.GetComponent<HealthBehavior>();
            enemy.TakeDamage(damage);
            Debug.Log($"{gameObject.name} dealt {damage} damage to {other.gameObject.name}");
        }
        if (other.CompareTag("Player"))
        {
            HealthBehavior player = other.gameObject.GetComponent<HealthBehavior>();
            player.TakeDamage(damage);
            Debug.Log($"{gameObject.name} dealt {damage} damage to {other.gameObject.name}");
        }
    }
}
