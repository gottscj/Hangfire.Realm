﻿using System;
using System.Collections.Generic;
using Realms;

namespace Hangfire.Realm.RealmObjects
{
	internal class JobDto : RealmObject
    {
		[PrimaryKey]
	    public string Id { get; set; }

	    public DateTimeOffset Created { get; set; }

	    public string StateName { get; set; }

	    public string InvocationData { get; set; }

	    public string Arguments { get; set; }

	    public IList<KeyValueDto> Parameters { get; }

	    public IList<StateDto> StateHistory { get; }

	    public DateTimeOffset? ExpireAt { get; set; }

	    public override string ToString()
	    {
		    return Id + " " + StateName;
	    }
    }
}