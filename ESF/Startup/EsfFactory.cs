namespace ESF.Startup
{
    public static class EsfFactory
    {
        public static IEsfWebApplicationBuilder Build(string[] args)
        {
            var webApplicationBuilder = WebApplication.CreateBuilder(args);
            return new EsfWebApplicationBuilder(webApplicationBuilder);
        }
    }
}