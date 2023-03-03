namespace MusicHub;

using System;
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

        Console.WriteLine(ExportSongsAboveDuration(context, 4));
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
        StringBuilder output = new StringBuilder();

        var songs = context.Songs
                           .ToArray()
                           .Where(s => s.Duration.TotalSeconds > duration)
                           .Select(s => new
                           {
                               s.Name,
                               SongPerformers = s.SongPerformers
                                                     .Select(sp => $"{sp.Performer.FirstName} {sp.Performer.LastName}")
                                                     .OrderBy(p => p)
                                                     .ToArray(),
                               WriterName = s.Writer.Name,
                               AlbumProducer = s.Album.Producer.Name,
                               Duration = s.Duration.ToString("c"),

                           })
                           .OrderBy(s => s.Name)
                           .ThenBy(s => s.WriterName)
                           .ToArray();

        int songNum = 1;

        foreach (var song in songs)
        {
            output.AppendLine($"-Song #{songNum}")
                  .AppendLine($"---SongName: {song.Name}")
                  .AppendLine($"---Writer: {song.WriterName}");

            foreach (var performer in song.SongPerformers)
            {
                output.AppendLine($"---Performer: {performer}");
            }

            output.AppendLine($"---AlbumProducer: {song.AlbumProducer}")
                  .AppendLine($"---Duration: {song.Duration}");

            songNum++;
        }

        return output.ToString().TrimEnd();
    }
}
