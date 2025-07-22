using UnityEngine;
using SpacetimeDB;
using SpacetimeDB.Types;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

public class SpacetimeManager : MonoBehaviour
{
    // Define the server URI and module name
    private const string SERVER_URI = "http://127.0.0.1:3000"; // Replace with your server URI
    private const string MODULE_NAME = "mmorpg"; // Replace with your module name

    // Define some methods to handle connection events
    public static event Action OnConnected;
    public static event Action OnSubscriptionApplied;

    // Define a singleton instance of SpacetimeManager
    public static SpacetimeManager Instance { get; private set; }

    // Define Identity and DbConnection properties
    public static Identity LocalIdentity { get; private set; }
    public DbConnection Conn { get; private set; }



    private void Awake()
    {
        Instance = this;

        // Don't destroy this object on scene load
        DontDestroyOnLoad(gameObject);
    }


    // Update is called once per frame
    void Update()
    {

    }

    // Connect function
    public async Task Connect()
    {
        // Connect to the SpacetimeDB server
        try
        {
            // In order to build a connection to SpacetimeDB we need to register our callbacks and specify a server URI and module name
            var builder = DbConnection.Builder()
                .OnConnect(HandleConnect)
                .OnConnectError(HandleConnectError)
                .OnDisconnect(HandleDisconnect)
                .WithUri(SERVER_URI) // Replace with your server URI
                .WithModuleName(MODULE_NAME); // Replace with your module name

            // If the user has a SpacetimeDB auth token stored in the Unity PlayerPrefs, we can use it to authenticate the connection.

            if (AuthToken.Token != "")
            {
                builder = builder.WithToken(AuthToken.Token);
            }


            // Building the connection will establish a connection to the SpacetimeDB server.
            Conn = builder.Build();
            Debug.Log("SpacetimeManager: Building connection to SpacetimeDB...");
        }
        catch
        {
            Debug.LogError("SpacetimeManager: Failed to connect to SpacetimeDB. Please check your server URI and module name.");
        }
    }

    // HandleConnect is called when we connect to SpacetimeDB and recieve our client identity
    void HandleConnect(DbConnection _conn, Identity identity, string token)
    {
        Debug.Log("🟢 Connected to SpacetimeDB");
        AuthToken.SaveToken(token);
        LocalIdentity = identity;

        // Invoke the OnConnected event
        OnConnected?.Invoke();

        // Subscribe to the login tables: account and userNotification
        Debug.Log("SpacetimeManager: Subscribing to login tables...");
        Conn.SubscriptionBuilder()
            .OnApplied(HandleSubscriptionApplied)
            .Subscribe(new string[] {
                "SELECT * FROM account", "SELECT * FROM  userNotification where identity = '" + LocalIdentity + "'"
            });
    }

    void HandleConnectError(Exception ex)
    {
        Debug.LogError($"Connection error: {ex}");
    }

    void HandleDisconnect(DbConnection _conn, Exception ex)
    {

        Debug.Log("Disconnected.");
        if (ex != null)
        {
            Debug.LogException(ex);
        }
    }

    private void HandleSubscriptionApplied(SubscriptionEventContext ctx)
    {
        Debug.Log("🟢 Subscription applied!");
        OnSubscriptionApplied?.Invoke();
    }

    public bool IsConnected()
    {
        return Instance != null && Conn.IsActive;
    }

    public async Task Disconnect()
    {
        // Call the disconnect reducer to handle the disconnection
        Conn.Reducers.Logout();

        // Wait for the disconnection to be processed
        await Task.Delay(1000); // [CHECK] Adjust the delay as needed or create a ReducerEventHandler for logout to ensure the disconnection is processed before closing the connection
        // Close the connection
        Conn.Disconnect();
        Conn = null;
    }
    
    void OnApplicationQuit() // This method is called when the application is quitting
    {
        if (IsConnected())
        {
            Disconnect();
        }
    }
}
