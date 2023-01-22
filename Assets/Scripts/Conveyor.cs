using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Conveyor : MonoBehaviour
{
    #region Serialize Fields
    [SerializeField]
    private Transform surface = null;
    
    [SerializeField]
    private Transform spawner = null;
    
    [SerializeField]
    private Material material = null;
    
    [SerializeField]
    private GameObject apple = null;
    [SerializeField]
    private GameObject banana = null;
    [SerializeField]
    private GameObject lemon = null;

    [Range(0.2f, 0.9f)] 
    [SerializeField] 
    private float width = 0.0f;
    #endregion

    #region Private Fields
    private const float SPAWN_TIME = 0.5f;
    
    private const int MAX_SINGLE_FRUIT_COUNT = 5;                                   // count of specific type of fruit in basket OR  on conveyor
    private const int MAX_ALL_SINGLE_FRUIT_COUNT = (MAX_SINGLE_FRUIT_COUNT * 2) + 1;// count of specific type of fruit in basket AND on conveyor SIMULTANEOUSLY

    private int fruitsTypesCount = 0;
    
    private GameObject[,] fruits = null;
    #endregion

    #region Public Fields
    public static float speed = 1.0f;
    public static float сonveyorLenght = 0.0f;
    #endregion


    #region Private Methods
    private IEnumerator Start()
    {
        LevelInfo.onWin += ResetFruit;
        
        SetConveyorLenght();
        InitPool();

        yield return FruitSpawner();
    }

    private void OnDestroy()
    {
        LevelInfo.onWin -= ResetFruit;
    }
    
    private void FixedUpdate()
    {
        MoveTexture();
    }

    private void SetConveyorLenght()
    {
        сonveyorLenght = surface.lossyScale.z;
    }

    private void InitPool()
    {
        fruitsTypesCount = Enum.GetValues(typeof(FruitType)).Length;
        fruits = new GameObject[fruitsTypesCount, MAX_ALL_SINGLE_FRUIT_COUNT];

        for (int i = 0; i < fruitsTypesCount; i++)
            for (int j = 0; j < MAX_ALL_SINGLE_FRUIT_COUNT; j++)
                fruits[i, j] = Instantiate(GetFruit((FruitType) i), spawner);
    }

    private GameObject GetFruit(FruitType fruitType)
    {
        switch (fruitType)
        {
            case FruitType.APPLE  : return apple;
            case FruitType.BANANA : return banana;
            case FruitType.LEMON  : return lemon;
            
            default: return null;
        }
    }

    private void MoveTexture()
    {
        material.mainTextureOffset = new Vector2(0.0f, speed / сonveyorLenght * Time.time);
    }

    private IEnumerator FruitSpawner()
    {
        int appleCounter  = 0;
        int bananaCounter = 0;
        int lemonCounter  = 0;
        
        while (true)
        {
            yield return new WaitForSeconds(SPAWN_TIME);
            spawn();
        }

        void spawn()
        {
            FruitType fruitType = (FruitType) Random.Range(0, fruitsTypesCount);

            switch (fruitType)
            {
                case FruitType.APPLE: SpawnFruit((int) fruitType, ref appleCounter); 
                    break;
                
                case FruitType.BANANA: SpawnFruit((int) fruitType, ref bananaCounter); 
                    break;
                
                case FruitType.LEMON: SpawnFruit((int) fruitType, ref lemonCounter); 
                    break;
            }
        }
    }

    private void SpawnFruit(int fruitType, ref int counter)
    {
        for (int i = 0; i < MAX_ALL_SINGLE_FRUIT_COUNT; i++)
        {
            if (!fruits[fruitType, counter].activeSelf)
                break;
            
            IncrementCounter(ref counter);
        }

        GameObject fruit = fruits[fruitType, counter];

        fruit.transform.localPosition = new Vector3(Random.Range(-width / 2, width / 2), 0.0f, 0.0f);
        fruit.transform.SetRotationY(Random.Range(0.0f, 360.0f));

        fruit.SetActive(true);
        
        IncrementCounter(ref counter);

        void IncrementCounter(ref int counterLocal)
        {
            if (++counterLocal == MAX_ALL_SINGLE_FRUIT_COUNT)
                counterLocal = 0;
        }
    }

    private void ResetFruit( bool _ )
    {
        StopCoroutine(FruitSpawner());
        
        foreach (GameObject fruit in fruits)
        {
            fruit.transform.parent = spawner;
            fruit.GetComponent<Fruit>().is_picked = false;
            fruit.SetActive(false);
        }
    }
    #endregion
}

#region Enum
public enum FruitType : byte
{
    APPLE = 0,
    BANANA ,
    LEMON
}
#endregion
