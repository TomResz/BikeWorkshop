using BikeWorkshop.Application.Interfaces.Email;

namespace BikeWorkshop.Infrastructure.Email.Contents;

internal sealed class CreateOrderEmailContent : ICreateOrderEmailContent
{
	public string Content(string url, string shortId)
		=> $@"
            <!DOCTYPE html>
            <html lang=""pl"">
            <head>
                <meta charset=""UTF-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                <title>Created Order</title>
                <style>
                    p {{
                        font-family: 'Times New Roman', Times, serif;
                        font-size: 25px;
                    }}
                    b {{
                        font-style: italic;
                    }}
                </style>
            </head>
            <body>
                <p>Your order has been already created!</p>
                <p>This is your tracking code: <b>{shortId}</b></p>
                <p>You can track your order there: <a href=""http:/{url}"">{url}</a></p>
            </body>
            </html>";
}
