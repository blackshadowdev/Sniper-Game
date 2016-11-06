using UnityEngine;
using System;
using System.Collections;

public class Raycaster : MonoBehaviour 
{
    public event Action<RaycastHit> OnRaycasthit;
    public Transform _bloodPfxPrefab;

    public Camera _scopeCamera;
    public EnemyManager _enemyManagerScript;
    public UIManager _UIManager;
    public Rifle _rifle;

    void Update() {

       // Raycast();
    }

    public void Raycast() {
        Vector3 _fwd = _scopeCamera.transform.TransformDirection(Vector3.forward);
        RaycastHit _hit;
        Debug.DrawRay(_scopeCamera.transform.position, _fwd * 100, Color.green, 1);
        if(Physics.Raycast(_scopeCamera.transform.position, _fwd, out _hit)) {
            InteractiveItem interactible = _hit.collider.GetComponent<InteractiveItem>();   //attempt to get the InteractiveItem on the hit object
           // if (_rifle._fired) 
            {
               

                if (_hit.transform.tag == "Enemy") {
                    _enemyManagerScript.KillEnemy(_hit.transform.gameObject);
                    Instantiate(_bloodPfxPrefab, _hit.point, Quaternion.identity);
                    _UIManager._bonusTime += 15;
                    _UIManager._timerPfx.Emit(50);
                    _UIManager._playerScore += 250;
                    _UIManager._scoreText.text = _UIManager._playerScore.ToString();
                    _UIManager._scorePFX.Emit(50);
                }
                else if (_hit.transform.tag == "Civilian") {
                    CivilianAI _civilianController = _hit.transform.GetComponent<CivilianAI>();
                    _civilianController.Die();
                    _UIManager._playerScore -= 500;
                    _UIManager._scoreText.text = _UIManager._playerScore.ToString();
                    _UIManager._scorePFX.Emit(50);
                }
                else {
                    interactible.ShootMenuItem();
                }
            }
        }

        if (OnRaycasthit != null)
                OnRaycasthit(_hit);
        }
}
