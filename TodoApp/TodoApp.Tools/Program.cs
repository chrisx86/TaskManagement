// We need to use the PasswordHasher from our Infrastructure project.
using TodoApp.Infrastructure.Security;

// This is a temporary tool to generate a static password hash.
// The password to hash is "admin".
string passwordToHash = "admin";

// Call the hasher method.
string hashedPassword = PasswordHasher.HashPassword(passwordToHash);

// Print the result to the console.
Console.WriteLine("Password Hash Generator");
Console.WriteLine("-----------------------");
Console.WriteLine($"Plain Password: {passwordToHash}");
Console.WriteLine($"Hashed Value  : {hashedPassword}");
Console.WriteLine();
Console.WriteLine("Please copy the hashed value above and paste it into AppDbContext.cs");

// Keep the console window open until a key is pressed.
Console.ReadKey();