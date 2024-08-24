using UnityEngine;

public class Weapon : MonoBehaviour
{
    public TrailRenderer _trailRenderer;

    private void Awake()
    {
        _trailRenderer = GetComponentInChildren<TrailRenderer>();
        StopTrailEffect();
    }

    public void PlayTrailEffect()
    {
        _trailRenderer.gameObject.SetActive(true);
    }

    public void StopTrailEffect()
    {
        _trailRenderer.gameObject.SetActive(false);
    }
}
