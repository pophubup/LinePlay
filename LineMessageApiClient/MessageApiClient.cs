using GooglePlaceDetailClient;
using GoogleTextSerachClient;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace LineMessageApiClient
{
    public interface ILineBot
    {
        string RunAsync(HttpContext httpContext);
    }
    internal static class WebhookRequestMessageHelper
    {
        internal static async Task<(IEnumerable<WebhookEvent>, LineMessagingClient)> GetWebhookEventsAsync(this HttpRequest request, IConfiguration iconfig , string botUserId = null)
        {
            string channelSecret = iconfig["LineBot:channelSecret"];
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
            return ( WebhookEventParser.ParseEvents(json.events) , new LineMessagingClient(iconfig["LineBot:accessToken"]));
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
        private  LineMessagingClient _messagingClient = default!;
        private IGooglePlaceDetail _googlePlaceDetail;
        private IGoogleTextSearch _googleTextSearch;
        public LineBotApp(IConfiguration config, IGooglePlaceDetail googlePlaceDetail, IGoogleTextSearch googleTextSearch)
        {
            _config = config;
            _googlePlaceDetail = googlePlaceDetail;
            _googleTextSearch = googleTextSearch;
        }
        public string RunAsync( HttpContext httpContext)
        {
            var ( _event, messagingClient) =  httpContext.Request.GetWebhookEventsAsync(_config).GetAwaiter().GetResult();
            _messagingClient = messagingClient;
            base.RunAsync(_event).GetAwaiter().GetResult();
            return  "Success";
        }
        protected override async Task OnMessageAsync(MessageEvent ev)
        {
            //FlexMessage Editor https://developers.line.biz/flex-simulator/
            //FlexMessage Convert https://fysh711426.github.io/flexMessageConvert/index.html
            var result = new List<ISendMessage>();

            switch (ev.Message.Type)
            {
                case EventMessageType.Text:
                    var richMenu = new RichMenu()
                    {
                        Size = new ImagemapSize(2500, 843),
                        Selected = false,
                        Name = "richmenu-1",
                        ChatBarText = "選單1",
                        Areas = new List<ActionArea>()
    {
        new ActionArea()
        {
            Bounds = new ImagemapArea(0, 0 ,833, 843),
            Action = new MessageTemplateAction("文字", "Hello, iBot!")
        },
        new ActionArea()
        {
            Bounds = new ImagemapArea(833, 0 ,833, 843),
            Action = new UriTemplateAction("網址", "https://ithelp.ithome.com.tw/users/20106865/ironman/2732")
        },
        new ActionArea()
        {
            Bounds = new ImagemapArea(1666, 0 ,833, 843),
            Action = new PostbackTemplateAction("選單2", "action=changeMenu2")
        }
    }
                    };
                    await _messagingClient.CreateRichMenuAsync(richMenu);
                    break;
                case EventMessageType.Location:
                    var Address = ev.Message.GetType().GetProperty("Address")?.GetValue(ev.Message)?.ToString();
                    var texts = _googleTextSearch.GetTextSearch(Address, "附近餐廳");
                    var cards = texts?.results.Take(10).Select(x =>
                    {
                        var detail = _googlePlaceDetail.GetPlaceDetail(x.place_id)?.result;
                      var second = detail?.opening_hours?.weekday_text.Select(x=> new TextComponent()
                      {
                          Text = x,
                          Wrap = true,
                          Align = Align.Start,
                          Size = ComponentSize.Sm,
                          Color = "#9C9C9C"
                      }).ToList();
                        var first = new TextComponent
                        {
                            Text = detail?.formatted_address,
                            Size = ComponentSize.Md,
                             Wrap = true,
                            Weight = Weight.Bold,
                            Color = "#9C9C9C"
                        };
                        var body = new List<IFlexComponent>();
                        body.Add(first);
                        body.Add(new SeparatorComponent() { Margin = Spacing.Md });
                        body.AddRange(second);
                       
                       
                        return new BubbleContainer()
                        {
                            Hero = new ImageComponent
                            {
                                Url = $"https://maps.googleapis.com/maps/api/place/photo?maxwidth=400&photo_reference={x.photos.FirstOrDefault()?.photo_reference}&key={_config.GetSection("apikey").Value}",
                                Size = ComponentSize.Full,
                                AspectRatio = new AspectRatio(151, 100),
                                AspectMode = AspectMode.Cover,
                                BackgroundColor = "#FFFFFF",

                            },
                            Header = new BoxComponent
                            {
                                Layout = BoxLayout.Vertical,
                                Contents = new List<IFlexComponent>
                                {
                                    new TextComponent
                                     {
                                          Text = detail?.name,
                                          Weight = Weight.Bold,
                                          Size = ComponentSize.Xl
                                     },
                                }
                            },
                            Body = new BoxComponent
                            {
                                Layout = BoxLayout.Vertical,
                                Contents = body
                            },
                            Footer = new BoxComponent()
                            {
                                Layout = BoxLayout.Horizontal,
                                Contents = new List<IFlexComponent>
                                {
                                          new ButtonComponent
                                          {
                                              Action = new UriTemplateAction("Go!!!", detail?.url),
                                          }
                                }
                            }
                        };
                    }).ToList();
                    result = new List<ISendMessage>
                    {
                         new FlexMessage("附近的餐廳 1 公里以內")
                         {
                              Contents = new CarouselContainer
                              {
                                 Contents = cards,


                              }
                         }

                    };
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
            service.AddGooglePlaceDetailClient();
            service.AddGoogleTextSearchClient();
            service.AddTransient<ILineBot,LineBotApp>();
            return service;
        }
    }
}