using UnityEngine;

public class Footsteps_VFX : MonoBehaviour
{
    [Header("Splash Effect")]
    [Tooltip("The Foot_Splash prefab to spawn on each foot hit")]
    public GameObject footSplashPrefab;

    [Header("Foot Spawn Points")]
    [Tooltip("Empty GameObject under left foot bone")]
    public Transform leftFootSpawnPoint;
    [Tooltip("Empty GameObject under right foot bone")]
    public Transform rightFootSpawnPoint;

    // Called by Animation Event at left-foot contact
    public void LeftFootSplash()
    {
        SpawnSplash(leftFootSpawnPoint);
    }

    // Called by Animation Event at right-foot contact
    public void RightFootSplash()
    {
        SpawnSplash(rightFootSpawnPoint);
    }

    private void SpawnSplash(Transform footPoint)
    {
        if (footPoint == null || footSplashPrefab == null) return;

        // Instantiate the prefab at the foot’s position & rotation
        Instantiate(
            footSplashPrefab,
            footPoint.position,
            footPoint.rotation
        );
    }
}
