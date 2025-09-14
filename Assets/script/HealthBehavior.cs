using UnityEngine;

public class HealthBehavior : MonoBehaviour
{
    [SerializeField] private float Health;
    [SerializeField] private float MaxHealth;

    private float invincibilityDuration = 1.0f; // Durée d'invincibilité en secondes
    private float timer = 0.0f;
    private bool isInvincible = false;


    [SerializeField] private HealthBar healthBar;

    void Start()
    {
        Health = MaxHealth;
        healthBar.UpdateHealthBar(MaxHealth, Health);
    }
    public void TakeDamage(float damageAmount)
    {
        if (!isInvincible)
        {
            Health -= damageAmount;
            healthBar.UpdateHealthBar(MaxHealth, Health);
            Debug.Log($"{gameObject.name} took {damageAmount} damage, remaining health: {Health}");
            timer = invincibilityDuration;
            isInvincible = true;

            // Vérification de la vie
            if (Health <= 0)
            {
                if(gameObject.CompareTag("Player"))
                {
                    // Logique spécifique pour le joueur
                    Debug.Log("Player has died!");
                    Time.timeScale = 0; // Met le jeu en pause
                    // Ajouter ici des actions spécifiques au joueur, comme recharger la scène, afficher un écran de fin, etc.
                }
                else
                {
                    Destroy(gameObject);
                    // Ajouter ici des actions spécifiques aux ennemis ou autres objets, comme jouer une animation de mort, etc.
                }
            }
        }
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;   
        }
        else
        {
            isInvincible = false;
        }
    }
}
