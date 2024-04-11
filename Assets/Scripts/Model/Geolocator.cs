//pass to DataService
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using System.Runtime.CompilerServices;


public class Geolocator
{
    private const string TomtomApiKey = "9ZhsAfWEkUAmAjxSLaF2rNz9kAVWhoht";
    
    public async Task<(float latitude, float longitude)> GetGeoLocationAsync()
    {
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("Location services are not enabled");
            return (52.520194f,13.404923f);
        }

        Input.location.Start();

        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            await Task.Delay(1000);
            maxWait--;
        }

        if (maxWait < 1 || Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Failed to get device location.");
            Input.location.Stop();
            return (0, 0);
        }

        float latitude = Input.location.lastData.latitude;
        float longitude = Input.location.lastData.longitude;

        Input.location.Stop();

        return (latitude, longitude);
    }

    public async Task<string> GetStreetNameFromGeoLocationAsync(float latitude, float longitude)
    {
        string latitudeAsString = latitude.ToString().Replace(",", ".");
        string longitudeAsString = longitude.ToString().Replace(",", ".");
        string url = $"https://api.tomtom.com/search/2/reverseGeocode/{latitudeAsString},{longitudeAsString}.json?key={TomtomApiKey}&radius=100";
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log($"Error while fetching street name: {request.error}");
                return string.Empty;
            }
            else
            {
                string json = request.downloadHandler.text;
                GeolocationData geolocationData = JsonUtility.FromJson<GeolocationData>(json);
                return geolocationData != null && geolocationData.addresses.Length > 0 ? geolocationData.addresses[0].address.streetName : string.Empty;
            }
        }
    }


    [System.Serializable]
    public class Address
    {
        public string streetName;
    }

    [System.Serializable]
    public class Result
    {
        public Address address;
        public string position;
        public string id;
    }

    [System.Serializable]
    public class GeolocationData
    {
        public Result[] addresses;
    }
}

public static class UnityWebRequestExtensions
{
    public static TaskAwaiter GetAwaiter(this UnityWebRequestAsyncOperation asyncOp)
    {
        var completionSource = new TaskCompletionSource<object>();
        asyncOp.completed += _ => completionSource.TrySetResult(null);
        return ((Task)completionSource.Task).GetAwaiter();
    }
}