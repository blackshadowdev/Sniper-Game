using UnityEngine;
using System.Collections;


public class CivilianAI : BaseAI
{
	[SerializeField] private Transform _centerPoint;
    private float _lastFindWaypointTime;
    [SerializeField] private UIManager _manager;
	private bool _isCrouching;
    
    public void Die(InteractiveItem _civilian)
    {
        Animator.SetTrigger("DeathTrigger");
        UpdatePlayerData();
    }

    protected override void OnEnable()
    {
        FindNewWaypoint();
		Animator.SetTrigger("RunTrigger");

    }

    protected override void OnTakeAction()
    {
    }

    protected override void OnUpdate()
    {
		if (_isCrouching == false)
		{
			transform.LookAt(WaypointNavigator.CurrentWaypoint);
			if (WaypointNavigator.InStoppingDistance || (Time.time - _lastFindWaypointTime > 15f))
			{
				Debug.Log("update and near waypoint");
				FindNewWaypoint();
			}	
		}
    }

    protected override void OnDamage(DamageInfo info)
    {
    }

    protected override void OnDie()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
		if (collision.transform.tag != "Floor")
		{
			Debug.Log("civilian collides with not floor");
			StartCoroutine(CrouchPauseMove());
		}
		//FindNewWaypoint();
    }

    private void FindNewWaypoint()
    {
        var waypoint = WaypointNavigator.CurrentWaypoint;

        waypoint.position = new Vector3(_centerPoint.position.x + Random.Range(-30f, 30f), 0f,
            _centerPoint.position.z + Random.Range(-30f, 30f));

        _lastFindWaypointTime = Time.time;
    }

    private void UpdatePlayerData() {
        _manager._playerScore -= 500;
        _manager._scoreText.text = _manager._playerScore.ToString();
        _manager._scorePFX.Emit(50);
    }

	private IEnumerator CrouchPauseMove()
	{
		_isCrouching = true;
		Animator.SetTrigger("CrouchTrigger");
		float _pauseTime = Random.Range(1,6);
		yield return new WaitForSeconds(_pauseTime);
		FindNewWaypoint();
		Animator.SetTrigger("RunTrigger");
		_isCrouching = false;
	}
}