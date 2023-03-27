namespace MSLServer.Models
{
    public static class Resource
    {


        public static string workingDirectory = Environment.CurrentDirectory;
        public static string projectDirectory = Directory.GetParent(workingDirectory).FullName;
        public static string FileDirectory = projectDirectory+ @"\\Resources\\Files\\";


        public static string thumbnailDirectory = FileDirectory + @"\ServerThumbnails";
        public static string logoDirectory = FileDirectory + @"\ServerLogos";

        public static string FilePath = "";
        public static string Criptkey = "nightworksfirstprojectvalidationkeyforrulez";
    }
}
