using UnityEngine;

public class SosyalMedya : MonoBehaviour
{
    public void Instagram()
    {
        Application.OpenURL("https://www.instagram.com/omeryavus/");
    }

    public void Youtube()
    {
        Application.OpenURL("https://www.youtube.com/@Omeryavus");
    }

    public void RateUs()
    {
#if UNITY_ANDROID
        string playStoreURL = "https://play.google.com/store/apps/details?id=com.Omeryavus.Fortuna";
        Application.OpenURL(playStoreURL);
#elif UNITY_IOS
        string appID = "";
        Application.OpenURL("itms-apps://itunes.apple.com/app/id" + appID);
#else
        string playStoreURL = "https://play.google.com/store/apps/details?id=com.Omeryavus.Fortuna";
        Application.OpenURL(playStoreURL);
#endif
    }
}
