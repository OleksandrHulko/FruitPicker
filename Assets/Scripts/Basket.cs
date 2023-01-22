using System;
using UnityEngine;

public class Basket : MonoBehaviour
{
    #region Serialize Fields
    [SerializeField]
    private Transform[] transforms = new Transform[SEATS_COUNT];
    #endregion

    #region Private Fields
    private Quaternion defaultRotationInBasket = Quaternion.Euler(0.0f, -90.0f, 0.0f);
    private const int SEATS_COUNT = 15;
    private int number_of_place = 0;
    #endregion

    #region Public Fields
    public static Basket instance = null;
    public static Action<FruitType> onPutFruit = null;
    #endregion
    
    
    #region Private Methods
    private void Awake()
    {
        LevelInfo.onWin += ResetBasket;
        instance = this;
    }

    private void OnDestroy()
    {
        LevelInfo.onWin -= ResetBasket;
    }

    private void ResetBasket( bool _ )
    {
        number_of_place = 0;
    }
    #endregion

    #region Public Methods
    public void PutFruit( Fruit fruit )
    {
        fruit.transform.SetParent(transforms[number_of_place++]);
        fruit.transform.localPosition = Vector3.zero;
        fruit.transform.localRotation = defaultRotationInBasket;
        
        onPutFruit?.Invoke(fruit.fruitType);
    }
    #endregion
}
