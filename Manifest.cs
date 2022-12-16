using OrchardCore.Modules.Manifest;

[assembly: Module(
    Id = "EvanQ.OrchardCore.Media.Download",
    Name = "Media Download",
    Version = "1.0.2",
    Description = "Allow users to force download a media file",
    Category = "Content Management",
    Dependencies = new[]
    {
        "OrchardCore.Contents"
    },
    Priority = "-2147483648"
)]