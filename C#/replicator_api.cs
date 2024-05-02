using System;
using System.IO;
using System.Diagnostics;

Console.WriteLine("Replycator API 0.01");

void DeleteFiles()
{
    string directoryPath = "files/";
    foreach (string filename in Directory.GetFiles(directoryPath))
    {
        File.Delete(filename);
    }
}

bool Send(string mode, string fileType = null)
{
    if (mode == "social")
    {
        string command;
        if (fileType == "all")
        {
            command = "sh replycator.sh --social=twitter --text=true --files=true";
        }
        else
        {
            command = "sh replycator.sh --social=twitter --files=true";
        }

        int checkSend = RunCommand(command);
        if (checkSend == 0)
        {
            DeleteFiles();
            return true;
        }
        else if (checkSend == 1)
        {
            return false;
        }
    }
    else if (mode == "youtube")
    {
        int checkSend = RunCommand("sh replycator.sh --social=youtube");
        if (checkSend == 0)
        {
            DeleteFiles();
            return true;
        }
        else if (checkSend == 1)
        {
            return false;
        }
    }

    return false;
}

int RunCommand(string command)
{
    ProcessStartInfo startInfo = new ProcessStartInfo
    {
        FileName = "/bin/bash",
        Arguments = "-c \"" + command + "\"",
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
        CreateNoWindow = true
    };

    Process process = Process.Start(startInfo);
    process.WaitForExit();
    return process.ExitCode;
}

