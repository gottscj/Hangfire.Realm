# Hangfire.Realm

This [Hangfire](http://hangfire.io) extension adds support for using the lightweight embeddable [Realm](https://realm.io) object database.

_**Warning:** This project is under active development and has not been fully tested in production. Please use responsibly. Any developer input is appreciated._

## Installation

This project is not yet released as a NuGet package. Install it by checking it out with Git or downloading it directly as a zip and adding a reference to the Hangfire.Realm project to your code.

## Usage

### .NET Core

Please see the [Hangfire.Realm.Sample.NET.Core](https://github.com/gottscj/Hangfire.Realm/tree/master/src/Hangfire.Realm.Sample.NET.Core) project for a working example.

```csharp
public static void Main()
{
    //The path to the Realm DB file.
    string dbPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Hangfire.Realm.Sample.NetCore.realm");

    //A standard Realm configuration.
    RealmConfiguration realmConfiguration = new RealmConfiguration(dbPath)
    {
        ShouldCompactOnLaunch = (totalBytes, usedBytes) =>
        {
            // Compact if the file is over 100MB in size and less than 50% 'used'
            var oneHundredMB = (ulong)(100 * 1024 * 1024);
            return totalBytes > oneHundredMB && (double)usedBytes / totalBytes < 0.5;
        }
    };

    //Hangfire.Realm storage options.
    RealmJobStorageOptions storageOptions = new RealmJobStorageOptions
    {
        RealmConfiguration = realmConfiguration, //Required.
        QueuePollInterval = TimeSpan.FromSeconds(1), //Optional. Defaults to TimeSpan.FromSeconds(15)
        SlidingInvisibilityTimeout = TimeSpan.FromSeconds(10), //Optional. Defaults to TimeSpan.FromMinutes(10)
        JobExpirationCheckInterval = TimeSpan.FromMinutes(1) //Optional. Defaults to TimeSpan.FromMinutes(30)
    };

    //Standard Hangfire server options.
    BackgroundJobServerOptions serverOptions = new BackgroundJobServerOptions()
    {
        WorkerCount = 10,
        Queues = new[] { "default", "critical" },
        ServerTimeout = TimeSpan.FromMinutes(10),
        HeartbeatInterval = TimeSpan.FromSeconds(30),
        ServerCheckInterval = TimeSpan.FromSeconds(10),
        SchedulePollingInterval = TimeSpan.FromSeconds(10)
    };

    //Hangfire global configuration
    GlobalConfiguration.Configuration
    .UseLogProvider(new ColouredConsoleLogProvider(Logging.LogLevel.Debug))
    .UseRealmJobStorage(storageOptions);


    using (new BackgroundJobServer(serverOptions))
    {

        //Queue a bunch of fire-and-forget jobs
        for (var i = 0; i < JobCount; i++)
        {
            var jobNumber = i + 1;
            BackgroundJob.Enqueue(() =>
            Console.WriteLine($"Fire-and-forget job {jobNumber}"));
        }

        //A scheduled job that will run 1.5 minutes after being placed in queue
        BackgroundJob.Schedule(() =>
        Console.WriteLine("A Scheduled job."),
        TimeSpan.FromMinutes(1.5));

        //A fire-and-forget continuation job that has three steps
        BackgroundJob.ContinueJobWith(
            BackgroundJob.ContinueJobWith(
            BackgroundJob.Enqueue(
                    () => Console.WriteLine($"Knock knock..")),
                    () => Console.WriteLine("Who's there?")),
                        () => Console.WriteLine("A continuation job!"));

        //A scheduled continuation job that has three steps
        BackgroundJob.ContinueJobWith(
            BackgroundJob.ContinueJobWith(
            BackgroundJob.Schedule(
                    () => Console.WriteLine($"Knock knock.."), TimeSpan.FromMinutes(2)),
                    () => Console.WriteLine("Who's there?")),
                        () => Console.WriteLine("A scheduled continuation job!"));

        //A Cron based recurring job
        RecurringJob.AddOrUpdate("recurring-job-1", () =>
        Console.WriteLine("Recurring job 1."),
        Cron.Minutely);

        //Another recurring job
        RecurringJob.AddOrUpdate("recurring-job-2", () =>
        Console.WriteLine("Recurring job 2."),
        Cron.Minutely);

        //An update to the first recurring job
        RecurringJob.AddOrUpdate("recurring-job-1", () =>
        Console.WriteLine("Recurring job 1 (edited)."),
        Cron.Minutely);

        Console.Read();
    }
}
```

### ASP.NET Core

Please see the [Hangfire.Realm.Sample.ASP.NET.Core](https://github.com/gottscj/Hangfire.Realm/tree/master/src/Hangfire.Realm.Sample.ASP.NET.Core) project for a working example.

```csharp
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        RealmJobStorageOptions storageOptions = new RealmJobStorageOptions
        {
            RealmConfiguration = new RealmConfiguration(
              Path.Combine(
              Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
              "Some.realm"),
            QueuePollInterval = TimeSpan.FromSeconds(1),
            SlidingInvisibilityTimeout = TimeSpan.FromSeconds(10)
        };

        services.AddHangfire(config =>
        {
            config
            .UseRealmJobStorage(storageOptions)
            .UseLogProvider(new ColouredConsoleLogProvider());
        });
        services.AddHangfireServer(options =>
        {
            options.WorkerCount = 10;
            options.Queues = new[] { "default" };
            options.ServerTimeout = TimeSpan.FromMinutes(10);
            options.HeartbeatInterval = TimeSpan.FromSeconds(30);
            options.ServerCheckInterval = TimeSpan.FromSeconds(10);
            options.SchedulePollingInterval = TimeSpan.FromSeconds(10);
        });

        services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHsts();
        }
        app.UseHangfireDashboard();
        app.UseStaticFiles();
        app.UseHttpsRedirection();
        app.UseMvc();
    }
}
```

The Hangfire web dashboard will be available at /hangfire.

## Issues

If you have any questions or issues related to Hangfire.Realm or want to discuss new features please create a new or comment on an existing [issue](https://github.com/gottscj/Hangfire.Realm/issues).
