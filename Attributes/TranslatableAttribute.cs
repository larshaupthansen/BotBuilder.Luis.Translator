using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotBuilder.Luis.Translator.Attributes
{

    public interface ITranslateOptions {

        string SubscriptionKey { get;  }

        string From { get;  }

        string To { get; }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]


    [Serializable]
    public class TranslatableAttribute : System.Attribute, ITranslateOptions
    {
        public string SubscriptionKey { get; set; }

        public string From { get; set; }

        public string To { get; set; }
    }
}
