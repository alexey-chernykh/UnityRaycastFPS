using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastScript : MonoBehaviour
{
    private Camera _camera;
    public GUIStyle _style;
    public GameObject _bulletPrefab;
    public GameObject _gun;
    public bool isADS = false;
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 point = new Vector3(
                _camera.pixelWidth / 2.0f,
                _camera.pixelHeight / 2.0f,
                0
                ); 
            Ray ray = _camera.ScreenPointToRay( point );
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log($"Hit: {hit.point}");
                GameObject obj = hit.transform.gameObject;
                ReactiveTarget rt = obj.GetComponent<ReactiveTarget>();
                if (rt == null)
                {
                    StartCoroutine(SphereIndicator(hit.point));
                }
                else
                {
                    rt.ReactHit();
                }
            }
            else Debug.Log($"Hit out: {hit.point}");
        }
        else if (Input.GetMouseButtonDown(1))
        {
            GunADS();
        }
    }

    private void GunADS()
    {
        if (isADS)
        {
            _camera.fieldOfView = 60f;
            _gun.transform.Translate(-0.3f, -0.12f, 0);
            //_gun.transform.position = new Vector3(-0.28f, -0.28f, _gun.transform.position.z);
            isADS = false;
        }
        else
        {
            _camera.fieldOfView = 30f;
            _gun.transform.Translate(0.3f, 0.12f, 0);
            //_gun.transform.position = new Vector3(0f, -0.18f, _gun.transform.position.z);
            isADS = true;
        }
    }

    private IEnumerator SphereIndicator(Vector3 point)
    {
        GameObject tmpBullet = GameObject.Instantiate(_bulletPrefab, point, Quaternion.identity);
        yield return new WaitForSeconds(3f);
        Destroy(tmpBullet);

    }

    void OnGUI()
    {
        float size = 50;
        float x = (_camera.pixelWidth / 2.0f) - size / 2f;
        float y = (_camera.pixelHeight / 2.0f) - size / 2f;
        GUI.Label(new Rect(x,y,size, size), "+", _style);
    }
}
