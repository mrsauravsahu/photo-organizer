using System;
using System.IO;
using System.Linq;

var inputPath = args[0];
var outputPath = args[1];

Console.WriteLine($"files: ");

var filePaths = Directory.EnumerateFiles(inputPath);

var fileData = filePaths.Select(p => new FileInfo(p));

var filesGroupedByDate = fileData
    .Select(p =>
    new
    {
      FileName = p.Name,
      Path = p.FullName,
      CreatedAt = p.CreationTimeUtc
    })
    .GroupBy(p => p.CreatedAt);


Console.WriteLine("Starting grouping...");
filesGroupedByDate
    .ToList()
    .ForEach(p =>
    {
      var directoryPath = Path.Join(outputPath, String.Format("{0:yyyy-MM-dd}", p.Key));
      Console.WriteLine($"Creating directory '{directoryPath}'");
      Directory.CreateDirectory(directoryPath);

      p.ToList().ForEach(file =>
      {
        var filePath = Path.Join(directoryPath, file.FileName);
        Console.WriteLine($"Copying file '{file.Path}' to '{filePath}'");
        File.Move(file.Path, filePath);
      });
    });
