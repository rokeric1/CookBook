using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

public class AuthenticationService
{
    private Dictionary<string, string> users = new Dictionary<string, string>();

    public string ReadPassword()
    {
        string password = string.Empty;
        ConsoleKey key;

        do
        {
            var keyInfo = Console.ReadKey(intercept: true);
            key = keyInfo.Key;

            if (key == ConsoleKey.Backspace && password.Length > 0)
            {
                Console.Write("\b \b");
                password = password[0..^1];
            }
            else if (!char.IsControl(keyInfo.KeyChar))
            {
                Console.Write("*");
                password += keyInfo.KeyChar;
            }
        } while (key != ConsoleKey.Enter);

        Console.WriteLine();
        return password;
    }

    public string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }
    }

    public bool RegistrujKorisnika(string username, string password)
    {
        if (users.ContainsKey(username))
            return false;

        string hashedPassword = HashPassword(password);
        users[username] = hashedPassword;
        return true;
    }

    public bool AutentifikujKorisnika(string username, string password)
    {
        if (!users.ContainsKey(username))
            return false;

        string hashedPassword = HashPassword(password);
        return users[username] == hashedPassword;
    }
}
