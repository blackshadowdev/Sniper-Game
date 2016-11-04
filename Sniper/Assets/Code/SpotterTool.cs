using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SpotterTool : MonoBehaviour {

	[SerializeField] private Material _spotterMaterial;
	[SerializeField] private Material _normalMaterial;
	[SerializeField] private EnemyManager _enemyManager;
	[SerializeField] private List<Renderer> _spottedEnemyRenderers;		// if I make this private (non-serialized) it doesn't work.  Why?  There's no need to expose it in the inspector, so I want it to be just private.
	private float _spotterStartTime;
	private bool _needToTurnOffSpotter;
 
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("s"))
		{
			ActivateSpotter();
		}

		if (_needToTurnOffSpotter)
		{
			if (Time.time - _spotterStartTime > 5)
			{
				for (int i = 0; i < _spottedEnemyRenderers.Count; i++)
				{
					_spottedEnemyRenderers[i].material = _normalMaterial;
				}
				_spottedEnemyRenderers.Clear();
				_needToTurnOffSpotter = false;
			}
		}
	}

	public void ActivateSpotter ()
	{
		_spotterStartTime = Time.time;
		_needToTurnOffSpotter = true;
		for (int i = 0; i < _enemyManager._activeEnemies.Count; i++)
		{
			Renderer _tempRenderer = _enemyManager._activeEnemies[i].transform.FindChild("RPG-Character-Mesh").GetComponent<Renderer>();
			_spottedEnemyRenderers.Add(_tempRenderer);
			_spottedEnemyRenderers[i].material = _spotterMaterial;
		}
	}
}
