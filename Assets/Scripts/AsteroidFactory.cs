using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AsteroidFactory : MonoBehaviour
{
    public GameObject SmallAsteroid = null;
    public int SmallAsteroidChance = 90;
    public GameObject MediumAsteroid = null;
    public int MediumAsteroidChance = 9;
    public GameObject BigAsteroid = null;
    public int BigAsteroidChance = 1;

    public float SpawnSpeed = 5f;
    public float AsteroidSpeed = 100f;
    public float RotationForce = 5f;

    public float MinX = 100f;
    private float X = 0;
    private float RandX = 1000f;


    public Text DistanceText = null;
    public Text HighScoreText = null;

    private float DivideFactor = 10f;
    private bool IsTimeToSpawn = true;
    private Transform ThisTransform = null;
    private BoxCollider2D ThisCollider = null;
    private int Distance = 0;
    private int HighScore = 0;
    private int Chance = 0;
    private List<AsteroidChance> AsteroidBag = new List<AsteroidChance>();

    private struct AsteroidChance
    {
        public int Id;
        public GameObject Value;

        public AsteroidChance(int id, GameObject val)
        {
            Id = id;
            Value = val;
        }
    }

    private void Awake()
    {
        ThisTransform = GetComponent<Transform>();
        ThisCollider = GetComponent<BoxCollider2D>();
        RandX = ThisCollider.size.x / 2;

        DistanceText = DistanceText.GetComponent<Text>();
        HighScoreText = HighScoreText.GetComponent<Text>();

        if (PlayerPrefs.HasKey("High_Score")) {
            HighScore = PlayerPrefs.GetInt("High_Score");
            HighScoreText.text = HighScore.ToString() + " Km";
        }

        Chance = SmallAsteroidChance + MediumAsteroidChance + BigAsteroidChance;
        
        for ( int i = 0 ; i < SmallAsteroidChance; i++ )
        {
            AsteroidChance a = new AsteroidChance
            {
                Id = AsteroidBag.Count,
                Value = SmallAsteroid
            };
            AsteroidBag.Add(a);
        }

        for (int i = 0 ; i < MediumAsteroidChance; i++)
        {
            AsteroidChance a = new AsteroidChance
            {
                Id = AsteroidBag.Count,
                Value = MediumAsteroid
            };
            AsteroidBag.Add(a);
        }

        for (int i = 0 ; i < BigAsteroidChance; i++)
        {
            AsteroidChance a = new AsteroidChance
            {
                Id = AsteroidBag.Count,
                Value = BigAsteroid
            };
            AsteroidBag.Add(a);
        }
    }

    void OnApplicationQuit()
    {
        ReSetScore();
    }

    void Update()
    {
        SpawnEnemy();
    }

    void SpawnEnemy()
    {
        if (IsTimeToSpawn)
        {
            IsTimeToSpawn = false;
            if(Distance % 50 == 0) { DivideFactor += 1; }
            Invoke("TimeToSpawn",SpawnSpeed/DivideFactor);
            GameObject Asteroid = Instantiate(AsteroidBag[UnityEngine.Random.Range(0,Chance)].Value);
            Asteroid.transform.parent = ThisTransform;
            SetNewX();
            float Y = -Asteroid.transform.localScale.y * 2.5f;
            Asteroid.transform.localPosition = new Vector3( X, Y, 0 );
            Asteroid.GetComponent<Rigidbody2D>().AddForce( new Vector2( 0, -(AsteroidSpeed + Distance)), ForceMode2D.Impulse);
            Asteroid.GetComponent<Rigidbody2D>().AddTorque(UnityEngine.Random.Range(-RotationForce, RotationForce), ForceMode2D.Impulse);
        }
        return;
    }

    void SetNewX()
    {
        float NewX = UnityEngine.Random.Range(-RandX, RandX);
        if (Distance % 3 == 0) { NewX = UnityEngine.Random.Range(-MinX/2, MinX/2); }

        while ( Mathf.Abs(X - NewX) < MinX )
        {
            NewX = UnityEngine.Random.Range(-RandX, RandX);
        }

        X = NewX;
    }
    void TimeToSpawn()
    {
        IsTimeToSpawn = true;
        Distance += 1;

        if (HighScore < Distance) {
            PlayerPrefs.SetInt("High_Score",Distance);
            HighScore = Distance;
            HighScoreText.text = HighScore.ToString() + " Km";
        }

        DistanceText.text = Distance.ToString() + " Km";
        return;
    }
    public void ReSetScore()
    {
        if (PlayerPrefs.HasKey("High_Score")) { PlayerPrefs.SetInt("High_Score", 0); }
        HighScore = 0;
        HighScoreText.text = HighScore.ToString() + " Km";
    }
}
