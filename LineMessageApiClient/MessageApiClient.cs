using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace LineMessageApiClient
{
    public interface ILineBot
    {
        void RunAsync();
    }
    internal static class WebhookRequestMessageHelper
    {
        internal static async Task<IEnumerable<WebhookEvent>> GetWebhookEventsAsync(this HttpRequest request, string channelSecret, string botUserId = null)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (channelSecret == null) { throw new ArgumentNullException(nameof(channelSecret)); }

            var content = "";
            using (var reader = new StreamReader(request.Body))
            {
                content = await reader.ReadToEndAsync();
            }

            var xLineSignature = request.Headers["X-Line-Signature"].ToString();
            if (string.IsNullOrEmpty(xLineSignature) || !VerifySignature(channelSecret, xLineSignature, content))
            {
                throw new InvalidSignatureException("Signature validation faild.");
            }

            dynamic json = JsonConvert.DeserializeObject(content);

            if (!string.IsNullOrEmpty(botUserId))
            {
                if (botUserId != (string)json.destination)
                {
                    throw new UserIdMismatchException("Bot user ID does not match.");
                }
            }
            return WebhookEventParser.ParseEvents(json.events);
        }

        internal static bool VerifySignature(string channelSecret, string xLineSignature, string requestBody)
        {
            try
            {
                var key = Encoding.UTF8.GetBytes(channelSecret);
                var body = Encoding.UTF8.GetBytes(requestBody);

                using (HMACSHA256 hmac = new HMACSHA256(key))
                {
                    var hash = hmac.ComputeHash(body, 0, body.Length);
                    var xLineBytes = Convert.FromBase64String(xLineSignature);
                    return SlowEquals(xLineBytes, hash);
                }
            }
            catch
            {
                return false;
            }
        }

        private static bool SlowEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
                diff |= (uint)(a[i] ^ b[i]);
            return diff == 0;
        }
    }
    public class LineBotApp : WebhookApplication, ILineBot
    {
        private readonly IConfiguration _config;
        private readonly LineMessagingClient _messagingClient;
        private readonly IEnumerable<WebhookEvent> _event;
        public LineBotApp(IHttpContextAccessor HttpContextAccessor, IConfiguration config)
        {
            _config = config;
            _event = HttpContextAccessor.HttpContext.Request.GetWebhookEventsAsync(_config.GetSection("channelSecret").Value).GetAwaiter().GetResult();
            _messagingClient = new LineMessagingClient(_config.GetSection("accessToken").Value);
          
        }
        public void RunAsync()
        {
             this.RunAsync(_event).GetAwaiter().GetResult();
        }
        protected override async Task OnMessageAsync(MessageEvent ev)
        {
            //FlexMessage Editor https://developers.line.biz/flex-simulator/
            //FlexMessage Convert https://fysh711426.github.io/flexMessageConvert/index.html
            var result = new List<ISendMessage>();

            switch (ev.Message.Type)
            {
                case EventMessageType.Text:
                    result = new List<ISendMessage>
                {
                     new FlexMessage("AltText")
                     {
                          Contents = new CarouselContainer
                          {

                                Contents = new List<BubbleContainer>()
                                {
                                       new BubbleContainer()
            {
                  Hero = new ImageComponent
        {
            Url = "https://ithelp.ithome.com.tw/upload/images/20200106/20106865q03SKAqv0U.png",
            Size = ComponentSize.Full,
            AspectRatio = new AspectRatio(151, 100),
            AspectMode = AspectMode.Cover,
            BackgroundColor = "#FFFFFF",

        },
                 Body =    new BoxComponent
        {
            Layout = BoxLayout.Vertical,
            Contents = new List<IFlexComponent>
            {
                new TextComponent
                {
                    Text = "Title",
                    Weight = Weight.Bold,
                    Size = ComponentSize.Xl
                },
                new TextComponent
                {
                    Text = "Content Text",
                    Size = ComponentSize.Md,
                    Color = "#9C9C9C"
                }
            },

            }


        },
                                       new BubbleContainer()
                                       {
                                             Hero = new ImageComponent
        {
            Url = "https://ithelp.ithome.com.tw/upload/images/20200106/20106865q03SKAqv0U.png",
            Size = ComponentSize.Full,
            AspectRatio = new AspectRatio(151, 100),
            AspectMode = AspectMode.Cover,
            BackgroundColor = "#FFFFFF",

        },
                                             Body = new BoxComponent
        {
            Layout = BoxLayout.Vertical,
            Contents = new List<IFlexComponent>
            {
                new TextComponent
                {
                    Text = "Title",
                    Weight = Weight.Bold,
                    Size = ComponentSize.Xl
                },
                new TextComponent
                {
                    Text = "Content Text",
                    Size = ComponentSize.Md,
                    Color = "#9C9C9C"
                }
            },

            }
                                       },
                                },

                          }
                     }
                };
                    break;
                case EventMessageType.Location:
                    //var longitude = ev.Message.GetType().GetProperty("Longitude")?.GetValue(ev.Message)?.ToString();
                    //var Latitude = ev.Message.GetType().GetProperty("Latitude")?.GetValue(ev.Message)?.ToString();
                    //var Address = ev.Message.GetType().GetProperty("Address")?.GetValue(ev.Message)?.ToString();

                    // var ddd =  new HttpClient().GetAsync($"https://maps.googleapis.com/maps/api/place/textsearch/json?query={Address} 附近餐廳&radius=1000&key=AIzaSyBoI8mRu2B4RZEfCjSE61WjBS8G1dUFu-w").GetAwaiter().GetResult();
                    //var ccc = JsonConvert.DeserializeObject<GoogleTextSearchResponse>(ddd.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                    //var topfive = ccc.results.Take(10);
                    //var bubbles = new List<BubbleContainer>();

                    //topfive.ToList().ForEach(x =>
                    //{

                    //    x.photos.ForEach(y =>
                    //    {
                    //        var photo = new HttpClient().GetAsync($"https://maps.googleapis.com/maps/api/place/photo?maxwidth=400&photo_reference={y.photo_reference}&key=AIzaSyBoI8mRu2B4RZEfCjSE61WjBS8G1dUFu-w").GetAwaiter().GetResult();

                    //        var img = new ImageComponent
                    //        {
                    //            Url = $"https://maps.googleapis.com/maps/api/place/photo?maxwidth=400&photo_reference={y.photo_reference}&key=AIzaSyBoI8mRu2B4RZEfCjSE61WjBS8G1dUFu-w",
                    //            Size = ComponentSize.Full,
                    //            AspectRatio = new AspectRatio(151, 100),
                    //            AspectMode = AspectMode.Cover,
                    //            BackgroundColor = "#FFFFFF",

                    //        };
                    //        var body = new BoxComponent
                    //        {
                    //            Layout = BoxLayout.Vertical,
                    //            Contents = new List<IFlexComponent>
                    //            {
                    //                 new TextComponent
                    //                 {
                    //                      Text = x.name,
                    //                      Weight = Weight.Bold,
                    //                      Size = ComponentSize.Xl
                    //                 },
                    //                 new TextComponent
                    //                 {
                    //                    Text = x.formatted_address,
                    //                    Size = ComponentSize.Md,
                    //                    Color = "#9C9C9C"
                    //                 },

                    //            },

                    //        };

                    //        var Details = new HttpClient().GetAsync($"https://maps.googleapis.com/maps/api/place/details/json?place_id={x.place_id}&key=AIzaSyBoI8mRu2B4RZEfCjSE61WjBS8G1dUFu-w").GetAwaiter().GetResult();
                    //        var moreinfo = JsonConvert.DeserializeObject<GooglePlaceDetailResponse>(ddd.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                    //        var footer = new BoxComponent()
                    //        {
                    //            Layout = BoxLayout.Horizontal,
                    //            Contents = new List<IFlexComponent>
                    //            {
                    //              new ButtonComponent
                    //              {
                    //                  Action = new UriTemplateAction("Go!!!", moreinfo.result.url),
                    //              }
                    //            }
                    //        };

                    //        bubbles.Add(new BubbleContainer()
                    //        {
                    //            Hero = img,
                    //            Body = body,
                    //            Footer = footer
                    //        });
                    //    });
                    //});
                    //result = new List<ISendMessage>
                    //{
                    //     new FlexMessage("附近的餐廳 1 公里以內")
                    //     {
                    //          Contents = new CarouselContainer
                    //          { 
                    //             Contents = bubbles,


                    //          }
                    //     }

                    //};

                    break;
                case EventMessageType.Image:
                    break;
                case EventMessageType.Audio:
                    break;

            }

            if (result != null)
                await _messagingClient.ReplyMessageAsync(ev.ReplyToken, result);
        }
    }
    public static class MessageApiClient
    {
        public static IServiceCollection AddLineMessageApiClient(this IServiceCollection service)
        {
            service.AddScoped<ILineBot,LineBotApp>();
            return service;
        }
    }
}