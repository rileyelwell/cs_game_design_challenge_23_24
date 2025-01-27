using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagManager : MonoBehaviour
{
    public static string PLAYER_TAG = "Player";

    // For scene navigation
    public static string GAMEPLAY_SCENE_TAG = "MainGame";
    public static string TITLE_SCREEN_SCENE_TAG = "TitleScreen";
    public static string LOADING_SCREEN_TAG = "LoadingScreen";

    // For decribing objects and collisions
    public static string PEDESTRIAN_TAG = "Pedestrian";
    public static string VEHICLE_TAG = "Vehicle";
    public static string TRAIN_TAG = "Train";
}
