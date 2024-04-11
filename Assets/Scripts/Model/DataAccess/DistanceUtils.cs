using UnityEngine;

public class DistanceUtils
{
    public static float CalculateDistanceBetweenTwoPoints(float lat1, float lon1, float lat2, float lon2)
    {
        float R = 6371f; //Erdradius in km
        float dLat = Deg2Rad(lat2 - lat1); 
        float dLon = Deg2Rad(lon2 - lon1);
        float a =
          Mathf.Sin(dLat / 2f) * Mathf.Sin(dLat / 2f) +
          Mathf.Cos(Deg2Rad(lat1)) * Mathf.Cos(Deg2Rad(lat2)) *
          Mathf.Sin(dLon / 2f) * Mathf.Sin(dLon / 2f)
          ;
        float c = 2f * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1f - a));
        float d = R * c;
        
        return d;
    }
    public static float Deg2Rad(float deg)
    {
       return deg * (Mathf.PI / 180f);
    }
}