using UnityEngine;
using System.Collections;

public class ViveControllerLaptop : MonoBehaviour {
    public Transform _leftController;
    public Transform _rightController;
    public Transform _head;

    [SerializeField] private Rifle _rifle;
	[SerializeField] private Scope _scope;

    void Start () {
        _leftController.gameObject.SetActive(true);
        _rightController.gameObject.SetActive(true);
        _head.position = new Vector3(_head.position.x, _head.position.y + 1.6f, _head.position.z);
        _leftController.position = new Vector3(_leftController.position.x + 1, _leftController.position.y + 1.5f, _leftController.position.z - 1.5f);
        _rightController.position = new Vector3(_rightController.position.x + 1, _rightController.position.y + 1.5f, _rightController.position.z - 0.1f);
        _rifle = FindObjectOfType<Rifle>();
    }

    void Update() {
        if (Input.GetKeyDown("space")) {
			_scope.ScopeSwitch();
        }
        if (Input.GetKeyDown("f")) {
			_rifle.Fire();  
        }
		if (Input.GetKeyUp("f")) {
			_rifle.Invoke("TriggerUp", 0.5f);
		}
        if (Input.GetKey("e")) {
            _head.transform.Translate(Vector3.right * 0.1f);
        }
        if (Input.GetKey("q")) {
            _head.transform.Translate(Vector3.right * -0.1f);
        }
    }
}
