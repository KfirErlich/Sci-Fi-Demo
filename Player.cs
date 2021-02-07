using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    CharacterController _controller;
    [SerializeField]
    private float _speed=3.5f;
    private float _gravity=9.81f;
    [SerializeField]
    private GameObject _muzzleFlash;
    [SerializeField]
    private GameObject _hitMarkerPreFab;
    [SerializeField]
    private AudioSource _weaponAudio;
    [SerializeField]
    private int currentAmmo;
    private int maxAmmo = 50;
    [SerializeField]
    private GameObject _weapon;

    private UIManager _uiManager;

    private bool _isReloading = false;
    public bool hasCoin = false;
    // Start is called before the first frame update
    void Start()
    {
        _controller= GetComponent<CharacterController>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        currentAmmo = maxAmmo;

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0) && currentAmmo > 0)
        {
            shoot();        
        }

        else
        {
            _muzzleFlash.SetActive(false);
            _weaponAudio.Stop();
        }

        if (Input.GetKeyDown(KeyCode.R) && _isReloading==false)
        {
            _isReloading = true;
            StartCoroutine(Reload());
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        CalculateMovment();
        
    }

    void shoot()
    {
        currentAmmo--;
        _muzzleFlash.SetActive(true);
        _uiManager.UpdateAmmo(currentAmmo);
        
        if (_weaponAudio.isPlaying == false)
        {
            _weaponAudio.Play();
        }

        Ray rayOrigin = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hitInfo;

        if (Physics.Raycast(rayOrigin, out hitInfo))
        {
            Debug.Log("Hit: " + hitInfo.transform.name);
            GameObject HitMarker = Instantiate(_hitMarkerPreFab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal)) as GameObject;
            Destroy(HitMarker, 1f);

            Destructable crate = hitInfo.transform.GetComponent<Destructable>();
            if(crate!=null)
            {
                crate.DestroyCrate();
            }    


        }

    }

    void CalculateMovment()
    {
         float horizontalInput=Input.GetAxis("Horizontal");
        float verticalInput=Input.GetAxis("Vertical");
        Vector3 diraction= new Vector3(horizontalInput,0,verticalInput);
        Vector3 velocity= diraction * _speed;
        velocity.y -= _gravity;
        velocity= transform.TransformDirection(velocity);
        _controller.Move(velocity * Time.deltaTime);
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(1.5f);
        currentAmmo = maxAmmo;
        _uiManager.UpdateAmmo(currentAmmo);
        _isReloading = false;
    }

    public void EnableWeapon()
    {
        _weapon.SetActive(true);
    }
}

