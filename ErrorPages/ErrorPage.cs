using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace App.ErrorPages
{
    public static class ErrorPage
    {
        public static void ErrorsWebsite(this IApplicationBuilder app)
        {
            app.UseStatusCodePages(error =>
{
    error.Run(async context =>
    {
        var response = context.Response;
        var code = response.StatusCode;
        var content = @$"<html>
        <head>
           <meta charset = 'UTF-8' />
           <title>{code}-NotFound</title>
        </head>
        <body>
            <p style='color: Yellow; font-size: 30px'>
        lỗi không truy cập được : {code} - {(HttpStatusCode)code}
            </p>
        </body>
        </html>";

        await response.WriteAsync(content);
    });
});

        }
    }
}