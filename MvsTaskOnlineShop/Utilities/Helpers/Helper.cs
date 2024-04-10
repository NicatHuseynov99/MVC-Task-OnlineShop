namespace MvsTaskOnlineShop.Utilities.Helpers
{
    public class Helper
    {
        public static string GetFilePath(string root, string folder, string fileName)
        {
            return Path.Combine(root, folder, fileName);
        }
    }
}
