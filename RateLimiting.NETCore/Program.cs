
using Microsoft.AspNetCore.RateLimiting;

namespace RateLimiting.NETCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddRateLimiter(option =>
            {
                option.AddFixedWindowLimiter("FixedWindoPolicy", opt =>
                {
                    opt.Window = TimeSpan.FromSeconds(10);
                    opt.PermitLimit = 1;
                    opt.QueueLimit = 1;
                    opt.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;

                }).RejectionStatusCode = 429; // Too Mant Reqyest;
            });

            builder.Services.AddRateLimiter(option =>
            {
                option.AddSlidingWindowLimiter("SlidingWindowPolicy",opt =>
                {
                    opt.Window = TimeSpan.FromSeconds(10);
                    opt.PermitLimit= 4;
                    opt.QueueLimit = 3;
                    opt.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
                    opt.SegmentsPerWindow = 3;
                }).RejectionStatusCode = 429;
            });


            builder.Services.AddRateLimiter(option =>
            {
                option.AddConcurrencyLimiter("ConcurrencyPoliciy", opt =>
                {
                    opt.PermitLimit = 1;
                    opt.QueueLimit = 10;
                    opt.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst; 
                   
                }).RejectionStatusCode = 429;
            });

            builder.Services.AddRateLimiter(option =>
            {
                option.AddTokenBucketLimiter("TokenBucketPolicy", opt =>
                {
                    opt.TokenLimit = 4;
                    opt.QueueLimit = 2;
                    opt.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
                    opt.ReplenishmentPeriod = TimeSpan.FromSeconds(10);
                    opt.TokensPerPeriod = 4;
                    opt.AutoReplenishment = true;
                }).RejectionStatusCode = 429;
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseRateLimiter();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
