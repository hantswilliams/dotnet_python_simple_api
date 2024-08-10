using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel to listen on 0.0.0.0:5005
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5005);
});


var app = builder.Build();

// Simple endpoint that returns "Hello World!"
app.MapGet("/", () => "Hello World!");

// New endpoint that takes a name parameter
app.MapGet("/hello", 
    (string name) => $"Hello {name}!"
);

// New endpoint that calls Python using a subprocess
app.MapGet("/python", () =>
{
    string pythonScript = "./hello.py";  // Path to your Python script
    string pythonResult = string.Empty;
    string errorResult = string.Empty;

    try
    {
        var processStartInfo = new ProcessStartInfo("python3.11")
        {
            Arguments = pythonScript,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (var process = new Process { StartInfo = processStartInfo })
        {
            process.Start();
            pythonResult = process.StandardOutput.ReadToEnd();
            errorResult = process.StandardError.ReadToEnd();
            process.WaitForExit();
        }
    }
    catch (Exception ex)
    {
        return $"Error: {ex.Message}";
    }

    if (!string.IsNullOrEmpty(errorResult))
    {
        return $"Python Error: {errorResult}";
    }

    return pythonResult;
});

// Endpoint that calls Python using a subprocess and passes name and age as arguments
// Endpoint that calls Python using a subprocess and passes name and age as arguments
app.MapGet("/python2", (string? name, int? age) =>
{
    // Check if name or age is missing
    if (string.IsNullOrEmpty(name) || !age.HasValue)
    {
        return "Error: Please provide both name and age as query parameters.";
    }

    // Print to console the name and age
    Console.WriteLine($"Name: {name}");
    Console.WriteLine($"Age: {age}");

    string pythonScript = "./hello_name_age.py";  // Path to your Python script
    string pythonResult = string.Empty;
    string errorResult = string.Empty;

    try
    {
        var processStartInfo = new ProcessStartInfo("python3.11")
        {
            Arguments = $"{pythonScript} {name} {age}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (var process = new Process { StartInfo = processStartInfo })
        {
            process.Start();
            pythonResult = process.StandardOutput.ReadToEnd();
            errorResult = process.StandardError.ReadToEnd();
            process.WaitForExit();
        }
    }
    catch (Exception ex)
    {
        return $"Error: {ex.Message}";
    }

    if (!string.IsNullOrEmpty(errorResult))
    {
        return $"Python Error: {errorResult}";
    }

    return pythonResult;
});




app.Run();
