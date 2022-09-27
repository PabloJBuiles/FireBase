using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;

public class UsernameLabel : MonoBehaviour
{
    [SerializeField] private Text _userNameText;

    private void Reset()
    {
        _userNameText = GetComponent<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        FirebaseAuth.DefaultInstance.StateChanged += HandleAuthChange;
    }

    private void HandleAuthChange(object sender, EventArgs e)
    {
     
        var currentUser = FirebaseAuth.DefaultInstance.CurrentUser;
        string username;
        if (currentUser == null)
        {
            username = "NULL";
        }
        else
        {
            username = currentUser.UserId;
            GetUserName(username);

        }

       
        
        Debug.Log("Email:  " + currentUser.Email);
    }

    private void GetUserName(string UserID)
    {
        FirebaseDatabase.DefaultInstance.GetReference("users/" + UserID + "/username").GetValueAsync()
            .ContinueWithOnMainThread(
                task =>
                {
                    if (task.IsFaulted)
                    {
                        Debug.Log(task.Exception);
                    }
                    else if (task.IsCompleted)

                    {
                        DataSnapshot snapshot = task.Result;
                        _userNameText.text = snapshot.Value.ToString();
                        GameManager.UserName = snapshot.Value.ToString();
                    }
                });
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
