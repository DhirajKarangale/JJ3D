using UnityEngine;
using FirstGearGames.SmoothCameraShaker;

public class CamController : Singleton<CamController>
{
    public ShakeData shakeData;

    public void Shake(float magnitude)
    {
        shakeData.Magnitude = magnitude;
        CameraShakerHandler.Shake(shakeData);
    }
}
