using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using log4net.Appender;
using log4net.Core;
using log4net.Util;

namespace NewRelicInsights.Log4Net
{
    public class NewRelicInsightsAppender : AppenderSkeleton
    {
        private readonly HttpClient client;
        private string applicationJson = "application/json";
        public string ApiEndPoint { get; set; }
        public string InsertKey { get; set; }
        private Uri Uri { get; set; }

        public NewRelicInsightsAppender()
        {
            client = new HttpClient();
        }

        public override void ActivateOptions()
        {
            base.ActivateOptions();

            Uri = new Uri(ApiEndPoint);

            client.DefaultRequestHeaders.Accept.Clear();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(applicationJson));
            client.DefaultRequestHeaders.Add("X-Insert-Key", InsertKey);
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            var payload = this.RenderLoggingEvent(loggingEvent);
            PostEvent(payload).Wait();
        }

        private async Task PostEvent(string payload)
        {
            try
            {
                var content = new StringContent(payload, System.Text.Encoding.UTF8, applicationJson);
                var response = await client.PostAsync(Uri, content);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                this.ErrorHandler.Error($"Unable to send logging event to remote host {this.ApiEndPoint}", ex);
            }
        }
    }
}