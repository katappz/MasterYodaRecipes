# Master Yoda Recipes

Microsoft Bot framework example that has Master Yoda search recipes for you :) Made this for fun and to learn and decided to share. See video with example:

https://github.com/katappz/MasterYodaRecipes/blob/master/MasterYoda/Example/Master%20Yoda%20Recipes%20Example.mp4

Requires registration on Microsoft Cognitive Services (LUIS) and you also have to request a free API key with Edamam (see Prerequisites section). 

### Prerequisites

Requires registration on Microsoft Cognitive Services (LUIS):
https://www.luis.ai/home

You also have to request a free API key with Edamam (recipe search API):
https://developer.edamam.com/edamam-recipe-api


### Installing

After registering in LUIS.ai portal import the app's model by clicking on "Import new app" (https://www.luis.ai/applications) 
Then choose the model's json file that is in (https://github.com/katappz/MasterYodaRecipes/blob/master/MasterYoda/Example/Master%20Yoda%20Recipes.json). 

Train and publish your model (You can see how in https://docs.microsoft.com/en-us/azure/cognitive-services/luis/luis-how-to-train).

Then you'll have your model ID and Subscription Key for your app.
Copy and paste these into the project's RootDialog.cs class:

 ```
/// <summary>
/// Change Model ID and Subscription Key below with your own (https://www.luis.ai/home):
/// </summary>
[LuisModel("MODEL_ID", "SUBSCRIPTION_KEY")]
```

After registering for a free API key with Edamam (https://developer.edamam.com/edamam-recipe-api) you'll have your API_ID and APP_KEY. 
Copy and paste these into the project's SearchRecipesManager.cs class:

```
// Go to https://developer.edamam.com/edamam-recipe-api to request free API_ID and APP_KEY and set values below
  private string appID = "[API_ID]";
  private string appKey = "[APP_KEY]";
```

Save, build and run and then you're ready to use on BotFramework emulator :)

## Authors

* **Katherine Batista** - [katappz](https://github.com/katappz)

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details
