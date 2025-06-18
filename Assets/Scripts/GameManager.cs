using UnityEngine;
using SpacetimeDB;
using SpacetimeDB.Types;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    // Define variables for the game manager
    const string SERVER_URL = "http://127.0.0.1:3000";
    const string MODULE_NAME = "mmorpg";

    public static event Action OnConnected;
    public static event Action OnSubscriptionApplied;

    public float borderThickness = 2;
    public Material borderMaterial;

    public static GameManager Instance { get; private set; }
    public static Identity LocalIdentity { get; private set; }
    public static DbConnection Conn { get; private set; }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        Application.targetFrameRate = 60;

        // In order to build a connection to SpacetimeDB, we need to register our callbacks and specify a SpacetimeDB server URI and module name
        var builder = DbConnection.Builder()
            .OnConnect(HandleConnect)
            .OnConnectError(HandleConnectError)
            .OnDisconnect(HandleDisconnect)
            .WithUri(SERVER_URL)
            .WithModuleName(MODULE_NAME);

        // If the user has a SpacetimeDB auth token stored in the Unity PlayerPrefs, we can use it to authenticate connection
        if (AuthToken.Token != "")
        {
            builder = builder.WithToken(AuthToken.Token);
        }

        // Build the connection, establish a connection to the SpacetimeDB server
        Conn = builder.Build();
    }

    // HandleConnect is called when we connect to SpacetimeDB server and recieve our client identify
    void HandleConnect(DbConnection _conn, Identity identity, string token)
    {
        Debug.Log("Connected");
        AuthToken.SaveToken(token);
        LocalIdentity = identity;

        OnConnected?.Invoke();

        // Request all tables
        Conn.SubscriptionBuilder()
            .OnApplied(HandleSubscriptionApplied)
            .SubscribeToAllTables(); // Sync of all the tables
    }

    // HandleConnectError is called when there is a problem with the connection
    void HandleConnectError(Exception ex)
    {
        Debug.LogError($"Connection error: {ex}");
    }

    // HandleDisconnect is called when we disconnect from SpacetimeDB server
    void HandleDisconnect(DbConnection _conn, Exception ex)
    {
        Debug.Log("Disconnected");
        if (ex != null)
        {
            Debug.LogException(ex);
        }
    }

    // HandleSubscriptionApplied is called when the subscription is successfully made
    private void HandleSubscriptionApplied(SubscriptionEventContext ctx)
    {
        Debug.Log("Subscription applied");
        OnSubscriptionApplied?.Invoke();
    }

    // Check if the user is connected
    public static bool IsConnected()
    {
        return Conn != null && Conn.IsActive;
    }

    // Disconnect
    public void Disconnect()
    {
        Conn.Disconnect();
        Conn = null;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
