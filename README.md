# BotBuilder.Luis.Translator
Allows the LUIS BotBuilder framework to be used in other languages than those supported by LUIS (by way of the Translations Services)

## Usage

1 Instead of having your Dialog inherit from `LuisDialog<T>` you inherit from `TranslatableDialog<T>` (which inherits `LuisDialog<T>`)
1 Decorate your Dialog class with `[Translatable(SubscriptionKey="", From="", To")]`. `SubscriptionKey` is you Azure subscription Guid, 
`From` is the locale you are translating from, `To` is the locale you are translating to (usually English)

## `From="auto"` 
If you set `From="auto"` then it will pull the locale from the message from the client

