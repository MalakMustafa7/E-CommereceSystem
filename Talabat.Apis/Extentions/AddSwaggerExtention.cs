﻿namespace Talabat.Apis.Extentions
{
    public static class AddSwaggerExtention
    {
        public static WebApplication UseSwaggerMiddleWares(this WebApplication app) {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }
    }
}
