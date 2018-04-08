using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System.Linq;
using MasterYodaRecipes.Models;

namespace MasterYodaRecipes.Dialogs
{
    /// <summary>
    /// Change Model ID and Subscription Key below with your own (https://www.luis.ai/home):
    /// </summary>
    [LuisModel("MODEL_ID", "SUBSCRIPTION_KEY")]
    [Serializable]
    public class RootDialog : LuisDialog<object>
    {
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"Understand '{result.Query}' I did not. The force is not strong with this one ...";
            await context.PostAsync(message);
            context.Wait(this.MessageReceived);
        }

        [LuisIntent("SearchRecipes")]
        [LuisIntent("SearchRecipesByCuisineType")]
        [LuisIntent("SearchRecipesByIngredient")]
        public async Task SearchRecipes(IDialogContext context, LuisResult result)
        {
            List<string> searchParameters = new List<string>();
            if (result.Entities.Count > 0)
            {
                string message = "Search for";
                EntityRecommendation cuisineTypeEntity;
                if (result.TryFindEntity(CustomEntity.CuisineType, out cuisineTypeEntity))
                {
                    searchParameters.Add(cuisineTypeEntity.Entity);
                    message += " " + cuisineTypeEntity.Entity;
                }

                message += " recipes";
                EntityRecommendation ingredientEntity;
                if (result.TryFindEntity(CustomEntity.Ingredient, out ingredientEntity))
                {
                    searchParameters.Add(ingredientEntity.Entity);
                    message += " with " + ingredientEntity.Entity;
                }
                
                EntityRecommendation recipeEntity;
                if (result.TryFindEntity(CustomEntity.Recipe, out recipeEntity))
                {
                    searchParameters.Add(recipeEntity.Entity);
                }

                message += " I will...";
                await context.PostAsync(message);
            }

            if (searchParameters.Any())
            {
                SearchRecipesManager sm = new SearchRecipesManager();
                var results = await sm.GetRecipesAsync(searchParameters);
                if (results != null)
                {
                    if (results.Any())
                    {
                        int numResultsShow = Math.Min(15, results.Count);

                        List<Recipe> recipes = results.Take(numResultsShow).ToList();
                        await context.PostAsync($"Found {recipes.Count()} recipes I have:");

                        var resultMessage = context.MakeMessage();
                        resultMessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                        resultMessage.Attachments = new List<Attachment>();

                        foreach (var recipe in recipes)
                        {
                            HeroCard heroCard = new HeroCard()
                            {
                                Title = recipe.label,
                                Subtitle = recipe.source,
                                Images = new List<CardImage>()
                        {
                            new CardImage() { Url = recipe.image }
                        },
                                Buttons = new List<CardAction>()
                        {
                            new CardAction()
                            {
                                Title = "Recipe",
                                Type = ActionTypes.OpenUrl,
                                Value = recipe.url
                            }
                        }
                            };

                            resultMessage.Attachments.Add(heroCard.ToAttachment());
                        }
                        await context.PostAsync(resultMessage);
                    }
                    else
                        await context.PostAsync("No recipes I have found");
                }
                else
                    await context.PostAsync("Invalid credentials. Go to https://developer.edamam.com/edamam-recipe-api to request free API_ID and APP_KEY and set values in SearchRecipesManager ...");
            }
            else
                await context.PostAsync("Understand I did not. Help I need to search for recipes. Type 'Search for ... recipes' you must try.");
            context.Wait(this.MessageReceived);
        }
        
        [LuisIntent("DarkSideQuestions")]
        public async Task DarkSideQuestions(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("The dark side clouds everything, search for recipes is the way of the light.");

            context.Wait(this.MessageReceived);
        }

        
    }
}