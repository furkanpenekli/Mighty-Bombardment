using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    private static VFXManager instance;
    public static VFXManager Instance => instance;

    public ParticleSystem[] particleSystems;
    private Dictionary<string, ParticleSystem> vfxDictionary;

    private void Awake()
    {
        // Check if an instance already exists
        if (instance != null && instance != this)
        {
            // Destroy the duplicate instance
            Destroy(gameObject);
        }
        else
        {
            // Set the instance
            instance = this;
        }

        // Build the VFX dictionary
        BuildVFXDictionary();
    }

    private void Start()
    {
        // Disable all particle systems at the beginning
        foreach (ParticleSystem ps in particleSystems)
        {
            ps.Stop();
        }
    }

    private void BuildVFXDictionary()
    {
        vfxDictionary = new Dictionary<string, ParticleSystem>();

        foreach (ParticleSystem ps in particleSystems)
        {
            // Store the particle system in the dictionary with its name as the key
            vfxDictionary.Add(ps.gameObject.name, ps);
        }
    }
    public void PlayVFX(string vfxName, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        ParticleSystem psPrefab;

        // Check if the VFX exists in the dictionary
        if (vfxDictionary.TryGetValue(vfxName, out psPrefab))
        {
            ParticleSystem ps = Instantiate(psPrefab, position, rotation);
            ps.gameObject.transform.localScale = scale;
            ps.Play();

            // Check if the VFX should be destroyed after it finishes playing
            if (ps.main.loop)
            {
                // Get the total duration of the particle system
                float totalDuration = ps.main.duration + ps.main.startLifetime.constant;

                // Destroy the particle system after it finishes playing
                Destroy(ps.gameObject, totalDuration);
            }
        }
        else
        {
            Debug.LogError("VFX not found: " + vfxName);
        }
    }

    public void StopAllVFX()
    {
        // Stop all particle systems
        foreach (ParticleSystem ps in particleSystems)
        {
            ps.Stop();
        }
    }
}
