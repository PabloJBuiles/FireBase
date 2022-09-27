using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;

public class AuthManager : MonoBehaviour
{
    public Text logText;

    public Button SignInButton, SignUpButton;

    public InputField email, password, userName;
    
    // Start is called before the first frame update
    void Start()
    {
        Firebase.FirebaseApp.CheckDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            Firebase.DependencyStatus dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {

            }
            else
            {
                Debug.LogError("No se pudieron resolver las depedencias de firabase" + dependencyStatus);
            }
        });
    }

    public void OnClickSignUp()
    {
        Debug.Log("Click signUp");
        if (userName.text != null)
        {
            RegisterUser();
        }
        else
        {
            logText.text = "invalid username";
        }
    }

    private void RegisterUser()
    {
        FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(email.text, password.text)
            .ContinueWithOnMainThread(
                task =>
                {
                    if (task.IsCanceled)
                    {
                        Debug.Log("signUp Canceled");
                        logText.text = "SignUp canceled.";
                        return;
                    }

                    if (task.IsFaulted)
                    {
                        Debug.Log("SignUp Fault");
                        logText.text = task.Exception.ToString();
                        return;
                    }

                    Debug.Log("Fin signUP");
                    Firebase.Auth.FirebaseUser newUser = task.Result;
                    UserData data = new UserData();
                    data.username = userName.text;
                    string json = JsonUtility.ToJson(data);
                    FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(task.Result.UserId)
                        .SetRawJsonValueAsync(json);
                    
                    Debug.LogFormat("Usuario creado: {0} ({1})", newUser.DisplayName, newUser.UserId);
                });
    }

    public void OnClickSignIn()
    {
        Debug.Log("Click signIn");
        FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(email.text, password.text)
            .ContinueWith(
                task =>
                {
                    if (task.IsCanceled)
                    {
                        Debug.Log("signIN Canceled");
                        logText.text = "signIN canceled.";
                        return;
                    }

                    if (task.IsFaulted)
                    {
                        Debug.Log("signIN Fault");
                        logText.text = task.Exception.ToString();
                        return;
                    }

                    Debug.Log("Fin signIN");
                    Firebase.Auth.FirebaseUser newUser = task.Result;
                    Debug.LogFormat("Usuario Iniciando: {0} ({1})", newUser.DisplayName, newUser.UserId);
                });
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
