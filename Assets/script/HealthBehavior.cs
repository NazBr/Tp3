using UnityEngine;

public class HealthBehavior : MonoBehaviour
{
    [SerializeField] private float Health;
    [SerializeField] private float MaxHealth;

    private float invincibilityDuration = 1.0f; // Dur�e d'invincibilit� en secondes
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

            // V�rification de la vie
            if (Health <= 0)
            {
                if(gameObject.CompareTag("Player"))
                {
                    // Logique sp�cifique pour le joueur
                    Debug.Log("Player has died!");
                    Time.timeScale = 0; // Met le jeu en pause
                    // Ajouter ici des actions sp�cifiques au joueur, comme recharger la sc�ne, afficher un �cran de fin, etc.
                }
                else
                {
                    Destroy(gameObject);
                    // Ajouter ici des actions sp�cifiques aux ennemis ou autres objets, comme jouer une animation de mort, etc.
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
