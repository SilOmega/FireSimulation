using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Fire : MonoBehaviour
{
    public int ParticleCount = 300;
    public float ParticleSize = 0.2f;
    public float Height = 4f;
    private Particle[] FireParticles;
    public float SpawnRadius = 1.5f;
    private Vector3 TargetPoint = new Vector3();
    public Vector3 WindForce = new Vector3();
    public float Noise = 0.25f;
    public float ParticleMass = 100f;

    void Start()
    {
        FireParticles = new Particle[ParticleCount];
        for (int i = 0; i < FireParticles.Length; i++)
        {
            Vector2 pos = Random.insideUnitCircle * SpawnRadius;
            Color col = new Vector4(255 / 255f, 130 / 255f, 7 / 255f, 2f);
            if (Mathf.Abs(pos.x) < SpawnRadius * 0.5f && Mathf.Abs(pos.y) < SpawnRadius * 0.5f)
                col = new Vector4(255 / 255f, 70 / 255f, 7 / 255f, 2f);
            FireParticles[i] = new Particle(new Vector3(transform.position.x+pos.x, 0 , transform.position.z+pos.y),
                                            new Vector3(0,0,0), ParticleSize, ParticleMass, col, 0f, 2.5f);

        }
      
        TargetPoint = new Vector3(transform.position.x, Height, transform.position.z);
                
    }

    void FixedUpdate()
    {
        for (int i = 0; i < FireParticles.Length; i++)
        {        
            // Apply force on z and x axis to simulate heating force
            Vector3 force = new Vector3((TargetPoint.x - FireParticles[i].Position.x) * 0.35f,
                                        0,
                                        (TargetPoint.z - FireParticles[i].Position.z) * 0.35f);

            // Add some noice forse to have randomness in particles movement
            Vector3 NoiseForce = new Vector3(Random.Range(-Noise, Noise), Random.value, Random.Range(-Noise, Noise));
            FireParticles[i].Simulate(force+NoiseForce+WindForce);

            Color temp = FireParticles[i].Col;
            float GreenVal = temp.g;
            
            /* Change color of central particles from red to a more orange color to simulate
             * difference of heating between central particles and outer ones
            */ 

            if (FireParticles[i].Position.y >= Height * 0.1 && GreenVal <100f/255f)
            {                
                float alpha = temp.a;
                FireParticles[i].Col = new Vector4(255 / 255f, 100 / 255f, 7 / 255f, alpha);
            }

            if (FireParticles[i].Position.y > Height * 0.15 && GreenVal < 123f / 255f)
            {                
                float alpha = temp.a;
                FireParticles[i].Col = new Vector4(255 / 255f, 130 / 255f, 7 / 255f, alpha);
            }

            // Fade opacity of particles depending on their height

            if (FireParticles[i].Position.y > Height * 0.5f && FireParticles[i].Col.a >= 1.75f)
            {                
                float alpha = temp.a;
                alpha = alpha - 0.5f;
                temp.a = alpha;
                FireParticles[i].Col = temp;
            }

            if (FireParticles[i].Position.y > Height * 0.65f && FireParticles[i].Col.a >= 1.5f)
            {
                float alpha = temp.a;
                alpha = alpha - 0.25f;
                temp.a = alpha;
                FireParticles[i].Col = temp;
            }

            if (FireParticles[i].Position.y > Height * 0.8f && FireParticles[i].Col.a >= 1.25f)
            {
                float alpha = temp.a;
                alpha = alpha - 0.25f;
                temp.a = alpha;
                FireParticles[i].Col = temp;
            }

            if (FireParticles[i].Position.y > Height * 0.9f && FireParticles[i].Col.a >= 1f)
            {
                float alpha = temp.a;
                alpha = alpha - 0.25f;
                temp.a = alpha;
                FireParticles[i].Col = temp;
            }

            // Reposition and reset faded particles on the fire base

            if (FireParticles[i].Position.y >= Height)
            {
                Vector2 pos = Random.insideUnitCircle * SpawnRadius;
                Color col = new Vector4(255 / 255f, 130 / 255f, 7 / 255f, 2f);
                if (Mathf.Abs(pos.x) < SpawnRadius * 0.5f && Mathf.Abs(pos.y) < SpawnRadius * 0.5f)
                    col = new Vector4(255 / 255f, 70 / 255f, 7 / 255f, 2f);
                FireParticles[i] = new Particle(new Vector3(transform.position.x + pos.x, 0, transform.position.z + pos.y),
                                                new Vector3(0, Random.Range(0, 0.05f), 0), ParticleSize, ParticleMass, col, 0f, 2.5f);
            }
        }
    }

    void OnRenderObject()
    {
        for (int i = 0; i < FireParticles.Length; i++)
        {
            FireParticles[i].Draw(FireParticles[i].Size, FireParticles[i].Col);
        }

    }

}
