using System;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    #region Private Fields
    private float distance = 0.0f;
    private float start_z_position = 0.0f;
    #endregion

    #region Public Fields
    [NonSerialized]
    public bool is_picked = false;

    public FruitType fruitType;
    #endregion


    #region Private Methods
    private void OnEnable()
    {
        distance = 0.0f;
        start_z_position = transform.position.z;
    }

    private void Update()
    {
        TryMoveFruit();
    }

    private void TryMoveFruit()
    {
        if (is_picked)
            return;
        
        distance += Time.deltaTime * -Conveyor.speed;
        transform.SetPositionZ(start_z_position + distance);

        if (Mathf.Abs(distance) >= Conveyor.сonveyorLenght)
            gameObject.SetActive(false);
    }
    #endregion
}
