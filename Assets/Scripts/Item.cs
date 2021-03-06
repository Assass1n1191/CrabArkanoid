﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Shrimp1,
    Shrimp2,
    Shrimp3,
    ShrimpPack,
    SeaUrchin,
    Clock,
    X2Bonus,
    Shark
}

public class Item : MonoBehaviour 
{
    public ItemType Type;
    public float SpawnProbability;
    public float Score;

    private float groundY = -9f;
    private GameObject _bubble;

    private Vector2 _posInField;

    public bool IsOnTheGround = false;

	private void Awake () 
	{
		
	}

	private void Start () 
	{
        _bubble = transform.GetChild(0).gameObject;
	}
	
	private void Update () 
	{

    }

    public void OnBallHit()
    {
        FieldController.Instance.ItemIsDestroyed(_posInField);
        switch (Type)
        {
            case ItemType.Shrimp1:
                GetScore();
                break;
            case ItemType.Shrimp2:
                GetScore();
                break;
            case ItemType.Shrimp3:
                GetScore();
                break;
            case ItemType.ShrimpPack:
                GetScore();
                break;
            case ItemType.Shark:
                GetScore();
                break;
            case ItemType.SeaUrchin:
                GameScreen.Instance.ResetToInitState();
                Destroy();
                break;
            case ItemType.Clock:
                Fall();
                break;
            case ItemType.X2Bonus:
                Fall();
                break;
        }
    }

    public void MoveDown(float desinationY, int posCount)
    {
        _posInField.y -= posCount;
        iTween.MoveTo(gameObject, new Vector2(transform.position.x, desinationY/*transform.position.y - verticalDistance*/), 1f);
    }

    public void Fall()
    {
        IsOnTheGround = true;

        iTween.ColorTo(_bubble, new Color(1f, 1f, 1f, 0f), 0.2f);
        iTween.ScaleTo(_bubble, new Vector3(1.2f, 1.2f, 1.2f), 0.2f);
        iTween.MoveTo(gameObject, iTween.Hash("position", new Vector3(transform.position.x, groundY, 0f),
                                              "delay", 0.1f,
                                              "speed", 3.2f,
                                              "easetype", iTween.EaseType.linear));

        StartCoroutine(DisappearFromTheGround());
    }

    public void GetScore()
    {
        GetComponent<Collider2D>().enabled = false;
        GameScreen.Instance.ChangeScore(Score);

        iTween.ColorTo(_bubble, new Color(1f, 1f, 1f, 0f), 0.2f);
        iTween.ScaleTo(_bubble, new Vector3(1.2f, 1.2f, 1.2f), 0.2f);
        iTween.MoveTo(gameObject, iTween.Hash("position", new Vector3(transform.position.x, transform.position.y + 1f, 0f),
                                              "delay", 0.1f,
                                              "time", 1f));
        iTween.ColorTo(gameObject, iTween.Hash("color", new Color(1f, 1f, 1f, 0f),
                                               "oncomplete", "Destroy",
                                               "delay", 0.3f,
                                               "time", 0.5f));
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void SetUpItemInField(Vector2 posInField)
    {
        _posInField = posInField;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag.Equals(Tags.Crab))
        {
            switch (Type)
            {
                case ItemType.Clock:
                    GameScreen.Instance.ChangeTime(5);
                    Destroy(gameObject);
                    break;
                case ItemType.X2Bonus:
                    GameScreen.Instance.SetMultiplierBonus(2, 10f);
                    Destroy(gameObject);
                    break;
            }
        }
    }

    private IEnumerator DisappearFromTheGround()
    {
        yield return new WaitForSeconds(2f);
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        for(int i = 0; i < 2; i++)
        {
            sr.color = new Color(1f, 1f, 1f, 0.5f);
            yield return new WaitForSeconds(1f);
            sr.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(1f);
        }

        Destroy(gameObject);
    }
}
