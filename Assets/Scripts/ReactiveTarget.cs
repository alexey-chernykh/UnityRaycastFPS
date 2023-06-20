using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactiveTarget : MonoBehaviour
{
    private bool _isAlive = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReactHit() 
    {
        Debug.Log("Enemy Hit!!!");
        StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        if (_isAlive)
        {
            _isAlive = false;
            this.transform.Rotate(-90, 0, 0);
            yield return new WaitForSeconds(1f);
            Destroy(this.gameObject);
        }
    }
}
