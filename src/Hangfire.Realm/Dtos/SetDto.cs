﻿using System;
using System.Collections.Generic;
using Realms;

namespace Hangfire.Realm.Dtos
{
    public class SetDto : RealmObject
    {
        [PrimaryKey]
        public string Key { get; set; }

        public DateTimeOffset Created { get; set; } = DateTimeOffset.Now;
        
        public DateTimeOffset? ExpireIn { get; set; }

        public IList<ScoreDto> Scores { get; set; }
    }
}