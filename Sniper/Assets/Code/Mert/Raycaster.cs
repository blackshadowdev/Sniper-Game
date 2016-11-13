using UnityEngine;
using System;

public class Raycaster : MonoBehaviour {
    public event Action<RaycastHit> OnRaycasthit;

    [SerializeField] private Camera _scopeCamera;
    [SerializeField] private EnemyManager _enemyManagerScript;
    [SerializeField] private CivilianAI _civilianController;
    [SerializeField] private UIManager _UIManager;
    [SerializeField] private Rifle _rifle;

    void Start() {
        _enemyManagerScript = FindObjectOfType<EnemyManager>();
        _UIManager = FindObjectOfType<UIManager>();
        _rifle = FindObjectOfType<Rifle>();
    }

    public void Raycast() {
        Vector3 _fwd = _scopeCamera.transform.TransformDirection(Vector3.forward);
        RaycastHit _hit;
        Debug.DrawRay(_scopeCamera.transform.position, _fwd * 100, Color.green, 1);
        if (Physics.Raycast(_scopeCamera.transform.position, _fwd, out _hit)) {
            InteractiveItem interactible = _hit.collider.GetComponent<InteractiveItem>();   //attempt to get the InteractiveItem on the hit object                                                                                // if (_rifle._fired) 
            {
                if (interactible) {
                    interactible.ShootItem();
                    _enemyManagerScript.KillEnemy(interactible);
                    //_civilianController.Die(interactible);
                }
            }
        }

        if (OnRaycasthit != null)
            OnRaycasthit(_hit);
    }
}
