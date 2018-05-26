﻿using System;
using Hangfire.Storage;
using Realms;

namespace Hangfire.Realm
{
    public class RealmJobStorage : JobStorage
    {
	    public const int SchemaVersion = 1;

	    private readonly RealmJobStorageOptions _options;
	    private readonly Realms.Realm _realm;
	    
	    public RealmJobStorage(RealmJobStorageOptions options)
	    {
		    _options = options;
			_options.RealmConfiguration= new RealmConfiguration(options.DatabasePath)
			{
				SchemaVersion = SchemaVersion
			};
		    _realm = Realms.Realm.GetInstance(_options.RealmConfiguration);
	    }
	    public override IMonitoringApi GetMonitoringApi()
	    {
		    return new RealmMonitoringApi(_realm);
	    }

	    public override IStorageConnection GetConnection()
	    {
		    return new RealmStorageConnection();
	    }
    }
}