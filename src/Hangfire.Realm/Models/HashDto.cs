using System;
using System.Collections.Generic;
using Realms;

namespace Hangfire.Realm.Models
{
    public class HashDto : RealmObject, IExpireAt
    {
        public HashDto()
        {
        }

        public HashDto(string key)
        {
            Key = key;
        }

        [PrimaryKey]
        public string Key { get; set; }
        public DateTimeOffset Created { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset? ExpireAt { get; set; }
        public IList<FieldDto> Fields { get; }
    }
}