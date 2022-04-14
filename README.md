# HarvestForecast.Client
![Nuget](https://img.shields.io/nuget/v/HarvestForecast.Client)

API client package for Harvest Forecast

Inspired by the unofficial [Go package](https://github.com/joefitzgerald/forecast) for [Harvest Forecast](https://www.getharvest.com/forecast) which I had a quick play with [here](https://github.com/G18SSY/forecast-test). I created this .NET client package as a direct(ish) port of that so it could be used in .NET based projects.

## Installation

This package will be available to install via NuGet!
```shell
dotnet add package HarvestForecast.Client
```

## Usage
To get started you will need an access token and account ID. These can be created/found [here](https://id.getharvest.com/oauth2/access_tokens/new). Make sure you enter your Forecast ID and not your Harvest one (they're different).

Once you have these details, you can create a `ForecastClient`:
```c#
var options = new ForecastOptions( id, token );
var client = new ForecastClient( new HttpClient(), options );
```

Now that you have a client, you can make requests using the strongly typed API. I would suggest starting with `WhoAmIAsync()` in a lot of cases to get the current user ID:
```c#
var user = await client.WhoAmIAsync();
Console.WriteLine($"Your user ID is '{user.Id}'");
```

For detailed usage I would suggest viewing the [example project](./example/Forecast.Viewer).

Since the Forecast API we're accessing is undocumented, it is possible to find new features by experimentation. For instance, many of the resources can be filtered in the query string like milestones which accept a `project_id` filter via the `MilestoneFilter` object. If you find any additional features please contribute them back!

## Build
Requirements:
- Your favourite IDE or text editor
- .NET 6 SDK (older version may work but I haven't tried them)

This project is built using [NUKE](https://www.nuke.build/index.html) and GitHub actions. You can run the build locally by installing Nuke and running it's default command:
```shell
dotnet tool install Nuke.GlobalTool --global
nuke
```

This will build the package and place it into the artifacts directory of the solution.

Versioning is done by [GitVersion](). If you find you need the tool installed then you can install it with:
```shell
dotnet tool install --global GitVersion.Tool 
```

## Issues & Contributing

I don't expect this library to need a huge amount of maintenance, if you have an issue to please consider contributing a fix too!

Please target the `main` branch with an PRs.
