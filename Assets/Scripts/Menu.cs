using System;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    #region Serialize Fields
    [SerializeField]
    private Text description = null;
    [SerializeField]
    private Text levelBtnTxt = null;
    [SerializeField]
    private CanvasGroup canvasGroup = null;
    #endregion

    #region Private Fields
    private bool is_win = false;
    #endregion

    #region Public Methods
    public static Action onLevelStart = null;
    #endregion
    

    #region Public Methods
    public void Init( bool is_win )
    {
        this.is_win = is_win;
        
        canvasGroup.Show();
        SetDescriptionText();
        SetLevelBtnText();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void TryRunNextLevel()
    {
        canvasGroup.Show(false);
        
        onLevelStart?.Invoke();
    }
    #endregion
    
    #region Private Methods
    private void SetDescriptionText()
    {
        description.text = is_win ? "Level Passed" : "Level Not Passed";
    }

    private void SetLevelBtnText()
    {
        levelBtnTxt.text = is_win ? "Next Level" : "Try Again";
    }
    #endregion
}
