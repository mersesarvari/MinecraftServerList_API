namespace MSLServer.Models
{
    public static class Resource
    {

        public static string workingDirectory = Environment.CurrentDirectory;
        public static string projectDirectory = Directory.GetParent(workingDirectory).FullName;
        public static string fileDirectory = Directory.GetParent(workingDirectory).FullName + @"E:\Programing\MSLClient\public\Resources\Files\ServerThumbnails";

        public static string FilePath = "";
        public static string Criptkey = "nightworksfirstprojectvalidationkeyforrulez";
    }
}
