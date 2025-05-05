using UnityEngine;
using System.Collections;

public class ReactiveWater : MonoBehaviour
{
    [Header("Ripple Settings")]
    [SerializeField] private float rippleStrength = 1f;
    [SerializeField] private float rippleRadius = 2f;
    [SerializeField] private float rippleDuration = 1f;

    [Header("Splash Effects")]
    [SerializeField] private ParticleSystem splashParticles;
    [SerializeField] private float minImpactForce = 0.1f;
    [SerializeField] private float maxSplashForce = 5f;

    private MeshRenderer waterMeshRenderer;
    private MaterialPropertyBlock propertyBlock;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        waterMeshRenderer = GetComponent<MeshRenderer>();
        propertyBlock = new MaterialPropertyBlock();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision) {
        // Get the impact force
        float impactForce = collision.relativeVelocity.magnitude;

        // Only create effects if the impact is strong enough
        if (impactForce > minImpactForce) {
            Vector3 contactPoint = collision.contacts[0].point;
            
            // Trigger water effects
            TriggerWaterRipple(contactPoint, impactForce);
            CreateSplash(contactPoint, impactForce);
        }
    }

    private void TriggerWaterRipple(Vector3 position, float force)
    {
        // Convert world position to local UV coordinates
        Vector3 localPos = transform.InverseTransformPoint(position);
        Vector2 rippleUV = new Vector2(localPos.x + 0.5f, localPos.z + 0.5f);

        // Apply ripple effect to shader
        waterMeshRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetVector("_RippleCenter", rippleUV);
        propertyBlock.SetFloat("_RippleStrength", rippleStrength * Mathf.Clamp01(force / maxSplashForce));
        propertyBlock.SetFloat("_RippleRadius", rippleRadius);
        waterMeshRenderer.SetPropertyBlock(propertyBlock);

        // Start ripple animation
        StartCoroutine(AnimateRipple());
    }

    private void CreateSplash(Vector3 position, float force)
    {
        if (splashParticles != null)
        {
            // Position the particle system at the impact point
            splashParticles.transform.position = position;

            // Scale particle emission based on impact force
            var emission = splashParticles.emission;
            var burst = emission.GetBurst(0);
            burst.count = Mathf.Lerp(5, 20, force / maxSplashForce);
            emission.SetBurst(0, burst);

            splashParticles.Play();
        }
    }

    private System.Collections.IEnumerator AnimateRipple()
    {
        float elapsedTime = 0f;

        while (elapsedTime < rippleDuration)
        {
            elapsedTime += Time.deltaTime;
            float strength = Mathf.Lerp(rippleStrength, 0f, elapsedTime / rippleDuration);
            
            waterMeshRenderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetFloat("_RippleStrength", strength);
            waterMeshRenderer.SetPropertyBlock(propertyBlock);

            yield return null;
        }
    }
}
