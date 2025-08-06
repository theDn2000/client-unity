using UnityEngine;
using SpacetimeDB;
using SpacetimeDB.Types;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

public class AuthManager : MonoBehaviour
{
    // This class is responsible for managing authentication-related tasks
    public static AuthManager Instance;

    // Define events for login success and error
    public event Action OnLoginSuccess;
    public event Action<string> OnLoginError;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Ensure that there is only one instance of AuthManager
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public async Task Login(string username, string passwordHash)
    {
        // This method will handle the login process
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(passwordHash))
        {
            throw new Exception("Username and password cannot be empty");
        }

        try
        {
            await SpacetimeManager.Instance.Connect();

            // Esperar a que los handlers se ejecuten
            if (!SpacetimeManager.Instance.IsConnected())
            {
                // Usar TaskCompletionSource para esperar al evento
                var tcs = new TaskCompletionSource<bool>();

                void OnConnected()
                {
                    SpacetimeManager.OnConnected -= OnConnected;
                    tcs.SetResult(true);
                }

                SpacetimeManager.OnConnected += OnConnected;

                // Esperar máximo 10 segundos
                var timeoutTask = Task.Delay(10000);
                var completedTask = await Task.WhenAny(tcs.Task, timeoutTask);

                if (completedTask == timeoutTask)
                {
                    SpacetimeManager.OnConnected -= OnConnected;
                    throw new Exception("Connection timeout");
                }
            }

            // Here we trigger the login reducer on the server, using SpacetimeManager's connection
            try
            {
                // Register the callbacks for the login reducer
                RegisterCallbacks(SpacetimeManager.Instance.Conn);

                // Trigger the login reducer with the provided username and password hash
                SpacetimeManager.Instance.Conn.Reducers.Login(username, passwordHash);
                // Automatically, the Reducer_OnLogin method will be called when the login reducer is executed

            }
            catch (Exception ex)
            {
                Debug.LogError($"Login failed: {ex.Message}");
                throw;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Server connection failed: {ex.Message}");
            throw;
        }

    }


    // Register all the callbacks our app will use to respond to database events.
    void RegisterCallbacks(DbConnection conn)
    {
        // Reducers
        SpacetimeManager.Instance.Conn.Reducers.OnLogin += Reducer_OnLogin; // When the login reducer is called, this method will be executed

        // Handlers
        SpacetimeManager.OnSubscriptionApplied += OnSubscriptionsReady;
    }

    // Callback to be executed when the login reducer is called (to know if the login was successful)
    void Reducer_OnLogin(ReducerEventContext ctx, string username, string passwordHash)
    {
        var e = ctx.Event;
        if (e.CallerIdentity == SpacetimeManager.LocalIdentity && e.Status is Status.Failed(var error))
        {
            Debug.LogError($"Login failed for {username}: {error}");
            // Trigger the OnLoginError event with the error message
            OnLoginError?.Invoke(error);

            // Close the connection if the login fails
            SpacetimeManager.Instance.Conn.Disconnect();
        }
        else
        {
            Debug.Log($"Login successful for {username}");
            // Subscribe to tables after a successful login
            SpacetimeManager.Instance.SubscribeToTables(username);
        }
    }

    async void OnSubscriptionsReady()
    {
        // Unsubscribe to avoid duplicates
        SpacetimeManager.OnSubscriptionApplied -= OnSubscriptionsReady;
        // This method is called when all subscriptions are applied and the connection is ready
        OnLoginSuccess?.Invoke();

        // In this point we need call the CharacterManager to load the characters
        await CharacterManager.Instance.GetMyCharacters();

        // Limita los fps a 60 para evitar problemas de rendimiento al cambiar de escena
        Application.targetFrameRate = 60;

        // Change the scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Scene_CharacterSelection");   
    }
}
