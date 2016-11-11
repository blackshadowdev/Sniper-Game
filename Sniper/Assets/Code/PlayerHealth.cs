using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour {

	//[SerializeField] private int _playerHealth;
	[SerializeField] private SpriteRenderer _heartIcon1;
	[SerializeField] private SpriteRenderer _heartIcon2;
	[SerializeField] private SpriteRenderer _heartIcon3;
	[SerializeField] private Renderer _bloodSplatter;
	[SerializeField] private VibrateController _vibrateController;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (_bloodSplatter.material.color.a > 0) {
			float _tempAlpha = _bloodSplatter.material.color.a - 0.01f;
			_bloodSplatter.material.color = new Color(1, 1, 1, _tempAlpha);
		}
	}

	public void PlayerIsHit() {
		//_playerHealth--;
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
			// TODO: move thıs functıonalıty to a scene loader scrıpt
			if (Application.loadedLevelName == "Cartoon City 1" || Application.loadedLevelName == "Lunch Interrupted")
				SceneManager.LoadScene("YouDied");
		}
	}
}
