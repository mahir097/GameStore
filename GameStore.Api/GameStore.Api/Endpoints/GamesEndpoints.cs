using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
    {
       const string GetGameEndpointName = "GetGame";


        // WebApplication sınıfını genişleten bir genişletme yöntemi tanımlayın
        public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
        {

            var group = app.MapGroup("games").WithParameterValidation();
            


            // GET /games
            group.MapGet("/", (GameStoreContext dbContext) => 
                dbContext.Games
                          .Include(game => game.Genre)    
                          .Select(game => game.ToGameSummaryDto())
                          .AsNoTracking());

            // GET /games/{id}
            group.MapGet("/{id}", (int id, GameStoreContext dbContext) => 
            {
              Game? game = dbContext.Games.Find(id);

              return game is null ? 
                  Results.NotFound() : Results.Ok(game.ToGameDetailsDto());
            })
            .WithName(GetGameEndpointName);


            // POST /games
            group.MapPost("/",(CreateGameDto newGame, GameStoreContext dbContext) => 
            {

              Game game = newGame.ToEntity();

               dbContext.Games.Add(game);
               dbContext.SaveChanges();

    
               return Results.CreatedAtRoute(
                GetGameEndpointName, 
                new { id = game.Id }, 
                game.ToGameDetailsDto());
            })
            .WithParameterValidation();


            // PUT /games/{id}
            group.MapPut("/{id}", (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) => 
            {
              var existingGame = dbContext.Games.Find(id);

              if (existingGame is null)
              {
                  return Results.NotFound(); // Return a 404 Not Found response if the game does not exist
             }

                dbContext.Entry(existingGame)
                        .CurrentValues
                        .SetValues(updatedGame.ToEntity(id));
                
                dbContext.SaveChanges();

              return Results.NoContent();
            })
            .WithParameterValidation();



            // DELETE /games/{id}
               group.MapDelete("/{id}", (int id, GameStoreContext dbContext) => 
            {
                dbContext.Games
                .Where(game => game.Id == id)
                .ExecuteDelete();

               return Results.NoContent();
            });

            // WebApplication nesnesini döndürün
            return group;
        }
    }
