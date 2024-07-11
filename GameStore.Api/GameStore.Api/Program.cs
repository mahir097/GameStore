using GameStore.Api.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


List<GameDto> games = [
    new (1, "The Last of Us Part II", "Action", 59.99m, new DateOnly(2020, 6, 19)),
    new (2, "Ghost of Tsushima", "Action", 59.99m, new DateOnly(2020, 7, 17)),
    new (3, "Cyberpunk 2077", "RPG", 59.99m, new DateOnly(2020, 12, 10)),
];

app.MapGet("/", () => "Hello World!");

app.Run();
