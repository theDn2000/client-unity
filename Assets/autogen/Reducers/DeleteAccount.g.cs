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
        public delegate void DeleteAccountHandler(ReducerEventContext ctx);
        public event DeleteAccountHandler? OnDeleteAccount;

        public void DeleteAccount()
        {
            conn.InternalCallReducer(new Reducer.DeleteAccount(), this.SetCallReducerFlags.DeleteAccountFlags);
        }

        public bool InvokeDeleteAccount(ReducerEventContext ctx, Reducer.DeleteAccount args)
        {
            if (OnDeleteAccount == null)
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
            OnDeleteAccount(
                ctx
            );
            return true;
        }
    }

    public abstract partial class Reducer
    {
        [SpacetimeDB.Type]
        [DataContract]
        public sealed partial class DeleteAccount : Reducer, IReducerArgs
        {
            string IReducerArgs.ReducerName => "DeleteAccount";
        }
    }

    public sealed partial class SetReducerFlags
    {
        internal CallReducerFlags DeleteAccountFlags;
        public void DeleteAccount(CallReducerFlags flags) => DeleteAccountFlags = flags;
    }
}
