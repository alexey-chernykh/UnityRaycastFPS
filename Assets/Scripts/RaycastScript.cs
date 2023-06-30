using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class RaycastScript : MonoBehaviour
{
    [SerializeField] AudioClip _shootSound;
    [SerializeField] AudioClip _reloadSound;
    [SerializeField] TextMeshProUGUI _bulletCounter;
    private AudioSource audioSource;
    private Camera _camera;
    public GUIStyle _style;
    public GameObject _bulletPrefab;
    public GameObject _gun;
    public bool isADS = false;
    public bool isHolding = false;
    public bool isReloading = false;
    private bool isShooting = false;
    private bool isHitting = false;
    private float _shootIterator = 0;
    private float _zoomTarget = 60f;
    private int magazineCount = 30;
    private int bulletCount = 30;
    

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        _bulletCounter.text = $"{magazineCount}/{bulletCount}";
        _camera.fieldOfView = Mathf.MoveTowards(_camera.fieldOfView, _zoomTarget, 2);
        _shootIterator += 1;
        if (Input.GetMouseButton(0) && _shootIterator>=30 && magazineCount>0 && !isReloading && !isHitting)
        {
            _shootIterator = 0;
                Vector3 point = new Vector3(
                    _camera.pixelWidth / 2.0f,
                    _camera.pixelHeight / 2.0f,
                    0
                    );
                Ray ray = _camera.ScreenPointToRay(point);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    //Debug.Log($"Hit: {hit.point}");
                    GameObject obj = hit.transform.gameObject;
                    ReactiveTarget rt = obj.GetComponent<ReactiveTarget>();
                    if (rt == null)
                    {
                        StartCoroutine(SphereIndicator(hit.point));
                    }
                    else
                    {
                        rt.ReactHit(25f);
                    }
                }
            //else Debug.Log($"Hit out: {hit.point}");
            audioSource.PlayOneShot(_shootSound, 0.5f);
            if (!isADS)
            {
                _gun.GetComponent<Animator>().Play("Shoot");
            }
            else
            {
                _gun.GetComponent<Animator>().Play("ShootADS");
            }

            if (magazineCount-1 >= 0)
            {
                magazineCount--;
            }
            else
            {
                magazineCount = 0;
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            GunADS();
        }
        else if (Input.GetKeyDown(KeyCode.R) && bulletCount != 0 && magazineCount != 30 && !isHitting)
        {
            StartCoroutine(GunReload());
        }
        else if (Input.GetKeyDown(KeyCode.V) && !isReloading && !isADS && !isHitting)
        {
            StartCoroutine(GunbuttHit());
        }
    }

    private IEnumerator GunbuttHit()
    {
        Debug.Log(isHitting);
        if (!isHitting)
        {
            isHitting = true;
            _gun.GetComponent<Animator>().Play("GunbuttHit");
            yield return new WaitForSeconds(0.4f);
            Vector3 point = new Vector3(
                    _camera.pixelWidth / 2.0f,
                    _camera.pixelHeight / 2.0f,
                    0
                    );
            Ray ray = _camera.ScreenPointToRay(point);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject obj = hit.transform.gameObject;
                ReactiveTarget rt = obj.GetComponent<ReactiveTarget>();
                //Debug.Log($"Hit: {hit.point}");
                if (hit.distance <= 1.5f)
                {
                    rt.ReactHit(100f);
                }
            }
        }
        isHitting = false;
        Debug.Log(isHitting);
    }

    private IEnumerator GunReload()
    {
        if (!isReloading)
        {
            isReloading = true;
            audioSource.PlayOneShot(_reloadSound, 0.5f);
            _gun.GetComponent<Animator>().Play("Reload");
            yield return new WaitForSeconds(2);
            if (bulletCount - (30 - magazineCount) >= 0)
            {
                bulletCount -= 30 - magazineCount;
                magazineCount += 30 - magazineCount;
            }
            else
            {
                magazineCount += bulletCount;
                bulletCount = 0;
            }
        }
        isReloading = false;
    }

    private void GunADS()
    {
        if (isADS)
        {
            _zoomTarget = 60f;
            _gun.GetComponent<Animator>().Play("ADSout");
            _gun.transform.Translate(-0.3f, -0.12f, 0);
            //_gun.transform.position = new Vector3(-0.3f, -0.12f, 0);
            isADS = false;
        }
        else
        {
            _zoomTarget = 30f;
            _gun.GetComponent<Animator>().Play("ADSin");
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
