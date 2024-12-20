using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Penser à ajouter un Mathf.Lerp aux valeurs pour ajouter une animation aux changements de valeurs.
    
    [Header ("Référence UI")]
    [SerializeField] Slider _hpSlider;
    [SerializeField] Slider _shieldSlider;
    
    public void SetMaxHealth(int health)
    {
        _hpSlider.maxValue = health;
        _hpSlider.value = health;
    }

    public void SetHealth(int health)
    {
        _hpSlider.value = health;
    }

    public void SetMaxShield(int shield)
    {
        _shieldSlider.maxValue = 100;
        _shieldSlider.value = shield;
    }

    public void SetShield(int shield)
    {
        _shieldSlider.value = shield;
    }
}
