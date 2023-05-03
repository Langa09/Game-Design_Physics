using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuckScript : MonoBehaviour
{

    public ScoreScript ScoreScriptInstance;
    public static bool WasGoal { get; private set; }
    public float MaxSpeed;

    private Rigidbody2D rb;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        WasGoal = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!WasGoal)
        {
            if (other.tag == "AiGoal")
            {
                ScoreScriptInstance.Increment(ScoreScript.Score.PlayerScore);
                WasGoal = true;
                StartCoroutine(ResetPuck(false));
            }
            else if (other.tag == "PlayerGoal")
            {
                ScoreScriptInstance.Increment(ScoreScript.Score.AiScore);
                WasGoal = true;

                StartCoroutine(ResetPuck(true));
            }
        }
    }

    private IEnumerator ResetPuck(bool didAiScore)
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        yield return new WaitForSecondsRealtime(1);
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<CircleCollider2D>().enabled = true;
        WasGoal = false;
        rb.velocity = rb.position = new Vector2(0, 0);

        if (didAiScore)
            rb.position = new Vector2(0, -1);
        else
            rb.position = new Vector2(0, 1);
    }

    /*
     for (int i=3;i>=0;i--) 
    {
        some text display. text = i;
        yield return new WaitForSeconds(1f);
    }
     */

    public void CenterPuck()
    {
        rb.position = new Vector2(0, 0);
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, MaxSpeed);
    }
}