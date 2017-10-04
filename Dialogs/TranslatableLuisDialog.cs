using BotBuilder.Luis.Translator.Attributes;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BotBuilder.Luis.Translator.Dialogs
{
    [Serializable]

    public class TranslatableLuisDialog<T> : LuisDialog<T>
    {
        private const string OcpApimSubscriptionKeyHeader = "Ocp-Apim-Subscription-Key";

        private const string TranslateUrlTemplate = "http://api.microsofttranslator.com/v2/http.svc/translate?text={0}&from={1}&to={2}&category={3}";

        

        ITranslateOptions options = null;
        public TranslatableLuisDialog(params ILuisService[] services) : base(services) {

            this.options = GetTranslatableAttribute();
        }

        public ITranslateOptions GetTranslatableAttribute()

        {

            var type = this.GetType();

            var options  = type.GetCustomAttributes(typeof(TranslatableAttribute), true);
            return (ITranslateOptions)(options?.Length>0 ? options[0]: null);


        }
        protected override async Task MessageReceived(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            var message = await item;
            if (this.options != null) {
                var fromLanguage = this.options.From;
                var toLanguage = this.options.To;
                if (fromLanguage.Equals("auto"))
                {
                    fromLanguage = message.Locale;
                }


                message.Text = await translate(message.Text, fromLanguage, toLanguage);
            }
            await base.MessageReceived(context, item);
        }

        private async Task<string> translate(string text, string fromLanguage, string toLanguage)
        {
            var translateResponse = await TranslateRequest(string.Format(TranslateUrlTemplate, text, 
                fromLanguage, toLanguage, "general"));

            var translateResponseContent = await translateResponse.Content.ReadAsStringAsync();

            if (translateResponse.IsSuccessStatusCode)

            {
                var gt = translateResponseContent.IndexOf('>');
                var lt = translateResponseContent.IndexOf('<', gt);
                var transtext = translateResponseContent.Substring(gt + 1, lt - gt - 1);
                Console.WriteLine("Translation result: {0}", translateResponseContent);
                return transtext;
            }

            else

            {

                Console.Error.WriteLine("Failed to translate. Response: {0}", translateResponseContent);
                return text;
            }
        }

        public async Task<HttpResponseMessage> TranslateRequest(string url)

        {

            using (HttpClient client = new HttpClient())

            {

                client.DefaultRequestHeaders.Add(OcpApimSubscriptionKeyHeader, this.options.SubscriptionKey);

                return await client.GetAsync(url);

            }

        }
    }
}
