﻿using Realms;

namespace Hangfire.Realm.RealmObjects
{
    public class KeyValueDto : RealmObject
    {
		public string Key { get; set; }
		public string Value { get; set; }

	    public KeyValueDto()
	    {
		    
	    }

	    public KeyValueDto(string key, string value)
	    {
		    Key = key;
		    Value = value;
	    }
    }
}