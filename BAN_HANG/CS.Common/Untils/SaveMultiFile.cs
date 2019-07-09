using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CS.Common.Untils
{
    public class SaveMultiFile
    {
        public static string SaveFile(HttpPostedFileBase imageFile, string url, List<string> ext)
        {
            try
            {
                string extension = System.IO.Path.GetExtension(imageFile.FileName.ToLower());
                var check = ext.FirstOrDefault(ob => ob.ToLower() == extension);
                if (check == null)
                {
                    return null;
                }
                string imageUrl = url + Guid.NewGuid() + extension;
                imageFile.SaveAs(AppDomain.CurrentDomain.BaseDirectory + imageUrl);
                return imageUrl;
            }
            catch
            {
                return null;
            }
        }
    }

}
