using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.UI;

public class FB_ScoreController : MonoBehaviour
{

    DatabaseReference mDatabase;
    string UserId;
    public int maxLocalScore;
    private List<UserInfo> maxScoresData = new List<UserInfo>();

    [SerializeField]private Text laderBoardNames;
    [SerializeField]private Text laderBoardScores;
    UserInfo userInfo = new UserInfo();
   // private DataSnapshot snapshot;
    private bool goodTaskResult = false;
    public Dictionary<string, object> userObject;
    // Start is called before the first frame update
    void Start()
    {
        mDatabase = FirebaseDatabase.DefaultInstance.RootReference;
        UserId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        maxLocalScore = GetUserScore();


    }

    public void WriteNewScore(int score)
    {
        UserData data = new UserData();

        data.score = score;
        data.username = GameManager.UserName;
        string json = JsonUtility.ToJson(data);

        mDatabase.Child("users").Child(UserId).SetRawJsonValueAsync(json);

    }

    public int GetUserScore()
    {
        int scoreDB = 0;
        FirebaseDatabase.DefaultInstance
            .GetReference("users/"+UserId)
            .GetValueAsync().ContinueWithOnMainThread(task => {
                if (task.IsFaulted)
                {
                    Debug.Log(task.Exception);
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;

                    Debug.Log(snapshot.Value);

                    var data = (Dictionary<string, object>)snapshot.Value;

                    Debug.Log("Puntaje: " +data["score"]);
                    scoreDB = (int)data["score"];

                    /*foreach (var userDoc in (Dictionary<string,object>)snapshot.Value)
                    {
                        Debug.Log(userDoc.Key);
                        Debug.Log(userDoc.Value);

                    }*/
                    // Do something with snapshot...
                }
            });
        return scoreDB;
    }

    public void GetUsersMaxScore()
    {
        FirebaseDatabase.DefaultInstance.GetReference("users").OrderByChild("score").LimitToLast(5).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError(task.Exception);
            }
            else if (task.IsCompleted)
            {
                 List<UserInfo> maxScoresData2 = new List<UserInfo>();
                 DataSnapshot snapshot = task.Result;
                Debug.Log(snapshot);
  

                foreach (var userDoc in (Dictionary<string, object>)snapshot.Value)
                {

                    userObject = (Dictionary<string, object>)userDoc.Value;

                    Debug.Log((userObject["username"])+":"+userObject["score"]);
                    UserInfo userInfo2 = new UserInfo();
                    userInfo2.score = userObject["score"].ToString();
                    userInfo2.username = userObject["username"].ToString();
                    maxScoresData2.Add(userInfo2);
                    Debug.Log("Lista Creada con exito");
                    Debug.Log("Pinche CODIGOOO FUNCIONAAA APUES!!!AAAAAAAAAAAAAAAAAAAA");       

                }
                
           

                Debug.Log("Y ahora que passaaaaaaaa?");       
                
                laderBoardNames.text = "Names: \n";
                laderBoardScores.text = "Scores: \n";
                foreach (var VARIABLE in maxScoresData2.OrderByDescending(maxScoresData2 => maxScoresData2.score))
                {
                    laderBoardNames.text += VARIABLE.username + "\n";
                    laderBoardScores.text += VARIABLE.score + "\n";
                }
                
            }
        });
    }

    private void GetScoresTable(Dictionary<string, object> userObject)
    {
        userInfo.score = (string) userObject["score"];
        userInfo.username = (string) userObject["username"];
        maxScoresData.Add(userInfo);
        Debug.Log("Lista Creada con exito");
    }
}

public class UserData
{
    public int score;
    public string username;
}
public class UserInfo
{
    public string score;
    public string username;
}