using System;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[ GitHubActions( "RunTests",
                 GitHubActionsImage.UbuntuLatest,
                 EnableGitHubToken = true,
                 AutoGenerate = true,
                 OnPullRequestBranches = new[] {"main"},
                 InvokedTargets = new[] {nameof( Test )} ) ]
[ GitHubActions( "Push",
                 GitHubActionsImage.UbuntuLatest,
                 EnableGitHubToken = true,
                 AutoGenerate = true,
                 OnPushBranches = new[] {"main"},
                 ImportSecrets = new[] {"NUGET_API_KEY"},
                 InvokedTargets = new[] {nameof( Push )} ) ]
[ CheckBuildProjectConfigurations ]
[ ShutdownDotNetAfterServerBuild ]
class Build : NukeBuild
{
    [ Parameter( "Configuration to build - Default is 'Debug' (local) or 'Release' (server)" ) ] readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [GitVersion(Framework = "netcoreapp3.1")] readonly GitVersion GitVersion;

    [ Solution ] readonly Solution Solution;

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath TestsDirectory => RootDirectory / "test";
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

    Target Clean => _ => _
                        .Before( Restore )
                        .Executes( () =>
                                   {
                                       SourceDirectory.GlobDirectories( "**/bin", "**/obj" ).ForEach( DeleteDirectory );
                                       TestsDirectory.GlobDirectories( "**/bin", "**/obj" ).ForEach( DeleteDirectory );
                                       EnsureCleanDirectory( ArtifactsDirectory );
                                   } );

    Target Restore => _ => _
                         .Executes( () =>
                                    {
                                        DotNetRestore( s => s
                                                          .SetProjectFile( Solution ) );
                                    } );

    Target Compile => _ => _
                          .DependsOn( Restore )
                          .Executes( () =>
                                     {
                                         DotNetBuild( s => s
                                                          .SetProjectFile( Solution )
                                                          .SetConfiguration( Configuration )
                                                          .SetAssemblyVersion( GitVersion.AssemblySemVer )
                                                          .SetFileVersion( GitVersion.AssemblySemFileVer )
                                                          .SetInformationalVersion( GitVersion.InformationalVersion )
                                                          .EnableNoRestore() );
                                     } );

    Target Pack => _ => _
                       .DependsOn( Compile )
                       .Executes( () =>
                                  {
                                      DotNetPack( s => s
                                                      .SetProject( Solution )
                                                      .SetConfiguration( Configuration )
                                                      .SetAssemblyVersion( GitVersion.AssemblySemVer )
                                                      .SetFileVersion( GitVersion.AssemblySemFileVer )
                                                      .SetInformationalVersion( GitVersion.InformationalVersion )
                                                      .SetOutputDirectory( ArtifactsDirectory )
                                                      .EnableNoBuild() );
                                  } );

    Target Test => _ => _
                       .DependsOn( Compile )
                       .Executes( () =>
                                  {
                                      DotNetTest( s => s
                                                      .SetProjectFile( Solution )
                                                      .SetConfiguration( Configuration )
                                                      .EnableNoBuild() );
                                  } );

    Target Push => _ => _
                       .DependsOn( Pack, Test )
                       .OnlyWhenStatic( () => !IsLocalBuild )
                       .Executes( () =>
                                  {
                                      var files = ArtifactsDirectory.GlobFiles( "*.nupkg" );
                                      
                                      DotNetNuGetPush( s => s
                                                           .SetSource( @"https://api.nuget.org/v3/index.json" )
                                                           .SetApiKey( Environment.GetEnvironmentVariable( "NUGET_API_KEY" ) )
                                                           .CombineWith( files, ( s, f ) => s.SetTargetPath( f ) ) );
                                  } );

    /// Support plugins are available for:
    /// - JetBrains ReSharper        https://nuke.build/resharper
    /// - JetBrains Rider            https://nuke.build/rider
    /// - Microsoft VisualStudio     https://nuke.build/visualstudio
    /// - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<Build>( x => x.Pack, x => x.Test, x => x.Clean );
}
