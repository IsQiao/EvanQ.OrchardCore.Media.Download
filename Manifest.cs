using OrchardCore.Modules.Manifest;

[assembly: Module(
    Id = "EvanQ.OrchardCore.Media.Download",
    Name = "Media Download",
    Description = "Allow users to force download a media file",
    Category = "Content Management",
    Dependencies = new[]
    {
        "OrchardCore.ContentTypes",
        // "OrchardCore.Media", // todo: fix it
    }
)]