
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] string _androidGameId;
    [SerializeField] string _iOsGameId;
    [SerializeField] bool _testMode = true;
    private string _gameId;
    

    void Awake()
    {
        InitializeAds();
    }

    public void InitializeAds()
    {
        

#if UNITY_IOS
        _gameId = _iOsGameId;
#elif UNITY_ANDROID
		 _gameId = _androidGameId;
#endif

        Advertisement.Initialize (_gameId, _testMode, this);
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }


    
    

}

