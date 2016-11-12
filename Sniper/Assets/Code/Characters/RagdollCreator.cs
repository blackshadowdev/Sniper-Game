using UnityEngine;

public class RagdollCreator : MonoBehaviour
{
    [SerializeField] private GameObject _ragdollPrefab = null;
    private IHealth _health;

    public void CreateRagdoll()
    {
        var ragdoll = (GameObject)Instantiate(_ragdollPrefab, transform.position, transform.rotation);
        MatchBipeds(transform, ragdoll.transform);
    }

    private void Awake()
    {
        _health = GetComponent<IHealth>();
        _health.DieEvent += OnDie;
    }

    private void OnDestroy()
    {
        _health.DieEvent -= OnDie;
    }

    private void OnDie()
    {
        CreateRagdoll();
    }

    private static void MatchBipeds(Transform original, Transform created)
    {
        created.position = original.position;
        created.rotation = original.rotation;

        foreach (Transform originalChild in original)
        {
            foreach (Transform createdChild in created)
            {
                if (originalChild.name != createdChild.name)
                {
                    continue;
                }

                MatchBipeds(originalChild, createdChild);
                break;
            }
        }
    }
}