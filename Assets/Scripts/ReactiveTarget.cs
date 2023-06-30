using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactiveTarget : MonoBehaviour
{
    [SerializeField] private HealthbarController _healthBar;
    private float speed = 4;
    private float hp = 100;
    private bool _isAlive = true;
    private bool _MoveDirection = true;
    private System.Random rnd;
    private float _moveIterator = 0;

    // Start is called before the first frame update
    void Start()
    {
        rnd = new System.Random();
        _healthBar.gameObject.SetActive(true);
        _healthBar.UpdateHealthBar(100, 100, false);
    }

    private void MovingTarget()
    {
        if (_isAlive)
        {
            if (_MoveDirection)
                transform.position = (new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z));
            else
                transform.position = (new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z));
        }
    }

    // Update is called once per frame
    void Update()
    {
        _moveIterator += 1;
        if (_moveIterator >= rnd.Next(100, 350))
        {
            _moveIterator = 0;
            _MoveDirection = !_MoveDirection;
        }
        MovingTarget();
    }

    public void ReactHit(float damage) 
    {
        Debug.Log("Enemy Hit!!!");
        MinusHP(damage);
    }

    private void MinusHP(float damage)
    {
        if (hp - damage > 0)
        {
            hp -= damage;
            _healthBar.UpdateHealthBar(100, hp, true);
        }
        else
        {
            hp = 0;
            _healthBar.gameObject.SetActive(false);
            StartCoroutine(Die());
        }
        
    }

    private IEnumerator Die()
    {
        if (_isAlive)
        {
            _isAlive = false;
            this.transform.Rotate(-90, 0, 0);
            yield return new WaitForSeconds(1f);
            Instantiate(this.gameObject, new Vector3(rnd.Next(-18, 16), 2, rnd.Next(8, 18)), Quaternion.identity);
            Destroy(this.gameObject);
            
        }
    }
}
