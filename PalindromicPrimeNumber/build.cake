#addin "nuget:https://api.nuget.org/v3/index.json?package=Cake.Sonar&version=1.1.25"
#addin "nuget:https://api.nuget.org/v3/index.json?package=Cake.Coverlet&version=2.5.4"

#tool "dotnet:https://api.nuget.org/v3/index.json?package=DotNet-SonarScanner&version=5.1.0"
#tool "dotnet:https://api.nuget.org/v3/index.json?package=GitVersion.Tool&version=5.6.6"

var target = Argument("target", "Build");
var configuration = Argument("configuration", "Release");
var forceSonar = Argument<bool>("sonar", false);

DotNetCoreMSBuildSettings msBuildSettings = null;
GitVersion gitVersions = null;

Setup<BuildData>(context => {
	var sonarProjectName = "PalindromicPrimeNumber";

	gitVersions = context.GitVersion();
	msBuildSettings = GetMsBuildSettings();

	if(gitVersions.BranchName != "master")
		sonarProjectName += $":{gitVersions.BranchName}";

	return new BuildData {
		IsReleasableBranch = gitVersions.BranchName == "master"
								|| gitVersions.BranchName == "develop"
								|| gitVersions.BranchName.StartsWith("release"),
		SonarProjectName = sonarProjectName,
		SonarUrl = EnvironmentVariable("SONAR_URL"),
		SonarUser = EnvironmentVariable("SONAR_USER"),
		SonarPassword = EnvironmentVariable("SONAR_PASSWORD")
	};
});

Task("Clean-Artifacts")
	.Does(() => {
		if(DirectoryExists(Constants.ArtifactsDirectoryPath))
			DeleteDirectory(
				Constants.ArtifactsDirectoryPath,
				new DeleteDirectorySettings { Recursive = true});

		EnsureDirectoryExists(Constants.ArtifactsDirectoryPath);
	});

Task("Sonar-Begin")
	.WithCriteria<BuildData>((context, data) => data.IsReleasableBranch || forceSonar)
	.Does<BuildData>(data => {
		SonarBegin(new SonarBeginSettings {
			Name = data.SonarProjectName,
			Key = data.SonarProjectName,
			Version = $"{gitVersions.AssemblySemVer}{gitVersions.PreReleaseTagWithDash}",
			Url = data.SonarUrl,
			Login = data.SonarUser,
			Password = data.SonarPassword,
			VsTestReportsPath = $"{Constants.VsTestReportsDirectoryPath}/*.{Constants.VsTestLogger}",
			OpenCoverReportsPath = $"{Constants.CoverageReportsDirectoryPath}/*.xml"			
		});
	});

Task("Sonar-End")
	.WithCriteria<BuildData>((context, data) => data.IsReleasableBranch || forceSonar)
	.Does<BuildData>(data => {
		SonarEnd(new SonarEndSettings { Login = data.SonarUser, Password = data.SonarPassword });
	});

Task("Restore")
	.Does(() => {
		DotNetCoreRestore(Constants.SolutionFilePath.FullPath, 
			new DotNetCoreRestoreSettings { ConfigFile = "Nuget.Config" });
	});

Task("Build")
	.IsDependentOn("Restore")
	.Does(() => {
		DotNetCoreBuild(
			Constants.SolutionFilePath.FullPath,
				new DotNetCoreBuildSettings() {
					Configuration = configuration,
					MSBuildSettings = msBuildSettings,
					NoRestore = true
				});
	});

Task("Test")
	.IsDependentOn("Clean-Artifacts")
	.IsDependentOn("Build")
	.Does(() => {
		var projects = GetFiles("./test/**/*Test.csproj");
		if(projects.Count == 0)
		{
			Warning("No test project found.");
			return;
		}
		
		foreach(var project in projects){
			var testSettings = new DotNetCoreTestSettings() {
				Configuration = configuration,
				Loggers = new [] {Constants.VsTestLogger},
				ResultsDirectory = Constants.VsTestReportsDirectoryPath,
				ArgumentCustomization = args => args.Append("--collect:\"XPlat Code Coverage\""),
				NoRestore = true,
				NoBuild = true
			};

			var coverletSettings = new CoverletSettings {
				CollectCoverage = true,
				CoverletOutputFormat = CoverletOutputFormat.opencover,
				CoverletOutputDirectory = Constants.CoverageReportsDirectoryPath,
				CoverletOutputName = $"Coverage{DateTime.UtcNow.Ticks}.xml"
			};

			DotNetCoreTest(
				project,
				testSettings,
				coverletSettings
			);
		}
	});

Task("Unit-Test")
	.IsDependentOn("Clean-Artifacts")
	.IsDependentOn("Build")
	.Does(() => {
		var projects = GetFiles("./test/**/*UnitTest.csproj");
		if(projects.Count == 0)
		{
			Warning("No test project found.");
			return;
		}
		
		foreach(var project in projects){
			var testSettings = new DotNetCoreTestSettings() {
				Configuration = configuration,
				Loggers = new [] {Constants.VsTestLogger},
				ResultsDirectory = Constants.VsTestReportsDirectoryPath,
				ArgumentCustomization = args => args.Append("--collect:\"XPlat Code Coverage\""),
				NoRestore = true,
				NoBuild = true
			};

			var coverletSettings = new CoverletSettings {
				CollectCoverage = true,
				CoverletOutputFormat = CoverletOutputFormat.opencover,
				CoverletOutputDirectory = Constants.CoverageReportsDirectoryPath,
				CoverletOutputName = $"Coverage{DateTime.UtcNow.Ticks}.xml"
			};

			DotNetCoreTest(
				project,
				testSettings,
				coverletSettings
			);
		}
	});

Task("Integration-Test")
	.IsDependentOn("Clean-Artifacts")
	.IsDependentOn("Build")
	.Does(() => {
		var projects = GetFiles("./test/**/*IntegrationTest.csproj");
		if(projects.Count == 0)
		{
			Warning("No test project found.");
			return;
		}
		
		foreach(var project in projects){
			var testSettings = new DotNetCoreTestSettings() {
				Configuration = configuration,
				Loggers = new [] {Constants.VsTestLogger},
				ResultsDirectory = Constants.VsTestReportsDirectoryPath,
				ArgumentCustomization = args => args.Append("--collect:\"XPlat Code Coverage\""),
				NoRestore = true,
				NoBuild = true
			};

			var coverletSettings = new CoverletSettings {
				CollectCoverage = true,
				CoverletOutputFormat = CoverletOutputFormat.opencover,
				CoverletOutputDirectory = Constants.CoverageReportsDirectoryPath,
				CoverletOutputName = $"Coverage{DateTime.UtcNow.Ticks}.xml"
			};

			DotNetCoreTest(
				project,
				testSettings,
				coverletSettings
			);
		}
	});

Task("Inspect-Test")
	.IsDependentOn("Sonar-Begin")
	.IsDependentOn("Test")
	.IsDependentOn("Sonar-End")
	.Does(() => {});

Task("Inspect-Unit-Test")
	.IsDependentOn("Sonar-Begin")
	.IsDependentOn("Unit-Test")
	.IsDependentOn("Sonar-End")
	.Does(() => {});

Task("Inspect-Integration-Test")
	.IsDependentOn("Sonar-Begin")
	.IsDependentOn("Integration-Test")
	.IsDependentOn("Sonar-End")
	.Does(() => {});

RunTarget(target);

// Helpers
public class BuildData
{
	public bool IsReleasableBranch { get; set; }

	public string SonarProjectName { get; set; }

	public string SonarUrl { get; set; }

	public string SonarUser { get; set; }

	public string SonarPassword { get; set; }
}


public static class Constants
{
	public static DirectoryPath ArtifactsDirectoryPath => "artifacts";

	public static DirectoryPath VsTestReportsDirectoryPath => $"{Constants.ArtifactsDirectoryPath}/VsTestReports";

	public static DirectoryPath CoverageReportsDirectoryPath => $"{Constants.ArtifactsDirectoryPath}/Coverage";

	public static FilePath SolutionFilePath => "PalindromicPrimeNumber.sln";

	public static string VsTestLogger = "trx";
}

private DotNetCoreMSBuildSettings GetMsBuildSettings()
{
	var settings = new DotNetCoreMSBuildSettings();

	settings.WithProperty("AssemblyVersion", gitVersions.AssemblySemVer);
	settings.WithProperty("VersionPrefix", gitVersions.AssemblySemVer);
	settings.WithProperty("FileVersion", gitVersions.AssemblySemVer);
	settings.WithProperty("InformationalVersion", gitVersions.AssemblySemVer + gitVersions.PreReleaseTagWithDash);
	settings.WithProperty("VersionSuffix", gitVersions.PreReleaseLabel + gitVersions.CommitsSinceVersionSourcePadded);

	return settings;
}