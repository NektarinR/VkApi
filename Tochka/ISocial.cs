using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tochka
{
    public interface ISocial
    {
        ICollection<string> GetPosts(string Name, int count);
        
        void Auth(string login, string password);
        
        void SendPostToMe(string data);
    }
}
