using GameStore.Api.Dtos;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
    {
       const string GetGameEndpointName = "GetGame";


 private static readonly List<GameDto> games = [
    new (1, "The Last of Us Part II", "Action", 59.99m, new DateOnly(2020, 6, 19)),
    new (2, "Ghost of Tsushima", "Action", 59.99m, new DateOnly(2020, 7, 17)),
    new (3, "Cyberpunk 2077", "RPG", 59.99m, new DateOnly(2020, 12, 10)),
];

        // WebApplication sınıfını genişleten bir genişletme yöntemi tanımlayın
        public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
        {

            var group = app.MapGroup("games").WithParameterValidation();
            


            // GET /games
            group.MapGet("/", () => games);

            // GET /games/{id}
            group.MapGet("/{id}", (int id) => 
            {
              games.Find(games => games.Id ==id);
            })
            .WithName(GetGameEndpointName);


            // POST /games
            group.MapPost("/",(CreateGameDto newGame) => 
            {

                // if(string.IsNullOrEmpty(newGame.Name))
                // {
                //     return Results.BadRequest("Name is required");
                // }

              GameDto game = new(
              games.Count + 1,
              newGame.Name,
              newGame.Genre,
              newGame.Price,
               newGame.ReleaseDate
               );

               games.Add(game);
    
               return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
            })
            .WithParameterValidation();


            // PUT /games/{id}
            group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) => 
            {
              var index = games.FindIndex(games => games.Id == id);

              if (index == -1)
              {
                  return Results.NotFound(); // Return a 404 Not Found response if the game does not exist
             }

             games[index] = new GameDto(
                    id,
                    updatedGame.Name,
                   updatedGame.Genre,
                   updatedGame.Price,
                  updatedGame.ReleaseDate
              );

              return Results.NoContent();
            })
            .WithParameterValidation();



            // DELETE /games/{id}
               group.MapDelete("/{id}", (int id) => 
            {
                games.RemoveAll(games => games.Id == id);

               return Results.NoContent();
            });

            // WebApplication nesnesini döndürün
            return group;
        }
    }
