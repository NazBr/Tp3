using UnityEngine;

public class attack : MonoBehaviour
{

    [SerializeField] private Collider weaponCollider;
    public void EnableWeaponCollider()
    {
        weaponCollider.enabled = true;
    }
    public void DisableWeaponCollider()
    {
        weaponCollider.enabled = false;
    }
}
