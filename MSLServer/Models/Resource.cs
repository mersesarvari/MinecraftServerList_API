namespace MSLServer.Models
{
    public static class Resource
    {
        public static string FileDirectory = @"E:\\Programing\\MSLClient\\public\\Resources\\Files\\";


        public static string workingDirectory = Environment.CurrentDirectory;
        public static string projectDirectory = Directory.GetParent(workingDirectory).FullName;


        public static string thumbnailDirectory = FileDirectory+ @"\ServerThumbnails";
        public static string logoDirectory = FileDirectory + @"\ServerLogos";

        public static string FilePath = "";
        public static string Criptkey = "nightworksfirstprojectvalidationkeyforrulez";
    }
}
