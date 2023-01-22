using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    #region Serialize Fields
    [SerializeField]
    private Transform target = null;
    [SerializeField]
    private Text[] textsOnBasket = null;
    [SerializeField]
    private LevelInfo levelInfo = null;
    [SerializeField]
    private Menu menu = null;
    #endregion

    #region Private Fields
    private int  idx_of_texts = 0;
    private bool is_win       = true;
    #endregion
    

    #region Private Methods
    private void Start()
    {
        levelInfo.SetNewLevelInfo();
        
        Basket.onPutFruit += OnPutFruitHandler;
        LevelInfo.onWin   += OnWinHandler;
        Menu.onLevelStart += OnLevelStartHandler;
    }

    private void OnDestroy()
    {
        Basket.onPutFruit -= OnPutFruitHandler;
        LevelInfo.onWin   -= OnWinHandler;
        Menu.onLevelStart -= OnLevelStartHandler;
    }

    private void OnPutFruitHandler( FruitType fruitType )
    {
        AddTooltipFlyUp(fruitType);
        levelInfo.UpdateLevelInfo(fruitType);
    }

    private void OnWinHandler( bool is_win )
    {
        this.is_win = is_win;
        
        menu.Init(is_win);
        levelInfo.Show(false);
    }

    private void OnLevelStartHandler()
    {
        if (is_win)
            levelInfo.SetNewLevelInfo();
        else
            levelInfo.SetOldLevelInfo();
    }

    private void AddTooltipFlyUp( FruitType fruitType )
    {
        Text textOnBasket = textsOnBasket[idx_of_texts];
        textOnBasket.text = getText();
        
        increment(ref idx_of_texts);
        
        StartCoroutine(flyAndHide());

        IEnumerator flyAndHide()
        {
            float speedFlyUp      = 0.3f;
            float flyTime         = 1.0f;
            float hideTime        = 0.2f;
            float currentTime     = Time.time;
            float currentDistance = 0.0f;
            float localScale      = 1.0f;
            
            resetScale();

            while (localScale != 0.0f)
            {
                flyUp();
                if (currentTime + flyTime < Time.time)
                    hide();

                yield return null;
            }

            void flyUp()
            {
                currentDistance += speedFlyUp * Time.deltaTime;
                Vector3 screenPointPos = Camera.main.WorldToScreenPoint(target.position.SetY(target.position.y + currentDistance ));
                
                textOnBasket.rectTransform.position = screenPointPos;
            }

            void hide()
            {
                localScale = Mathf.Clamp(localScale - Time.deltaTime / hideTime, 0.0f, 1.0f);
                textOnBasket.transform.localScale = Vector3.one * localScale;
            }

            void resetScale()
            {
                textOnBasket.transform.localScale = Vector3.one;
            }
        }

        string getText()
        {
            switch (fruitType)
            {
                case FruitType.APPLE:  return "+1 Apple";
                case FruitType.BANANA: return "+1 Banana";
                case FruitType.LEMON:  return "+1 Lemon";
                
                default: return string.Empty;
            }
        }
        
        void increment(ref int index)
        {
            if (++index == textsOnBasket.Length)
                index = 0;
        }
    }
    #endregion
}
