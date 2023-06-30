using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarController : MonoBehaviour
{
    [SerializeField] private Image _healthbarSprite;
    [SerializeField] private float _reduceSpeed = 2f;
    private float _target = 1f;
    private Camera _cam;
    private void Start()
    {
        _cam = Camera.main;
    }
    public void UpdateHealthBar(float maxHealth, float currentHealth, bool v)
    {
        if (v)
            _target = currentHealth / maxHealth;
        else
            _healthbarSprite.fillAmount = currentHealth / maxHealth;
    }
    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - _cam.transform.position);
        _healthbarSprite.fillAmount = Mathf.MoveTowards(_healthbarSprite.fillAmount, _target, _reduceSpeed * Time.deltaTime);
    }
}
