using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Handler
{
    public class FileHandler
    {
        public static string GetFileIcon(string fileName)
        {
            var ext = Path.GetExtension(fileName)?.ToLower();

            return ext switch
            {
                ".pdf" => "📄",
                ".jpg" => "🖼️",
                ".png" => "🖼️",
                ".docx" => "📝",
                _ => "📎"
            };
        }
    }
}
