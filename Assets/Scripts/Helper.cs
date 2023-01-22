using UnityEngine;


public static class Helper
{
    #region Static Methods
    public static void SetRotationY( this Transform transform, float y)
    {
        Vector3 eulerAngles = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(eulerAngles.x, y, eulerAngles.z);
    }

    public static void SetPositionZ( this Transform transform, float z)
    {
        Vector3 position = transform.position;
        transform.position = new Vector3(position.x, position.y, z);
    }

    public static Vector3 SetY(this Vector3 vector3, float y)
    {
        return new Vector3(vector3.x, y, vector3.z);
    }

    public static void Show(this CanvasGroup canvasGroup, bool show = true)
    {
        canvasGroup.alpha = show ? 1.0f : 0.0f;
        canvasGroup.blocksRaycasts = show;
    }

    public static void PlayAudioClip(this AudioSource audioSource, AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
    #endregion
}
