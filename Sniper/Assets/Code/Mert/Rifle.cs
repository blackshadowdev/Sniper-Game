using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Rifle : MonoBehaviour {
    public bool _scopeOn;
    public Camera _camera;
    public Camera _scopeCamera;
    public bool _rifleCanFire;
    public float _rightPadY;
    public AudioSource _rifleAudioSource;
    public AudioClip _rifleShot;
    public AudioClip _rifleChamber;
    public AudioClip _magazineReload;
    public SpriteRenderer _bulletIcon1;
    public SpriteRenderer _bulletIcon2;
    public SpriteRenderer _bulletIcon3;
    public SpriteRenderer _bulletIcon4;
    public SpriteRenderer _heartIcon1;
    public SpriteRenderer _heartIcon2;
    public SpriteRenderer _heartIcon3;
    public Transform _bloodPfxPrefab;
    private EnemyManager _enemyManagerScript;
    private sceneManager _sceneManager;
    //private float _startTime;
    public Text _scoreText;
    public Renderer _bloodSplatter;

    // Use this for initialization
    void Start() {
        _enemyManagerScript = FindObjectOfType<EnemyManager>();
        _sceneManager = FindObjectOfType<sceneManager>();
        _sceneManager._startTime = Time.time;
        _rifleCanFire = true;
        _scoreText.text = "0";
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
            _rifleAudioSource.PlayOneShot(_rifleShot);
            Vector3 _fwd = _scopeCamera.transform.TransformDirection(Vector3.forward);
            RaycastHit _hit;
            Debug.DrawRay(_scopeCamera.transform.position, _fwd * 100, Color.green, 1);
            if (Physics.Raycast(_scopeCamera.transform.position, _fwd, out _hit)) {
                if (_hit.transform.tag == "Enemy") {
                    _enemyManagerScript.KillEnemy(_hit.transform.gameObject);
                    Instantiate(_bloodPfxPrefab, _hit.point, Quaternion.identity);
                    _sceneManager._bonusTime += 15;
                    _sceneManager._timerPfx.Emit(50);
                    _sceneManager._playerScore += 250;
                    _scoreText.text = _sceneManager._playerScore.ToString();
                    _sceneManager._scorePFX.Emit(50);
                }

                if (_hit.transform.tag == "Civilian") {
                    CivilianAI _civilianController = _hit.transform.GetComponent<CivilianAI>();
                    _civilianController.Die();
                    _sceneManager._playerScore -= 500;
                    _scoreText.text = _sceneManager._playerScore.ToString();
                    _sceneManager._scorePFX.Emit(50);
                }
            }
            else {
                Debug.Log("Miss!");
            }

            //  SetBulletIcons();


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
        // _playerHealth--;
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
            // if (Application.loadedLevelName == "city 1")
            //SceneManager.LoadScene("city 1");
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