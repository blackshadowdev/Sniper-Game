using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class SpotterTool : MonoBehaviour {

	[SerializeField] private Material _spotterMaterial;
	[SerializeField] private Material _normalMaterial;
	[SerializeField] private EnemyManager _enemyManager;
	[SerializeField] private List<Renderer> _spottedEnemyRenderers;		// if I make this private (non-serialized) it doesn't work.  Why?  There's no need to expose it in the inspector, so I want it to be just private.
	private float _spotterStartTime;
	private float _spotterEndTime;
	private bool _needToTurnOffSpotter;
	[SerializeField] private Image _spotterUI;


 
	// Use this for initialization
	void Start () {
		_spotterUI.fillAmount = 1;
	}
	
	// Update is called once per frame
	void Update () {
		#if UNITY_EDITOR
		if (Input.GetKeyDown("s"))
		{
			ActivateSpotter();
		}
		#endif

		if (_needToTurnOffSpotter)
		{
			if (Time.time > _spotterEndTime)
			{
				for (int i = 0; i < _spottedEnemyRenderers.Count; i++)
				{
					_spottedEnemyRenderers[i].material = _normalMaterial;
				}
				_spottedEnemyRenderers.Clear();
				_needToTurnOffSpotter = false;
			}
		} else if (!_needToTurnOffSpotter && _spotterUI.fillAmount < 1)
			FillSpotterUI();
	}

	public void ActivateSpotter ()
	{
		if (_spotterUI.fillAmount == 1)
		{
			_spotterUI.fillAmount = 0;
			_spotterStartTime = Time.time;
			_spotterEndTime = Time.time + 5;
			_needToTurnOffSpotter = true;
			for (int i = 0; i < _enemyManager._activeEnemies.Count; i++)
			{
				Renderer _tempRenderer = _enemyManager._activeEnemies[i].transform.FindChild("RPG-Character-Mesh").GetComponent<Renderer>();
				_spottedEnemyRenderers.Add(_tempRenderer);
				_spottedEnemyRenderers[i].material = _spotterMaterial;
			}
		}
	}

	public void FillSpotterUI ()
	{
		float _elapsedTime = Time.time - _spotterEndTime;
		float _uiFillAmount = _elapsedTime/30;
		if (_uiFillAmount > 1)
			_uiFillAmount = 1;
		_spotterUI.fillAmount = _uiFillAmount;
	}
}
