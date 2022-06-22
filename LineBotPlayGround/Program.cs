using Line.Messaging;
using Line.Messaging.Webhooks;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapPost("/POST", async (HttpContext httpContext) =>
{

   

        var events = await httpContext.Request.GetWebhookEventsAsync(builder.Configuration.GetSection("channelSecret").Value);
        var lineMessagingClient = new LineMessagingClient(builder.Configuration.GetSection("accessToken").Value);
        var lineBotApp = new LineBotApp(lineMessagingClient);
         await lineBotApp.RunAsync(events);


    return "Success";

}).WithName("POST");


app.Run();
static class WebhookRequestMessageHelper
{
    public static async Task<IEnumerable<WebhookEvent>> GetWebhookEventsAsync(this HttpRequest request, string channelSecret, string botUserId = null)
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
class LineBotApp : WebhookApplication
{
    private readonly LineMessagingClient _messagingClient;
    public LineBotApp(LineMessagingClient lineMessagingClient)
    {
        _messagingClient = lineMessagingClient;
    }

    protected override async Task OnMessageAsync(MessageEvent ev)
    {
        var result = null as List<ISendMessage>;

        switch (ev.Message.Type)
        {
            case EventMessageType.Text:

                //回傳 hellow
               var container = new BubbleContainer()
                {
                   
                    Hero = new ImageComponent()
                    {
                        Size = ComponentSize.Full,
                        Url = "https://scdn.line-apps.com/n/channel_devcenter/img/fx/01_1_cafe.png"
                    },
                    Body = new BoxComponent()
                    {
                        Layout = BoxLayout.Vertical,
                        Contents = new IFlexComponent[]
                        {
                              new TextComponent
                           {
                              Text = $"{ ((TextEventMessage)ev.Message).Text}",
                                 Weight = Weight.Bold,
                               Size = ComponentSize.Xl
                           },
                           new TextComponent
                           {
                              Text = $"哈ㄏ哈ㄏ",
                               Size = ComponentSize.Md
                           },
                          new SeparatorComponent(),
                           new TextComponent
                           {
                              Text = $"ㄟㄟㄟㄟ",
                               Size = ComponentSize.Md
                           },
                        }
                    },
                     
                };
                
                result = new List<ISendMessage>
                        {
                           new FlexMessage("文章排名")
                           {
                              Contents = container
                            },
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