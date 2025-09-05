using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class FireflieSpawner : MonoBehaviour
{

    public static FireflieSpawner instance { get; private set; }


    public GameObject firefliePrefab;
    public int maxFireflies = 8;

    private int curActiveFireflies = 0;
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
        //fireflies = new GameObject[maxFireflies];
        ///int 

        AddFireflie();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            AddFireflie();
        }

        // if is not happening nothing...
        if (Input.GetKeyDown(KeyCode.W))
        {
            StopFireflies();
        }

    }


    private void AddFireflie()
    {
        //print("AddFireflie()");
        if (fireflies.Count >= maxFireflies)
            return;

        FireflieController fireflie = Instantiate(firefliePrefab, new Vector3(0f, 0f, 0f), Quaternion.identity).GetComponent<FireflieController>();
        fireflies.Add( fireflie );
    }

    private void StopFireflies()
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
        print("CanDestroyFireflie()");
        Destroy(fireflie.gameObject);
        fireflies.Remove(fireflie);        
    }
}
