using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{

    public float spawnRate = 2.0f;
    public int spawnAmount = 1;
    public float spawnDistance = 15.0f; //tha allaksei
    public float trajectoryVariance = 15.0f; // tha allaksei, eksartatai apo to panw
    public  Asteroid asteroidPrefab;


    private void Start()
    {
        InvokeRepeating(nameof(Spawn), this.spawnRate,this.spawnRate);
    }

    private void Spawn()
    {
        for (int i = 0; i < this.spawnAmount; i++)
        {
            Vector3 spawnDirection = Random.insideUnitCircle.normalized * spawnDistance; //spawn on a circle , only on the edge, * distance because we want outside of screen
            Vector3 spawnPoint = this.transform.position + spawnDirection;

            float variance = Random.Range(-this.trajectoryVariance,this.trajectoryVariance);
            Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);

            Asteroid asteroid = Instantiate(this.asteroidPrefab,spawnPoint,rotation);
            asteroid.size = Random.Range(asteroid.minSize,asteroid.maxSize);
            asteroid.SetTrajectory(rotation * -spawnDirection);
        }
    }
}
