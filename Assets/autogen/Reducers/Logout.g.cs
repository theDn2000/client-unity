// THIS FILE IS AUTOMATICALLY GENERATED BY SPACETIMEDB. EDITS TO THIS FILE
// WILL NOT BE SAVED. MODIFY TABLES IN YOUR MODULE SOURCE CODE INSTEAD.

// This was generated using spacetimedb cli version 1.2.0 (commit fb41e50eb73573b70eea532aeb6158eaac06fae0).

#nullable enable

using System;
using SpacetimeDB.ClientApi;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SpacetimeDB.Types
{
    public sealed partial class RemoteReducers : RemoteBase
    {
        public delegate void LogoutHandler(ReducerEventContext ctx);
        public event LogoutHandler? OnLogout;

        public void Logout()
        {
            conn.InternalCallReducer(new Reducer.Logout(), this.SetCallReducerFlags.LogoutFlags);
        }

        public bool InvokeLogout(ReducerEventContext ctx, Reducer.Logout args)
        {
            if (OnLogout == null)
            {
                if (InternalOnUnhandledReducerError != null)
                {
                    switch (ctx.Event.Status)
                    {
                        case Status.Failed(var reason): InternalOnUnhandledReducerError(ctx, new Exception(reason)); break;
                        case Status.OutOfEnergy(var _): InternalOnUnhandledReducerError(ctx, new Exception("out of energy")); break;
                    }
                }
                return false;
            }
            OnLogout(
                ctx
            );
            return true;
        }
    }

    public abstract partial class Reducer
    {
        [SpacetimeDB.Type]
        [DataContract]
        public sealed partial class Logout : Reducer, IReducerArgs
        {
            string IReducerArgs.ReducerName => "Logout";
        }
    }

    public sealed partial class SetReducerFlags
    {
        internal CallReducerFlags LogoutFlags;
        public void Logout(CallReducerFlags flags) => LogoutFlags = flags;
    }
}
