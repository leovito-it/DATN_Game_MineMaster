using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildScript
{
    private const string KeystorePassword = "mirai1102";
    private const string KeyAliasPassword = "mirai1102";

    [MenuItem("Build/Build APK")]
    public static void BuildAPK()
    {
        // Force resolve Android dependencies
        TryForceResolveAndroidDependencies();
        Build(BuildTarget.Android, "apk");
    }

    [MenuItem("Build/Build AAB")]
    public static void BuildAAB()
    {
        // Set buildAppBundle to true to build an Android App Bundle
        EditorUserBuildSettings.buildAppBundle = true;

        Build(BuildTarget.Android, "aab");
    }
    private static void TryForceResolveAndroidDependencies()
    {
        try
        {
            // The full name of the AndroidDependenciesResolver class
            string className = "UnityEditor.Android.AndroidDependenciesResolver";

            // Get the Type of the class
            Type resolverType = Type.GetType(className);

            if (resolverType != null)
            {
                // Get the Resolve method
                MethodInfo resolveMethod = resolverType.GetMethod("Resolve", BindingFlags.Public | BindingFlags.Static);

                if (resolveMethod != null)
                {
                    // Invoke the Resolve method
                    resolveMethod.Invoke(null, null);
                    Debug.Log("Android dependencies resolved.");
                }
                else
                {
                    Debug.LogError("Resolve method not found in AndroidDependenciesResolver.");
                }
            }
            else
            {
                Debug.LogError("AndroidDependenciesResolver class not found.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error while trying to resolve Android dependencies: " + ex);
        }
    }
    private static void Build(BuildTarget target, string extension)
    {
        // Get the scenes from the build settings
        EditorBuildSettingsScene[] buildScenes = EditorBuildSettings.scenes;

        // Create a string array to hold scene paths
        string[] scenes = new string[buildScenes.Length];

        for (int i = 0; i < buildScenes.Length; i++)
        {
            scenes[i] = buildScenes[i].path;
        }

        string productName = PlayerSettings.productName;
        string version = PlayerSettings.bundleVersion;

        // Construct the output path
        string projectDirectory = Directory.GetCurrentDirectory();
        string outputFolder = "Build"; // You can adjust this folder name if needed
        productName = productName.Replace(":", "").Replace(" ", "");

        // Update this line to correctly concatenate paths using Path.Combine
        string filename2 = productName + "_" + version + "." + extension;
        string outputPath = Path.Combine(projectDirectory, outputFolder, filename2);

        for (int i = 1; i <= 10; i++)
        {
            string filename = $"{productName}_{DateTime.Now:dd}{DateTime.Now:MM}_ver{i}.{extension}";
            outputPath = Path.Combine(projectDirectory, outputFolder, filename);
            if (File.Exists(outputPath))
                continue;
            else
                break;
        }

        PlayerSettings.keyaliasPass = KeyAliasPassword;
        PlayerSettings.keystorePass = KeystorePassword;

        var buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = outputPath,
            target = target,
            options = BuildOptions.None
        };

        var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        var summary = report.summary;

        switch (summary.result)
        {
            case BuildResult.Succeeded:
                Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
                break;
            case BuildResult.Failed:
                Debug.Log("Build failed");
                break;
            case BuildResult.Unknown:
            case BuildResult.Cancelled:
            default:
                Debug.Log("Build status: " + summary.result);
                break;
        }
    }
}
