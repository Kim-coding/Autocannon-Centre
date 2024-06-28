using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;

public class GooglePlayManager : MonoBehaviour
{
    public void ShowAchievementUI() => Social.ShowAchievementsUI();

    public void UnlockAchievement() => PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement, 1, (bool success) => { });
}
