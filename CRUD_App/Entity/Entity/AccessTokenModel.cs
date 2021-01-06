using System;
using System.Collections.Generic;
using System.Text;

namespace Go2Share.Entity.Entity
{
    public class AccessTokenModel
    {

        public string Token { get; set; }
        public string refresh_token { get; set; }
        public string token_type { get; set; }
        public long expires_in { get; set; }
    }
}
