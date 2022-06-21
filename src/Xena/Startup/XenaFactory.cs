namespace Xena.Startup
{
    public static class XenaFactory
    {
        public static IXenaWebApplicationBuilder Build(string[] args)
        {
            var webApplicationBuilder = WebApplication.CreateBuilder(args);
            return new XenaWebApplicationBuilder(webApplicationBuilder);
        }
    }
}