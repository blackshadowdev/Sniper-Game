using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;


public class Rifle : MonoBehaviour {
    [SerializeField] private bool _scopeOn;
    [SerializeField] private bool _rifleCanFire;
    [SerializeField] private float _rightPadY;

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
   
    public bool _fired;

	private VibrateController _vibrateController;
	[SerializeField] private ParticleSystem _muzzleFlashPfx;
	private BulletUI _bulletUi;
	[SerializeField] private int _startingQuantityBullets;
	private int _currentQuantityBullets;

    // Use this for initialization
    void Start() {
        _rifleCanFire = true;
		_currentQuantityBullets = _startingQuantityBullets;
		_vibrateController = transform.GetComponent<VibrateController>();
		_bulletUi = transform.GetComponent<BulletUI>();
    }

    // Update is called once per frame
    void Update() {
		Debug.Log("currentBullets = " + _currentQuantityBullets);
    }

    public void Fire() {
        if (_rifleCanFire) {
			_rifleCanFire = false;
			_vibrateController.VibrateForFiring ();
            _rifleAudioSource.PlayOneShot(_rifleShot);
			_muzzleFlashPfx.Emit (1);
			_bulletUi.DecreaseBulletIcons();
			_currentQuantityBullets --;
            _raycaster.Raycast();
        }
    }

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
		_bulletUi.RestoreBulletIcons();
		_currentQuantityBullets = _startingQuantityBullets;
		_rifleCanFire = true;
    }

    public void TriggerUp() {
		if (_currentQuantityBullets <= 0) {
            _rifleAudioSource.PlayOneShot(_magazineReload);
            Invoke("MagazineReloaded", 3.7f);
        }
        else {
            _rifleAudioSource.PlayOneShot(_rifleChamber);
            Invoke("RoundChambered", 1);
        }
    }
}