using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class FireflieSpawner : MonoBehaviour
{

    public static FireflieSpawner instance { get; private set; }

    [SerializeField]
    private GameObject firefliePrefab;

    [SerializeField]
    private int maxFireflies = 8;

    private List<FireflieController>    fireflies = new List<FireflieController>();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {/*
        if (Input.GetKeyDown(KeyCode.S))
        {
            AddFireflie();
        }

        // if is not happening nothing...
        if (Input.GetKeyDown(KeyCode.W))
        {
            StopFireflies();
        }
        */
    }


    public void AddFireflie()
    {
        //print("AddFireflie()");
        if (fireflies.Count >= maxFireflies)
            return;

        FireflieController fireflie = Instantiate(firefliePrefab, new Vector3(0f, 0f, 0f), Quaternion.identity).GetComponent<FireflieController>();
        fireflies.Add( fireflie );
    }

    public void RemoveFireflie()
    {
        if (fireflies.Count == 0)
            return;

        fireflies[0].Hide();
    }


    public void StopFireflies()
    {
        //print("StopFireflies()");
        foreach (FireflieController fireflie in fireflies)           
        {
            if (!fireflie.IsHiding())
                fireflie.Hide();
        }
    }

    public void    CanDestroyFirefile(FireflieController fireflie)
    {
        //print("CanDestroyFireflie()");
        Destroy(fireflie.gameObject);
        fireflies.Remove(fireflie);        
    }

    public int  GetMaxFireflies() => maxFireflies;
    public int GetCurrentActiveFireflies() => fireflies.Count;
}
