namespace MusicHub;

using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using Data;
using Initializer;
using MusicHub.Data.Models;

public class StartUp
{
    public static void Main()
    {
        MusicHubDbContext context =
            new MusicHubDbContext();

        DbInitializer.ResetDatabase(context);

    }

    public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
    {
        throw new NotImplementedException();
    }

    public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
    {
        throw new NotImplementedException();
    }
}
