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

        Console.WriteLine(ExportAlbumsInfo(context, 9));
    }

    public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
    {
       StringBuilder output = new StringBuilder();
        
        var albums = context.Albums
                     .Where(a => a.ProducerId == producerId)
                     .ToArray()
                     .Select(a => new 
                     {
                       a.Name,
                       ReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                       ProducerName = a.Producer.Name,
                       AlbumSongs = a.Songs.Select(s => new 
                                           {
                                             SongName = s.Name,
                                             s.Price,
                                             SongWriterNames = s.Writer.Name
                                           })
                                          .OrderByDescending(s => s.SongName)
                                          .ThenBy(s => s.SongWriterNames)
                                          .ToArray(),
                       TotalAlbumPrice = a.Price
                     })
                     .OrderByDescending(a => a.TotalAlbumPrice)
                     .ToArray();

        foreach (var album in albums)
        {
            output.AppendLine($"-AlbumName: {album.Name}")
                  .AppendLine($"-ReleaseDate: {album.ReleaseDate}")
                  .AppendLine($"-ProducerName: {album.ProducerName}")
                  .AppendLine($"-Songs:");

            int songNum = 1;

            foreach (var song in album.AlbumSongs)
            {
                output.AppendLine($"---#{songNum}")
                      .AppendLine($"---SongName: {song.SongName}")
                      .AppendLine($"---Price: {song.Price:f2}")
                      .AppendLine($"---Writer: {song.SongWriterNames}");
                songNum++;
            }

            output.AppendLine($"-AlbumPrice: {album.TotalAlbumPrice:f2}");
        }

        return output.ToString().TrimEnd();
    }

    public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
    {
        throw new NotImplementedException();
    }
}
