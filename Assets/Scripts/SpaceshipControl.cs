using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceshipControl : MonoBehaviour
{
    public Sprite ForwardMove = null;
    public Sprite RightMove = null;
    public Sprite LeftMove = null;

    private Transform ThisTransform = null;
    private SpriteRenderer ThisSprteRenderer = null;
    public float Speed = 500f;
    public float MinX = 10f;
    public float CenterDrag = 0.5f;
    private float StartX = 0f;
    private Vector3 SteerVector = new Vector3( 1f , 0f , 0f ); 
    
    void Awake()
    {
        ThisTransform = GetComponent<Transform>();
        ThisSprteRenderer = GetComponent<SpriteRenderer>();

        StartX = ThisTransform.position.x;
        SteerVector = ThisTransform.right;
    }

    void Update()
    {
        float H = Input.GetAxis("Horizontal");

        if ( H != 0 ) { Move(H); }
        if (Input.touchCount > 0)
        {
            
            if ( ( Input.GetTouch(0).position.y / Screen.height ) < 0.5f )
            {
                float W = Screen.width;
                float X = Input.GetTouch(0).position.x;
                H = (2 * X / W) - 1f;
                H = H / Mathf.Abs(H);
                Move(H);
            }
        }

        if ( H==0 ) { Recenter(); }

    }

    void Move(float H)
    {
        if ( H > 0 ) { ThisSprteRenderer.sprite = RightMove; }
        else { ThisSprteRenderer.sprite = LeftMove; }
        ThisTransform.position += Time.deltaTime * H * SteerVector * Speed;
    }

    void Recenter()
    {
        ThisSprteRenderer.sprite = ForwardMove;
        if( Mathf.Abs( ThisTransform.position.x - StartX ) >= MinX )
        {
            float D = (ThisTransform.position.x - StartX) / Mathf.Abs(ThisTransform.position.x - StartX);
            ThisTransform.position += Time.deltaTime * SteerVector * Speed * CenterDrag * -D;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Die();
    }

    private void Die()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}