using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance;
    public DatabaseReference databaseReference;

    private void Awake(){
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Initialization();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Initialization(){
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        });
    }
}
