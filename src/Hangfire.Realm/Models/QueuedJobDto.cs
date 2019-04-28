﻿using System;
using Realms;

namespace Hangfire.Realm.Models
{
	internal class QueuedJobDto : RealmObject
    {
		[PrimaryKey]
	    public string Id { get; set; }

	    public DateTimeOffset Created { get; set; } = DateTimeOffset.Now;

	    public string JobId { get; set; }

	    public string Queue { get; set; }

	    public DateTimeOffset? FetchedAt { get; set; }
    }
}