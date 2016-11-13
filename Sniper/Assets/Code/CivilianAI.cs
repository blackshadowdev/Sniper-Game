using UnityEngine;

public class CivilianAI : BaseAI
{
    private Transform _centerPoint = null;
    private float _lastFindWaypointTime;
    [SerializeField] private UIManager _manager;
    
    public void Die(InteractiveItem _civilian)
    {
        Animator.SetTrigger("DeathTrigger");
        UpdatePlayerData();
    }

    protected override void OnEnable()
    {
        FindNewWaypoint();
    }

    protected override void OnTakeAction()
    {
    }

    protected override void OnUpdate()
    {
        transform.LookAt(WaypointNavigator.CurrentWaypoint);
        if (WaypointNavigator.InStoppingDistance || (Time.time - _lastFindWaypointTime > 15f))
        {
            FindNewWaypoint();
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
        FindNewWaypoint();
    }

    private void FindNewWaypoint()
    {
        var waypoint = WaypointNavigator.CurrentWaypoint;

        waypoint.position = new Vector3(_centerPoint.position.x + Random.Range(-50f, 50f), 0f,
            _centerPoint.position.y + Random.Range(-50f, 50f));

        _lastFindWaypointTime = Time.time;
    }

    private void UpdatePlayerData() {
        _manager._playerScore -= 500;
        _manager._scoreText.text = _manager._playerScore.ToString();
        _manager._scorePFX.Emit(50);
    }
}