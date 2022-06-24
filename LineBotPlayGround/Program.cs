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
builder.Services.AddGooglePlaceDetailClient();
builder.Services.AddGoogleTextSearchClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddLineMessageApiClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapPost("/POST",  ( HttpContext httpContext ) =>
{

    //    var ChannelAccessToken = builder.Configuration.GetSection("accessToken").Value; //Channel Access Token
    //    var UserId = "U8e75dde4f4dddc3510f7a37200531788"; //收訊者
    //    var bot = new isRock.LineBot.Bot(ChannelAccessToken);
    //    var flexContents = @"
    //{
    //  ""type"": ""bubble"",
    //  ""hero"": {
    //    ""type"": ""image"",
    //    ""url"": ""https://scdn.line-apps.com/n/channel_devcenter/img/fx/01_1_cafe.png"",
    //    ""size"": ""full"",
    //    ""aspectRatio"": ""20:13"",
    //    ""aspectMode"": ""cover"",
    //    ""action"": {
    //      ""type"": ""uri"",
    //      ""uri"": ""http://linecorp.com/""
    //    }
    //  },
    //  ""body"": {
    //    ""type"": ""box"",
    //    ""layout"": ""vertical"",
    //    ""contents"": [
    //      {
    //        ""type"": ""text"",
    //        ""text"": ""Brown Cafe"",
    //        ""weight"": ""bold"",
    //        ""size"": ""xl""
    //      },
    //      {
    //        ""type"": ""box"",
    //        ""layout"": ""baseline"",
    //        ""margin"": ""md"",
    //        ""contents"": [
    //          {
    //            ""type"": ""icon"",
    //            ""size"": ""sm"",
    //            ""url"": ""https://scdn.line-apps.com/n/channel_devcenter/img/fx/review_gold_star_28.png""
    //          },
    //          {
    //            ""type"": ""icon"",
    //            ""size"": ""sm"",
    //            ""url"": ""https://scdn.line-apps.com/n/channel_devcenter/img/fx/review_gold_star_28.png""
    //          },
    //          {
    //            ""type"": ""icon"",
    //            ""size"": ""sm"",
    //            ""url"": ""https://scdn.line-apps.com/n/channel_devcenter/img/fx/review_gold_star_28.png""
    //          },
    //          {
    //            ""type"": ""icon"",
    //            ""size"": ""sm"",
    //            ""url"": ""https://scdn.line-apps.com/n/channel_devcenter/img/fx/review_gold_star_28.png""
    //          },
    //          {
    //            ""type"": ""icon"",
    //            ""size"": ""sm"",
    //            ""url"": ""https://scdn.line-apps.com/n/channel_devcenter/img/fx/review_gray_star_28.png""
    //          },
    //          {
    //            ""type"": ""text"",
    //            ""text"": ""4.0"",
    //            ""size"": ""sm"",
    //            ""color"": ""#999999"",
    //            ""margin"": ""md"",
    //            ""flex"": 0
    //          }
    //        ]
    //      },
    //      {
    //        ""type"": ""box"",
    //        ""layout"": ""vertical"",
    //        ""margin"": ""lg"",
    //        ""spacing"": ""sm"",
    //        ""contents"": [
    //          {
    //            ""type"": ""box"",
    //            ""layout"": ""baseline"",
    //            ""spacing"": ""sm"",
    //            ""contents"": [
    //              {
    //                ""type"": ""text"",
    //                ""text"": ""Place"",
    //                ""color"": ""#aaaaaa"",
    //                ""size"": ""sm"",
    //                ""flex"": 1
    //              },
    //              {
    //                ""type"": ""text"",
    //                ""text"": ""Miraina Tower, 4-1-6 Shinjuku, Tokyo"",
    //                ""wrap"": true,
    //                ""color"": ""#666666"",
    //                ""size"": ""sm"",
    //                ""flex"": 5
    //              }
    //            ]
    //          },
    //          {
    //            ""type"": ""box"",
    //            ""layout"": ""baseline"",
    //            ""spacing"": ""sm"",
    //            ""contents"": [
    //              {
    //                ""type"": ""text"",
    //                ""text"": ""Time"",
    //                ""color"": ""#aaaaaa"",
    //                ""size"": ""sm"",
    //                ""flex"": 1
    //              },
    //              {
    //                ""type"": ""text"",
    //                ""text"": ""10:00 - 23:00"",
    //                ""wrap"": true,
    //                ""color"": ""#666666"",
    //                ""size"": ""sm"",
    //                ""flex"": 5
    //              }
    //            ]
    //          }
    //        ]
    //      }
    //    ]
    //  },
    //  ""footer"": {
    //    ""type"": ""box"",
    //    ""layout"": ""vertical"",
    //    ""spacing"": ""sm"",
    //    ""contents"": [
    //      {
    //        ""type"": ""button"",
    //        ""style"": ""link"",
    //        ""height"": ""sm"",
    //        ""action"": {
    //          ""type"": ""uri"",
    //          ""label"": ""CALL"",
    //          ""uri"": ""https://linecorp.com""
    //        }
    //      },
    //      {
    //        ""type"": ""button"",
    //        ""style"": ""link"",
    //        ""height"": ""sm"",
    //        ""action"": {
    //          ""type"": ""uri"",
    //          ""label"": ""WEBSITE"",
    //          ""uri"": ""https://linecorp.com""
    //        }
    //      },
    //      {
    //        ""type"": ""spacer"",
    //        ""size"": ""sm""
    //      }
    //    ],
    //    ""flex"": 0
    //  }
    //}
    //        ";
    //    //定義一則訊息
    //    var Messages = @"
    //                [
    //                {
    //                ""type"": ""flex"",
    //                ""altText"": ""This is a Flex Message"",
    //                ""contents"": $flex$
    //                }
    //                ]
    //            ";

    //    //替換Flex Contents
    //    var MessageJSON = Messages.Replace("$flex$", flexContents);
    //    bot.PushMessageWithJSON(UserId, MessageJSON);
    
     builder.Services?.BuildServiceProvider()?.GetService<ILineBot>()?.RunAsync();

    return "Success";

}).WithName("POST");


app.Run();
