using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Private Fields
    private Vector3    startPosition = Vector3.zero;
    private Quaternion startRotation = Quaternion.identity;

    private Vector3    endPosition = new Vector3(-2.353f, 3.202f, 0.655f);
    private Quaternion endRotation = Quaternion.Euler(45.0f, -270.0f, 0.0f);
    #endregion


    #region Private Methods
    private void Start()
    {
        InitStartPositionAndRotation();
        LevelInfo.onWin += OnWinHandler;
    }

    private void OnDestroy()
    {
        LevelInfo.onWin -= OnWinHandler;
    }

    private void OnWinHandler( bool is_win )
    {
        if (is_win)
            StartCoroutine(Move());
    }

    private void InitStartPositionAndRotation()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    private IEnumerator Move()
    {
        float counter = 0.0f;
        float speedCoeff = 1.5f;
        
        while (counter < 1.0f)
        {
            init();
            counter += Time.deltaTime * speedCoeff;
            yield return null;
        }

        yield return new WaitForSeconds(1.0f);
        
        while (counter > 0.0f)
        {
            init();
            counter -= Time.deltaTime * speedCoeff;
            yield return null;
        }
        
        void init()
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, counter);
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, counter);
        }
    }
    #endregion
}
