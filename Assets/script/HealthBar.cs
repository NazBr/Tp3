using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _healthbarsprite;

    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        float healthRatio = currentHealth / maxHealth;
        _healthbarsprite.fillAmount = healthRatio;
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - _mainCamera.transform.position);
    }
}
