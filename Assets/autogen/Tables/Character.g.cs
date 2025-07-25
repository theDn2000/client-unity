// THIS FILE IS AUTOMATICALLY GENERATED BY SPACETIMEDB. EDITS TO THIS FILE
// WILL NOT BE SAVED. MODIFY TABLES IN YOUR MODULE SOURCE CODE INSTEAD.

// This was generated using spacetimedb cli version 1.2.0 (commit fb41e50eb73573b70eea532aeb6158eaac06fae0).

#nullable enable

using System;
using SpacetimeDB.BSATN;
using SpacetimeDB.ClientApi;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SpacetimeDB.Types
{
    public sealed partial class RemoteTables
    {
        public sealed class CharacterHandle : RemoteTableHandle<EventContext, Character>
        {
            protected override string RemoteTableName => "character";

            public sealed class AccountIdIndex : BTreeIndexBase<uint>
            {
                protected override uint GetKey(Character row) => row.AccountId;

                public AccountIdIndex(CharacterHandle table) : base(table) { }
            }

            public readonly AccountIdIndex AccountId;

            public sealed class CharacterIdUniqueIndex : UniqueIndexBase<uint>
            {
                protected override uint GetKey(Character row) => row.CharacterId;

                public CharacterIdUniqueIndex(CharacterHandle table) : base(table) { }
            }

            public readonly CharacterIdUniqueIndex CharacterId;

            public sealed class EntityIdIndex : BTreeIndexBase<uint>
            {
                protected override uint GetKey(Character row) => row.EntityId;

                public EntityIdIndex(CharacterHandle table) : base(table) { }
            }

            public readonly EntityIdIndex EntityId;

            public sealed class NameUniqueIndex : UniqueIndexBase<string>
            {
                protected override string GetKey(Character row) => row.Name;

                public NameUniqueIndex(CharacterHandle table) : base(table) { }
            }

            public readonly NameUniqueIndex Name;

            internal CharacterHandle(DbConnection conn) : base(conn)
            {
                AccountId = new(this);
                CharacterId = new(this);
                EntityId = new(this);
                Name = new(this);
            }

            protected override object GetPrimaryKey(Character row) => row.CharacterId;
        }

        public readonly CharacterHandle Character;
    }
}
