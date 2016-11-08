using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;


public class Rifle : MonoBehaviour {
    [SerializeField] private bool _scopeOn;
    [SerializeField] private bool _rifleCanFire;
    [SerializeField] private float _rightPadY;
    [SerializeField] private int _playerHealth;

    [SerializeField] private AudioSource _rifleAudioSource;
    [SerializeField] private AudioClip _rifleShot;
    [SerializeField] private AudioClip _rifleChamber;
    [SerializeField] private AudioClip _magazineReload;
    
    public EnemyManager _enemyManagerScript;
    public Raycaster _raycaster;

    public Camera _camera;
    public Camera _scopeCamera;
    public SpriteRenderer _bulletIcon1;
    public SpriteRenderer _bulletIcon2;
    public SpriteRenderer _bulletIcon3;
    public SpriteRenderer _bulletIcon4;
    public SpriteRenderer _heartIcon1;
    public SpriteRenderer _heartIcon2;
    public SpriteRenderer _heartIcon3;
    public Renderer _bloodSplatter;
    public bool _fired;

	private VibrateController _vibrateController;

    // Use this for initialization
    void Start() {
        _rifleCanFire = true;
		_vibrateController = transform.GetComponent<VibrateController> ();
    }

    // Update is called once per frame
    void Update() {
        if (_bloodSplatter.material.color.a > 0) {
            float _tempAlpha = _bloodSplatter.material.color.a - 0.01f;
            _bloodSplatter.material.color = new Color(1, 1, 1, _tempAlpha);
        }
    }

    public void Fire() {
        if (_rifleCanFire) {
			_vibrateController.VibrateForFiring ();
            _rifleAudioSource.PlayOneShot(_rifleShot);
           // _fired = true;
           
            //  SetBulletIcons();
            _raycaster.Raycast();

            _rifleCanFire = false;

        }
    } // end of Fire() 

    public void ScopeSwitch() {
        // if Scope View, switch to Normal View
        if (_scopeOn) {
            _camera.cullingMask = (1 << 0);
            _camera.clearFlags = CameraClearFlags.Skybox;

            _scopeOn = false;
        }
        // if Normal View, switch to Scope View
        else if (!_scopeOn) {
            _camera.cullingMask = (1 << LayerMask.NameToLayer("Scope Image"));
            _camera.clearFlags = CameraClearFlags.SolidColor;
            _scopeOn = true;
        }
    } // end of ScopeSwitch()

    private void AdjustZoom() {
        float _tempRightPadY = _rightPadY;
        if (_tempRightPadY > 0.5f) {
            _tempRightPadY = 0.5f;
        }
        else if (_tempRightPadY < -0.5f) {
            _tempRightPadY = -0.5f;
        }
        _tempRightPadY += 0.5f;
        float _newFov = _tempRightPadY * 50;
        _scopeCamera.fieldOfView = _newFov;

        //_scopePivot.LookAt(_head);
    }

    public void RoundChambered() {
        _rifleCanFire = true;
    }

    public void MagazineReloaded() {
        _bulletIcon1.enabled = true;
        _bulletIcon2.enabled = true;
        _bulletIcon3.enabled = true;
        _bulletIcon4.enabled = true;
        _rifleCanFire = true;
    }

    public void SetBulletIcons() {
        if (_bulletIcon4.enabled) {
            _bulletIcon4.enabled = false;
        }
        else if (_bulletIcon3.enabled) {
            _bulletIcon3.enabled = false;
        }
        else if (_bulletIcon2.enabled) {
            _bulletIcon2.enabled = false;
        }
        else if (_bulletIcon1.enabled) {
            _bulletIcon1.enabled = false;
        }
    }

    public void PlayerIsHit() {
        _playerHealth--;
		_vibrateController.VibrateForDamage ();
        if (_heartIcon3.enabled) {
            _heartIcon3.enabled = false;
            _bloodSplatter.material.color = new Color(1, 1, 1, 0.5f);
        }
        else if (_heartIcon2.enabled) {
            _heartIcon2.enabled = false;
            _bloodSplatter.material.color = new Color(1, 1, 1, 1);
        }
        else if (_heartIcon1.enabled) {
            _heartIcon1.enabled = false;
            //if (Application.loadedLevelName == "rifle and npcs")
            //SceneManager.LoadScene("rifle and npcs");
            if (Application.loadedLevelName == "Cartoon City 1")
            SceneManager.LoadScene("YouDied");
        }
    }

    public void TriggerUp() {
        if (_bulletIcon1.enabled == false) {
            _rifleAudioSource.PlayOneShot(_magazineReload);
            Invoke("MagazineReloaded", 3.7f);
        }
        else {
            _rifleAudioSource.PlayOneShot(_rifleChamber);
            Invoke("RoundChambered", 1);
        }
    }
}