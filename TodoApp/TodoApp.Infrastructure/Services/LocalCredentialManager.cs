#nullable enable
using System.Text;
using System.Security.Cryptography;

namespace TodoApp.Infrastructure.Services;

/// <summary>
/// Manages the secure storage and retrieval of user credentials on the local machine.
/// Uses DPAPI (ProtectedData) for encryption tied to the current Windows user account.
/// </summary>
public class LocalCredentialManager
{
    private readonly string _filePath;

    public LocalCredentialManager()
    {
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var appFolder = Path.Combine(appDataPath, "TaskManagmentApp");
        Directory.CreateDirectory(appFolder);
        _filePath = Path.Combine(appFolder, "TM_User.dat");
    }

    /// <summary>
    /// Saves the username and login token to an encrypted local file.
    /// </summary>
    public void SaveCredentials(string username, string token)
    {
        try
        {
            var plainText = $"{username}\n{token}";
            var dataToProtect = Encoding.UTF8.GetBytes(plainText);

            // Encrypt the data using the current Windows user's credentials.
            var protectedData = ProtectedData.Protect(
                dataToProtect,
                null, // Optional entropy
                DataProtectionScope.CurrentUser);

            File.WriteAllBytes(_filePath, protectedData);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[ERROR] Failed to save credentials: {ex.Message}");
        }
    }

    /// <summary>
    /// Tries to load and decrypt credentials from the local file.
    /// </summary>
    /// <returns>A tuple containing the username and token, or null if loading fails.</returns>
    public (string Username, string Token)? TryLoadCredentials()
    {
        if (!File.Exists(_filePath)) return null;

        try
        {
            var protectedData = File.ReadAllBytes(_filePath);

            // Decrypt the data. This will only succeed if run by the same Windows user who saved it.
            var plainData = ProtectedData.Unprotect(
                protectedData,
                null,
                DataProtectionScope.CurrentUser);

            var plainText = Encoding.UTF8.GetString(plainData);
            var parts = plainText.Split('\n');

            if (parts.Length == 2)
                return (Username: parts[0], Token: parts[1]);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[INFO] Failed to load credentials, possibly tampered or old file: {ex.Message}");
            ClearCredentials();
        }

        return null;
    }

    /// <summary>
    /// Deletes the local credential file.
    /// </summary>
    public void ClearCredentials()
    {
        try
        {
            if (File.Exists(_filePath))
                File.Delete(_filePath);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[ERROR] Failed to clear credentials: {ex.Message}");
        }
    }
}